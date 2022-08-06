using MoneyConverter.Interfaces;
using MoneyConverter.Types;

namespace MoneyConverter.Services;

internal static class MathService
{
    public static Money Add(Money a, Money b, ICurrency currency, IConverter converter) =>
        MathOperation(a, b, currency, converter, (d, d1) => d + d1);

    public static Money Sub(Money a, Money b, ICurrency currency, IConverter converter) =>
        MathOperation(a, b, currency, converter, (d, d1) => d - d1);

    private static Money MathOperation(Money a, Money b, ICurrency currency, IConverter converter, Func<double, double, double> mathFunc)
    {
        if (converter == null) throw new ArgumentNullException(nameof(converter));
        
        var first = converter.Convert(a, currency);
        var second = converter.Convert(b, currency);
        return new Money(currency, mathFunc(first.Value, second.Value));        
    }
}