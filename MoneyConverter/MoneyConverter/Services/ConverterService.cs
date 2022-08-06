using MoneyConverter.Interfaces;
using MoneyConverter.Types;

namespace MoneyConverter.Services;

public static class ConverterService
{
    public static Money Convert(Money money, ICurrency destinationCurrency, IEnumerable<IExchangeRate> exchangeRates)
    {
        if (destinationCurrency == null) throw new ArgumentNullException(nameof(destinationCurrency));            
        if (exchangeRates == null) throw new ArgumentNullException(nameof(exchangeRates));
            
        if (money.Currency.Equals(destinationCurrency))
            return money;
            
        try
        {
            var result = GetExchangeRates(money, destinationCurrency, exchangeRates);
            return result.Money;
        }
        catch (NotSupportedException)
        {
            throw new Exception($"Cannot convert {money.Currency.Name} to {destinationCurrency.Name}");
        }

    }

    public readonly struct SearchResult
    {
        public readonly IEnumerable<IExchangeRate> ExchangeRates;
        public readonly Money Money;

        public SearchResult(IEnumerable<IExchangeRate> exchangeRates, Money money)
        {
            ExchangeRates = exchangeRates ?? throw new ArgumentNullException(nameof(exchangeRates));
            Money = money;
        }

        public bool IsEmpty => ExchangeRates == null;

        public static bool operator < (SearchResult lhs, SearchResult rhs) => lhs.Money.Value < rhs.Money.Value;  

        public static bool operator > (SearchResult lhs, SearchResult rhs) => lhs.Money.Value > rhs.Money.Value;
    }

    public static SearchResult GetExchangeRates(Money money, ICurrency destinationCurrency, IEnumerable<IExchangeRate> exchangeRates)
    {
        var exchangeRate = exchangeRates.FirstOrDefault(er => er.SourceCurrency.Equals(money.Currency) && er.DestinationCurrency.Equals(destinationCurrency));
        if (exchangeRate != null)
            return new SearchResult(new List<IExchangeRate> {exchangeRate}, money.Convert(exchangeRate));

        var otherExchangeRates = exchangeRates.Where(er => er.SourceCurrency.Equals(money.Currency));
        if (!otherExchangeRates.Any())
            throw new NotSupportedException();

        var result = new SearchResult();

        foreach (var firstExchangeRate in otherExchangeRates)
        {
            var filteredExchangeRates = exchangeRates.Where(e => !e.SourceCurrency.Equals(money.Currency) && !e.DestinationCurrency.Equals(money.Currency));
            try
            {
                var childRates = GetExchangeRates(money.Convert(firstExchangeRate), destinationCurrency, filteredExchangeRates);
                var rates = new List<IExchangeRate> { firstExchangeRate };
                rates.AddRange(childRates.ExchangeRates);
                var currentResult = new SearchResult(rates, childRates.Money);
                result = result.IsEmpty ? currentResult : (result > currentResult ? result : currentResult);
            }
            catch (NotSupportedException)
            { 
            }
        }

        if (result.IsEmpty)
            throw new NotSupportedException();

        return result;
    }

    private static Money Convert(this Money money, IExchangeRate exchangeRate) =>
        new Money(exchangeRate.DestinationCurrency, money.Value * exchangeRate.Rate);
}