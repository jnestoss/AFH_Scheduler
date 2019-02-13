using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class EditProviderVM : ObservableObject, IPageViewModel
    {
        private string _editableProviderName;
        public string EditableProviderName
        {
            get { return _editableProviderName; }
            set
            {
                _editableProviderName = value;
                OnPropertyChanged("EditableProviderName");
            }
        }

        public EditProviderVM(string name)
        {
            EditableProviderName = name;
        }

        public string Name
        {
            get
            {
                return "Edit Provider Name";
            }
        }
    }
}
