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
            long textEntry;
            if (!long.TryParse((value ?? "").ToString(),
                NumberStyles.None,
                CultureInfo.CurrentCulture,
                out textEntry)) return new ValidationResult(false, "Invalid ID Entry");
         
            if(textEntry == EditVM._homeIDSave)
            {
                return ValidationResult.ValidResult;
            }

            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = from p in db.Provider_Homes
                            select p.PHome_ID;

                //Console.WriteLine(provs);

                foreach (long prov in provs)
                {
                    Console.WriteLine(prov);

                    if(prov.Equals(textEntry))
                    {
                        return new ValidationResult(false, "This ID Exists, try a different one");
                    }
                }
            }


            return ValidationResult.ValidResult;
        }
    }
}
