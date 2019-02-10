using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Globalization;

namespace AFH_Scheduler.Validations
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

            if (check.IsMatch(test))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, Message);
        }
    }
}
