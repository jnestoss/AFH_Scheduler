using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.HelperClasses
{
    public class UniqueDateImportItem
    {
        private string pocName;
        private DateTime date;

        public UniqueDateImportItem(string name, DateTime dateTime)
        {
            PocName = name;
            NextInspection = dateTime;
        }

        public string PocName
        {
            get { return pocName; }
            set
            {
                if (pocName == value) return;
                pocName = value;
            }
        }
        public DateTime NextInspection
        {
            get { return date; }
            set
            {
                if (date == value) return;
                date = value;
            }
        }
    }
}
