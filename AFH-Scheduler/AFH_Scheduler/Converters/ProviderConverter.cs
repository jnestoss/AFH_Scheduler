using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AFH_Scheduler.Converters
{
    [ValueConversion(typeof(Tuple<int,string>),typeof(string))]
    class ProviderConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tuple<int, string> providerInfo = (Tuple<int, string>)value;
            return providerInfo.Item1.ToString() + " - " + providerInfo.Item2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            Tuple<int, string> resultProvider = new Tuple<int, string>(
                Int32.Parse(strValue.Substring(0, 6)),
                strValue.Substring(9)
            );
            return resultProvider;
        }
    }
}
