namespace MoneyConverter.Interfaces;

public interface IExchangeRate
{
    ICurrency SourceCurrency { get; }
    ICurrency DestinationCurrency { get; }
    double Rate { get; }
}