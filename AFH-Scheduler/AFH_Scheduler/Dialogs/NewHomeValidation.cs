using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AFH_Scheduler.Dialogs
{
    public class NewHomeValidation : ValidationRule
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message == value) return;
                _message = value;
            }
        }

        private string _regex;        
        public string Regex
        {
            get { return _regex; }
            set
            {
                if (_regex == value) return;
                _regex = value;
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex check = new Regex(Regex);

            var test = (string)value;
            if(test.Length == 0)
            {
                return new ValidationResult(false, "This field is required.");
            }
            if (check.IsMatch(test))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, Message);
        }
    }
}
