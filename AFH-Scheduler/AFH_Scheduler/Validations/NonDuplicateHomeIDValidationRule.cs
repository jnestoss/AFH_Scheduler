using System;
using System.Globalization;
using System.Windows.Controls;
using AFH_Scheduler.Database;
using System.Linq;

namespace AFH_Scheduler.Validations
{
    public class NonDuplicateHomeIDValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Console.WriteLine("%%%%%%%%%%%%" + value + "%%%%%%%%%%%%%");

            String formattedText = (String) value;
            if (formattedText.EndsWith("_")) formattedText = formattedText.Substring(0, 4);

            int textEntry;
            if (!Int32.TryParse(formattedText,
                NumberStyles.Integer,
                CultureInfo.CurrentCulture,
                out textEntry)) return new ValidationResult(false, "Invalid ID Entry");
           
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = from p in db.Provider_Homes
                            select p.PHome_ID;

                //Console.WriteLine(provs);

                foreach (int prov in provs)
                {
                    Console.WriteLine(prov);

                    if(prov == textEntry)
                    {
                        return new ValidationResult(false, "This ID Exists, try a different one");
                    }
                }
            }


            return ValidationResult.ValidResult;
        }
    }
}
