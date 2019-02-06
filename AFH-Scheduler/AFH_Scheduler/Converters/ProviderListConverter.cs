using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AFH_Scheduler.Converters
{
    [ValueConversion(typeof(Tuple<int, string>), typeof(string))]
    class ProviderListConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Tuple<int, string>> providerInfo = (List<Tuple<int, string>>)value;

            List<string> providerOptions = new List<string>();

            foreach(var prov in providerInfo)
            {
                providerOptions.Add(prov.Item1.ToString() + " - " + prov.Item2);
            }

            return providerOptions;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> strValue = value as List<string>;

            List<Tuple<int, string>> providerInfo = new List<Tuple<int, string>>();

            foreach(var prov in strValue)
            {
                providerInfo.Add(
                    new Tuple<int, string>(
                        Int32.Parse(prov.Substring(0, 6)),
                        prov.Substring(9)
                ));
            }

            return providerInfo;
        }
    }
}