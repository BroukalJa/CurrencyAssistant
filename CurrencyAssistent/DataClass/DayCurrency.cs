using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyAssistent.DataClass
{
    public class DayCurrency
    {
        public DayCurrency() { }

        public decimal SellRate { get; set; }

        public decimal? BuyRate { get; set; }

        public int Amount { get; set; }
    }
}
