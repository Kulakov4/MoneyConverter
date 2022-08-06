namespace MoneyConverter.Interfaces;

public interface ICurrency: IEquatable<ICurrency>
{
    string Name { get; }
}