using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CurrencyAssistent.Converters
{
    public class BankEnumToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.TryParse(value.ToString(), out DataClass.BankEnumerator bank))
            {
                switch (bank)
                {
                    case DataClass.BankEnumerator.CNB:
                        return Brushes.Black;
                    case DataClass.BankEnumerator.CSOB:
                        return Brushes.Blue;
                    case DataClass.BankEnumerator.KB:
                        return Brushes.Red;
                    case DataClass.BankEnumerator.SPORITELNA:
                        return Brushes.Green;
                    case DataClass.BankEnumerator.RB:
                        return Brushes.Orange;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
