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
    [ValueConversion(typeof(Inspection_Outcome), typeof(string))]
    class OutcomeCodeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Inspection_Outcome outcome = (Inspection_Outcome) value;
            return outcome.IOutcome_Code;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;

            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                List<Inspection_Outcome> outcomes = db.Inspection_Outcome.ToList();
                Inspection_Outcome outcome = outcomes.Where(x => x.IOutcome_Code == strValue).First();
                return outcome;
            }
        }
    }
}
