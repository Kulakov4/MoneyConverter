using MoneyConverter.Interfaces;
using MoneyConverter.Types;

namespace MoneyConverter.Services;

public class Converter: IConverter
{
    private readonly IEnumerable<IExchangeRate> _exchangeRates;
    public Converter(IEnumerable<IExchangeRate> exchangeRates)
    {
        _exchangeRates = exchangeRates ?? throw new ArgumentNullException(nameof(exchangeRates));;
    }

    public Money Convert(Money money, ICurrency destinationCurrency) => ConverterService.Convert(money, destinationCurrency, _exchangeRates);
}