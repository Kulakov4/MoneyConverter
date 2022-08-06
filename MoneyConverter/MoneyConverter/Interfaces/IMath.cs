using MoneyConverter.Types;

namespace MoneyConverter.Interfaces;

public interface IMath
{
    Money Add(Money a, Money b, ICurrency destinationCurrency);
    Money Sub(Money a, Money b, ICurrency destinationCurrency);    
}