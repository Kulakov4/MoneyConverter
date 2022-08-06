// See https://aka.ms/new-console-template for more information

using MoneyConverter.Interfaces;
using MoneyConverter.Model;
using MoneyConverter.Services;
using MoneyConverter.Types;
using Math = MoneyConverter.Services.Math;

var ruble = new Currency("Ruble", "Ⳁ");
var dollar = new Currency("Dollar", "$");
var euro = new Currency("Euro", "€");
var kzt = new Currency("KZT", "₸");
var jpy = new Currency("JPY", "¥");

var exchangeRates = new IExchangeRate[]
{
    new ExchangeRate(ruble, dollar, 0.0166),
    //new ExchangeRate(ruble, euro, 0.0164),
    new ExchangeRate(ruble, kzt, 7.9038),
    //new ExchangeRate(ruble, jpy, 2.2175),

    new ExchangeRate(dollar, ruble, 60.258), new ExchangeRate(dollar, euro, 0.9864),
    new ExchangeRate(dollar, kzt, 476.2699),
    //new ExchangeRate(dollar, jpy, 133.6202),

    new ExchangeRate(euro, ruble, 61.0872), new ExchangeRate(euro, dollar, 1.0138),
    new ExchangeRate(euro, kzt, 482.8236), new ExchangeRate(euro, jpy, 135.4588), new ExchangeRate(kzt, ruble, 0.1265),
    new ExchangeRate(kzt, dollar, 0.0021), new ExchangeRate(kzt, euro, 0.0021),
    //new ExchangeRate(kzt, jpy, 0.2806),                

    new ExchangeRate(jpy, ruble, 0.451), new ExchangeRate(jpy, dollar, 0.0075), new ExchangeRate(jpy, euro, 0.0074),
    new ExchangeRate(jpy, kzt, 3.5643),
};
var converter = new Converter(exchangeRates);
var math = new Math(converter);

var rubMoney = new Money(ruble, 100);
try
{
    var convertedMoney = converter.Convert(rubMoney, jpy);
    Console.WriteLine(
        $"{rubMoney.Value} {rubMoney.Currency.Name} = {convertedMoney.Value} {convertedMoney.Currency.Name}");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

try
{
    var dollarMoney = new Money(dollar, 1);
    var sum = math.Add(rubMoney, dollarMoney, rubMoney.Currency);
    var sub = math.Sub(rubMoney, dollarMoney, rubMoney.Currency);
    Console.WriteLine($"{rubMoney} + {dollarMoney} = {sum}");
    Console.WriteLine($"{rubMoney} - {dollarMoney} = {sub}");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}