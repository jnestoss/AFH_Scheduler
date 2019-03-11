using System;
using System.Globalization;
using System.Windows.Data;
using AFH_Scheduler.Database;
using System.Collections.Generic;

namespace AFH_Scheduler.Converters
{
    [ValueConversion(typeof(Provider), typeof(string))]
    public class StringToProviderConverter : IValueConverter
    {
        private ICollection<Provider_Homes> _homes;
        private long _providerID;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Provider prov = (Provider)value;
            _homes = prov.Provider_Homes;
            _providerID = prov.Provider_ID;
            return prov.Provider_Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string updatedProvider = (string)value;
            return new Provider {
                Provider_Homes = _homes,
                Provider_ID = _providerID,
                Provider_Name = updatedProvider
            };
        }
    }
}