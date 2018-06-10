using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
    public class ConvertedCurrency
    {
        public decimal USD_UAH { get; set; }
        public decimal USD_EUR { get; set; }
        public decimal USD_GBP { get; set; }
    }

    class Program
    {
        private const string URL = "https://free.currencyconverterapi.com/api/v5/convert?";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Chouse: UAH, EUR or GBP");
            string toCurrency = Console.ReadLine();

            var result = CurrencyConvert(amount, toCurrency);
            Console.WriteLine(result);
            Console.ReadLine();
        }
        
        public static decimal CurrencyConvert(decimal amount, string toCurrency)
        {
            var currency = String.Format("{0}_{1}", "USD", toCurrency);

            string apiURL = String.Format("{0}q={1}&compact=ultra", URL, currency);

            var request = WebRequest.Create(apiURL);

            var streamReader = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.Encoding.ASCII);

            ConvertedCurrency apiResult = JsonConvert.DeserializeObject<ConvertedCurrency>(streamReader.ReadToEnd());

            var countOfCurrencyPerOneUsd = apiResult.GetType().GetProperties()
                .Single(pi => pi.Name == currency)
                .GetValue(apiResult, null);

            return amount * Decimal.Parse(countOfCurrencyPerOneUsd.ToString());        
        }        
    }          
}
