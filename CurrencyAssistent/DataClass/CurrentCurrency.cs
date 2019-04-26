using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAssistent.DataClass
{
    public class CurrentCurrency
    {
        public CurrentCurrency() { }

        public BankEnumerator Bank { get; set; }

        public string Name
        {
            get
            {
                return Currency.GetBankName(Bank);
            }
        }

        public decimal Rate { get; set; }
    }
}
