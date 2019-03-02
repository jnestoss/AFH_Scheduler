using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;
using AFH_Scheduler.Database;

namespace AFH_Scheduler.Validations
{
    public class UniqueOutcomeCodeword : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            var test = (string)value;

            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var providers = db.Inspection_Outcome.Where(r => r.IOutcome_Code.Equals(test)).ToList();
                if (providers.Count == 0)
                {
                    return ValidationResult.ValidResult;
                }
            }

            return new ValidationResult(false, "This Codeword is already assigned, please use a different one.");
        }

    }
}
