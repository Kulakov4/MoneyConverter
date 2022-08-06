using MoneyConverter.Interfaces;
using MoneyConverter.Model;
using MoneyConverter.Types;
using MoneyConverter.Services;
using NUnit.Framework;

namespace MoneyConverterTests;

[TestFixture]
internal static class ConverterTests
{
    private static readonly Currency Ruble = new Currency("Ruble", "Ⳁ");
    private static readonly Currency Dollar = new Currency("Dollar", "$");
    private static readonly Currency Euro = new Currency("Euro", "€");
    private static readonly Currency Kzt = new Currency("KZT", "₸");
    private static readonly Currency Jpy = new Currency("JPY", "¥");    
    
    
    private static readonly IEnumerable<IExchangeRate> ExchangeRates = new []
    {
        new ExchangeRate(Ruble, Dollar, 0.0166),
        new ExchangeRate(Ruble, Euro, 0.0164),
        new ExchangeRate(Ruble, Kzt, 7.9038),
        new ExchangeRate(Ruble, Jpy, 2.2175),
                
        new ExchangeRate(Dollar, Ruble, 60.258),
        new ExchangeRate(Dollar, Euro, 0.9864),
        new ExchangeRate(Dollar, Kzt, 476.2699),
        new ExchangeRate(Dollar, Jpy, 133.6202),
                
        new ExchangeRate(Euro, Ruble, 61.0872),
        new ExchangeRate(Euro, Dollar, 1.0138),
        new ExchangeRate(Euro, Kzt, 482.8236),
        new ExchangeRate(Euro, Jpy, 135.4588),
                
        new ExchangeRate(Kzt, Ruble, 0.1265),
        new ExchangeRate(Kzt, Dollar, 0.0021),
        new ExchangeRate(Kzt, Euro, 0.0021),
        new ExchangeRate(Kzt, Jpy, 0.2806),                
                
        new ExchangeRate(Jpy, Ruble, 0.451),
        new ExchangeRate(Jpy, Dollar, 0.0075),
        new ExchangeRate(Jpy, Euro, 0.0074),
        new ExchangeRate(Jpy, Kzt, 3.5643),                
        
    };

    private static IEnumerable<IExchangeRate> Exclude(this IEnumerable<IExchangeRate> exchangeRates, ICurrency s, ICurrency d) =>
        exchangeRates.Where(er => !(er.SourceCurrency.Equals(s) && er.DestinationCurrency.Equals(d)));
    
    private static IExchangeRate First(this IEnumerable<IExchangeRate> exchangeRates, ICurrency s, ICurrency d) =>
        exchangeRates.First(er => er.SourceCurrency.Equals(s) && er.DestinationCurrency.Equals(d));

    [Test]
    public static void ConverterExceptionTest()
    {
        var rates = ExchangeRates.Exclude(Ruble, Euro).Exclude(Ruble, Jpy).Exclude(Dollar, Jpy).Exclude(Euro, Jpy).Exclude(Kzt, Jpy);
        
        var converter = new Converter(rates);
        var rubMoney = new Money(Ruble, 100);
        Assert.That(() => converter.Convert(rubMoney, Jpy), Throws.InstanceOf<Exception>());
    }
    
    [Test]
    public static void ConverterServiceTest1()
    {
        var rates = ExchangeRates.Exclude(Ruble, Euro).Exclude(Ruble, Jpy).Exclude(Dollar, Jpy);
        var rubMoney = new Money(Ruble, 100);
        var result = ConverterService.GetExchangeRates(rubMoney, Jpy, rates).ExchangeRates.ToArray();
        Assert.AreEqual(result.Count(), 3);
        Assert.AreEqual(result[0].SourceCurrency, Ruble);
        Assert.AreEqual(result[0].DestinationCurrency, Dollar);
        Assert.AreEqual(result[1].SourceCurrency, Dollar);
        Assert.AreEqual(result[1].DestinationCurrency, Kzt);
        Assert.AreEqual(result[2].SourceCurrency, Kzt);
        Assert.AreEqual(result[2].DestinationCurrency, Jpy);
    }
    
    [Test]    
    public static void ConverterServiceTest2()
    {
        var rates = ExchangeRates.Exclude(Ruble, Euro).Exclude(Ruble, Jpy).Exclude(Dollar, Kzt).Exclude(Dollar, Jpy);
        var rubMoney = new Money(Ruble, 100);
        var result = ConverterService.GetExchangeRates(rubMoney, Jpy, rates).ExchangeRates.ToArray();
        Assert.AreEqual(result.Count(), 3);
        Assert.AreEqual(result[0].SourceCurrency, Ruble);
        Assert.AreEqual(result[0].DestinationCurrency, Dollar);
        Assert.AreEqual(result[1].SourceCurrency, Dollar);
        Assert.AreEqual(result[1].DestinationCurrency, Euro);
        Assert.AreEqual(result[2].SourceCurrency, Euro);
        Assert.AreEqual(result[2].DestinationCurrency, Jpy);        
    }    

    [Test]    
    public static void ConverterTest()
    {
        var rates = ExchangeRates.Exclude(Ruble, Euro).Exclude(Ruble, Jpy).Exclude(Dollar, Jpy);
        var rubMoney = new Money(Ruble, 100);        
        var converter = new Converter(rates);        
        var convertedMoney = converter.Convert(rubMoney, Jpy);
        Assert.AreEqual(convertedMoney.Currency, Jpy);
        Assert.AreEqual(convertedMoney.Value,
            rubMoney.Value * ExchangeRates.First(Ruble, Dollar).Rate * ExchangeRates.First(Dollar, Kzt).Rate *
            ExchangeRates.First(Kzt, Jpy).Rate);
    }    
}