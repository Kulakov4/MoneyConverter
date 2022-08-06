using MoneyConverter.Interfaces;

namespace MoneyConverter.Model;

public class Currency: ICurrency
{
    public string Name { get; }
    public string Symbol { get; }
    public Currency(string name, string symbol)
    {
        Name = name;
        Symbol = symbol;
    }

    public bool Equals(ICurrency? other) => other != null && Name == other.Name;
        
    public override string ToString() => $"{Name} {Symbol}";
}