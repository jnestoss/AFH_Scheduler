using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.Rescheduling
{
    public class ReschedulingVM : ObservableObject, IPageViewModel
    {
        private DateTime _datePicked;
        public DateTime DatePicked
        {
            get { return _datePicked; }
            set
            {
                _datePicked = value;
                OnPropertyChanged("DatePicked");
            }
        }

        public ReschedulingVM(string date)
        {
            DatePicked = DateTime.Parse(date);
        }
        public string Name
        {
            get
            {
                return "Rescheduling";
            }
        }
    }
}
