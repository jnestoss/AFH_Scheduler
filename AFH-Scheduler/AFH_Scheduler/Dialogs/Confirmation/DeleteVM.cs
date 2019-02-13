using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.Confirmation
{
    public class DeleteVM : ObservableObject, IPageViewModel
    {
        private string _name;
        public string SelectedName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("SelectedName");
            }
        }

        public DeleteVM(string nameAssigned)
        {
            SelectedName = nameAssigned;
        }

        public string Name
        {
            get
            {
                return "Delete Selected Provider";
            }
        }
    }
}
