using System;
using System.Globalization;
using System.Windows.Controls;
using AFH_Scheduler.Database;
using System.Linq;
using AFH_Scheduler.Dialogs;

namespace AFH_Scheduler.Validations
{
    public class NonDuplicateHomeIDValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (!long.TryParse((value ?? "").ToString(),
                NumberStyles.None,
                CultureInfo.CurrentCulture,
                out long textEntry)) return new ValidationResult(false, "Invalid ID Entry");
         
            //if(textEntry == EditVM._homeIDSave)
            //{
            //    return ValidationResult.ValidResult;
            //}

            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var providers = db.Providers.Where(r => r.Provider_ID == textEntry).ToList();

                if (providers.Count != 0)
                {
                    return new ValidationResult(false, "This ID Exists, try a different one");
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
