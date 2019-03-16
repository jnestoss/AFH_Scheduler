using System;
using System.Globalization;
using System.Windows.Data;
using AFH_Scheduler.Database;
using System.Collections.Generic;

namespace AFH_Scheduler.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    class DesiredAverageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double desiredAverage = (double)value;
            return String.Format("{0:0.00}", desiredAverage);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
