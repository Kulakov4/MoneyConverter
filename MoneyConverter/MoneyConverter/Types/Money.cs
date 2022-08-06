using MoneyConverter.Interfaces;

namespace MoneyConverter.Types;

public readonly struct Money
{
    public ICurrency Currency { get; }
    public double Value { get; }
    public Money(ICurrency currency, double value)
    { 
        Currency = currency;
        Value = value;
    }
    public override string ToString() => $"{Value} {Currency.Name}";
}