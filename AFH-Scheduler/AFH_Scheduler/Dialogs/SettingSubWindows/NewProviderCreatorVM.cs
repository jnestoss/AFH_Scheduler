using AFH_Scheduler.Data;
using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class NewProviderCreatorVM : ObservableObject, IPageViewModel
    {
        private ProvidersModel _newProviderAdded;
        public ProvidersModel NewProviderAdded
        {
            get { return _newProviderAdded; }
            set
            {
                _newProviderAdded = value;
                OnPropertyChanged("NewProviderAdded");
            }
        }

        public NewProviderCreatorVM()
        {
            NewProviderAdded = new ProvidersModel("", "");
        }

        public string Name
        {
            get
            {
                return "Create New Provider";
            }
        }

    }
}
