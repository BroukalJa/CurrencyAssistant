using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAssistent.DataClass
{
    public class Currency : INotifyPropertyChanged
    {
        public Currency()
        {
            BankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.CNB, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            BankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.CSOB, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            BankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.KB, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            BankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.RB, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            BankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.SPORITELNA, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            VisibleBankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.CNB, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            VisibleBankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.CSOB, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            VisibleBankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.KB, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            VisibleBankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.RB, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            VisibleBankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(BankEnumerator.SPORITELNA, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
        }

        public string Name { get; set; }

        public ObservableCollection<KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>> BankRates { get; set; } = new ObservableCollection<KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>>();

        public ObservableCollection<KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>> VisibleBankRates { get; set; } = new ObservableCollection<KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddRate(BankEnumerator bank, decimal sellRate, decimal? buyRate, int amount, DateTime date)
        {
            if (!BankRates.Any(x => x.Key == bank))
                BankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(bank, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
            if (!BankRates.First(x => x.Key == bank).Value.Any(x => x.Key == date))
                BankRates.First(x => x.Key == bank).Value.Add(new KeyValuePair<DateTime, DayCurrency>(date, new DayCurrency() { BuyRate = buyRate, Amount = amount, SellRate = sellRate }));
        }

        public void FilterVisible()
        {
            foreach(var bank in BankRates)
            {
                if (!VisibleBankRates.Any(x => x.Key == bank.Key))
                    VisibleBankRates.Add(new KeyValuePair<BankEnumerator, ObservableCollection<KeyValuePair<DateTime, DayCurrency>>>(bank.Key, new ObservableCollection<KeyValuePair<DateTime, DayCurrency>>()));
                else
                    VisibleBankRates.First(x => x.Key == bank.Key).Value.Clear();
                foreach (var rate in bank.Value)
                {
                    switch (CurrencySingleton.Instance.ActiveFilter)
                    {
                        case 0:
                            VisibleBankRates.First(x => x.Key == bank.Key).Value.Add(rate);
                            break;
                        case 1:
                            if(rate.Key > DateTime.Today.AddDays(-30))
                                VisibleBankRates.First(x => x.Key == bank.Key).Value.Add(rate);
                            break;
                        case 2:
                            if (rate.Key > DateTime.Today.AddDays(-7))
                                VisibleBankRates.First(x => x.Key == bank.Key).Value.Add(rate);
                            break;
                    }
                }
            }
        }

        //public void AddBuyRate(BankEnumerator bank, decimal rate, DateTime date)
        //{
        //    if (!BankRates.Any(x => x.Key == bank))
        //        BankRates.Add(bank, new Dictionary<DateTime, DayCurrency>());
        //    BankRates[bank].Add(date, rate);

        //}

        public CurrentCurrency CNBCurrency
        {
            get
            {
                if (BankRates.Any(x => x.Key == BankEnumerator.CNB) && BankRates.First(x => x.Key == BankEnumerator.CNB).Value.Count > 0)
                    return new CurrentCurrency() { Rate = BankRates.First(x => x.Key == BankEnumerator.CNB).Value.Last().Value.SellRate, Bank = BankEnumerator.CNB };
                else
                    return null;
            }
        }

        public CurrentCurrency HighestCurrency
        {
            get
            {
                CurrentCurrency cur = null;
                if (BankRates.Any(x => x.Key != BankEnumerator.CNB && x.Value.Any(y => y.Key == DateTime.Today)))
                {
                    var today = BankRates.Where(x => x.Key != BankEnumerator.CNB && x.Value.Any(y => y.Key == DateTime.Today)).ToList();
                    foreach (var rate in today)
                    {
                        if (cur == null)
                            cur = new CurrentCurrency() { Rate = rate.Value.First(x => x.Key == DateTime.Today).Value.BuyRate.Value, Bank = rate.Key };
                        else if (cur.Rate < rate.Value.First(x => x.Key == DateTime.Today).Value.BuyRate.Value)
                        {
                            cur.Rate = rate.Value.First(x => x.Key == DateTime.Today).Value.BuyRate.Value;
                            cur.Bank = rate.Key;
                        }
                    }
                }
                return cur;
            }
        }

        public CurrentCurrency LowestCurrency
        {
            get
            {
                CurrentCurrency cur = null;
                if (BankRates.Any(x => x.Key != BankEnumerator.CNB && x.Value.Any(y => y.Key == DateTime.Today)))
                {
                    var today = BankRates.Where(x => x.Key != BankEnumerator.CNB && x.Value.Any(y => y.Key == DateTime.Today)).ToList();
                    foreach (var rate in today)
                    {
                        if (cur == null)
                            cur = new CurrentCurrency() { Rate = rate.Value.First(x => x.Key == DateTime.Today).Value.SellRate, Bank = rate.Key };
                        else if (cur.Rate > rate.Value.First(x => x.Key == DateTime.Today).Value.SellRate)
                        {
                            cur.Rate = rate.Value.First(x => x.Key == DateTime.Today).Value.SellRate;
                            cur.Bank = rate.Key;
                        }
                    }
                }
                return cur;
            }
        }

        public string GetSerializedString()
        {
            var sb = new StringBuilder();
            foreach (var bank in BankRates)
            {
                foreach (var rate in bank.Value)
                {
                    sb.Append(Name).Append(";").Append((bank.Key.ToString())).Append(";").Append((rate.Value.SellRate)).Append(";").Append(rate.Value.BuyRate).Append(";").Append(rate.Value.Amount).Append(";").Append(rate.Key.ToString()).Append("\n");
                }
            }
            return sb.ToString();
        }

        private bool visible = false;

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Visible"));
            }
        }

        public static string GetBankName(BankEnumerator bank)
        {
            switch (bank)
            {
                case BankEnumerator.CNB:
                    return "Česká národní banka";
                case BankEnumerator.CSOB:
                    return "ČSOB";
                case BankEnumerator.KB:
                    return "Komerční banka";
                case BankEnumerator.RB:
                    return "Raiffeisen BANK";
                case BankEnumerator.SPORITELNA:
                    return "Česká spořitelna";
                default:
                    return String.Empty;
            }
        }
    }
}
