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
    public class UniqueProviderIDValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int id;
            if (!int.TryParse((value ?? "").ToString(),
                NumberStyles.None,
                CultureInfo.CurrentCulture,
                out id)) return new ValidationResult(false, "Invalid ID Entry");

            //var test = (string)value;
            //var id = Convert.ToInt64(test);

            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var providers = db.Providers.Where(r => r.Provider_ID == id).ToList();
                if (providers.Count == 0)
                {
                    return ValidationResult.ValidResult;
                }
            }

            return new ValidationResult(false, "This provider ID is already assigned, please use a different one.");
        }
    }
}
