using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAssistent.DataClass
{
    public class Currency : INotifyPropertyChanged
    {
        public Currency() { }

        public string Name { get; set; }

        public Dictionary<BankEnumerator, Dictionary<DateTime, DayCurrency>> BankRates { get; set; } = new Dictionary<BankEnumerator, Dictionary<DateTime, DayCurrency>>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddRate(BankEnumerator bank, decimal sellRate, decimal? buyRate, int amount, DateTime date)
        {
            if (!BankRates.Any(x => x.Key == bank))
                BankRates.Add(bank, new Dictionary<DateTime, DayCurrency>());
            if(!BankRates[bank].ContainsKey(date))
                BankRates[bank].Add(date, new DayCurrency() { BuyRate = buyRate, Amount = amount, SellRate = sellRate });
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
                if (BankRates.Keys.Contains(BankEnumerator.CNB) && BankRates[BankEnumerator.CNB].Keys.Contains(DateTime.Today))
                    return new CurrentCurrency() { Rate = BankRates[BankEnumerator.CNB][DateTime.Today].SellRate, Bank = BankEnumerator.CNB };
                else
                    return null;
            }
        }

        public CurrentCurrency HighestCurrency
        {
            get
            {
                CurrentCurrency cur = null;
                if (BankRates.Any(x => x.Key != BankEnumerator.CNB && x.Value.Keys.Contains(DateTime.Today)))
                {
                    var today = BankRates.Where(x => x.Key != BankEnumerator.CNB && x.Value.Keys.Contains(DateTime.Today)).ToList();
                    foreach (var rate in today)
                    {
                        if (cur == null)
                            cur = new CurrentCurrency() { Rate = rate.Value[DateTime.Today].BuyRate.Value, Bank = rate.Key };
                        else if (cur.Rate < rate.Value[DateTime.Today].BuyRate.Value)
                        {
                            cur.Rate = rate.Value[DateTime.Today].BuyRate.Value;
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
                if (BankRates.Any(x => x.Key != BankEnumerator.CNB && x.Value.Keys.Contains(DateTime.Today)))
                {
                    var today = BankRates.Where(x => x.Key != BankEnumerator.CNB && x.Value.Keys.Contains(DateTime.Today)).ToList();
                    foreach (var rate in today)
                    {
                        if (cur == null)
                            cur = new CurrentCurrency() { Rate = rate.Value[DateTime.Today].SellRate, Bank = rate.Key };
                        else if (cur.Rate > rate.Value[DateTime.Today].SellRate)
                        {
                            cur.Rate = rate.Value[DateTime.Today].SellRate;
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
