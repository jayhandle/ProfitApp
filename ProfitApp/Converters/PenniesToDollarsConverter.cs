using ProfitLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProfitApp.Converters
{
    public class PenniesToDollarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var dollars = "$0";
            if (value != null)
            {
                return PaymentDetail.ConvertPenniesToDollars((long)value); 
            }
            return dollars;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return PaymentDetail.ConvertDollarstoPennies(value.ToString()); 
        }
    }
}
