using MoneyConverter.Interfaces;
using MoneyConverter.Types;

namespace MoneyConverter.Services;

internal class Math: IMath
{
    private readonly IConverter _converter;

    public Math(IConverter converter)
    {
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
    }

    public Money Add(Money a, Money b, ICurrency destinationCurrency) => MathService.Add(a, b, destinationCurrency, _converter);
    public Money Sub(Money a, Money b, ICurrency destinationCurrency) => MathService.Sub(a, b, destinationCurrency, _converter);
}