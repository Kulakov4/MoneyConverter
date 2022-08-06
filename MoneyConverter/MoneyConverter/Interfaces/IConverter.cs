using MoneyConverter.Types;

namespace MoneyConverter.Interfaces;

public interface IConverter
{
    Money Convert(Money money, ICurrency destinationCurrency);
}