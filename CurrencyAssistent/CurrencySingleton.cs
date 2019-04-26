using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAssistent
{
    public sealed class CurrencySingleton
    {
        private static readonly CurrencySingleton instance = new CurrencySingleton();

        public ObservableCollection<DataClass.Currency> Currencies { get; set; } = new ObservableCollection<DataClass.Currency>();

        public void AddCurrencyRates(string currency, DataClass.BankEnumerator bank, decimal sellRate, decimal? buyRate, int amount, DateTime date)
        {
            if (!Currencies.Any(x => x.Name == currency))
                Currencies.Add(new DataClass.Currency() { Name = currency });
            var cur = Currencies.First(x => x.Name == currency);
            cur.AddRate(bank, sellRate, buyRate, amount, date);
        }

        public string SerializeCurrencies()
        {
            var sb = new StringBuilder();
            foreach (var cur in Currencies)
            {
                sb.Append(cur.GetSerializedString());
            }
            return sb.ToString();
        }

        static CurrencySingleton()
        {
        }

        private CurrencySingleton()
        {
        }

        public static CurrencySingleton Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
