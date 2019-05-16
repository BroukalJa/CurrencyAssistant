using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAssistent
{
    public sealed class CurrencySingleton : INotifyPropertyChanged
    {
        private static readonly CurrencySingleton instance = new CurrencySingleton();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DataClass.Currency> Currencies { get; set; } = new ObservableCollection<DataClass.Currency>();

        public void AddCurrencyRates(string currency, DataClass.BankEnumerator bank, decimal sellRate, decimal? buyRate, int amount, DateTime date)
        {
            if (!Currencies.Any(x => x.Name == currency))
                Currencies.Add(new DataClass.Currency() { Name = currency });
            var cur = Currencies.First(x => x.Name == currency);
            cur.AddRate(bank, sellRate, buyRate, amount, date);
        }

        private bool downloadRunning = false;

        public bool DownloadRunning
        {
            get
            {
                return downloadRunning;
            }
            set
            {
                downloadRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DownloadRunning"));
            }
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
