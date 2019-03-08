using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AFH_Scheduler.Database;
namespace AFH_Scheduler.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string storedDate = (string)value;
            DateTime time = DateTime.Parse(storedDate);
            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateToConvert = (DateTime)value;

            return $"{dateToConvert.Month}/{dateToConvert.Day}/{dateToConvert.Year}";
        }
    }
}
