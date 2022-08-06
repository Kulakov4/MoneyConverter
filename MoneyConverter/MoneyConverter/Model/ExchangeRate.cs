using MoneyConverter.Interfaces;

namespace MoneyConverter.Model;

public class ExchangeRate: IExchangeRate
{
    public ICurrency SourceCurrency { get; }
    public ICurrency DestinationCurrency { get; }
    public double Rate { get; }
    public ExchangeRate(Currency sourceCurrency, Currency destinationCurrency, double rate)
    {
        if (sourceCurrency.Equals(destinationCurrency))
            throw new ArgumentException("Invalid exchange rate.");
            
        if (rate <= 0)
            throw new ArgumentException("Exchange rate must be positive.");
            
        SourceCurrency = sourceCurrency;
        DestinationCurrency = destinationCurrency;
        Rate = rate;
    }

    public override string ToString() => $"1 {SourceCurrency.Name} = {Rate} {DestinationCurrency.Name}";
}