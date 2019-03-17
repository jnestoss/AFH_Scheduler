using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class EditAccountVM : ObservableObject, IPageViewModel
    {
        private string _editUsername;
        public string EditUsername
        {
            get { return _editUsername; }
            set
            {
                _editUsername = value;
                OnPropertyChanged("EditUsername");
            }
        }
        private string _editPassword;
        public string EditPassword
        {
            get { return _editPassword; }
            set
            {
                _editPassword = value;
                OnPropertyChanged("EditPassword");
            }
        }
        private int _editAdministrator;
        public int EditAdministrator
        {
            get { return _editAdministrator; }
            set
            {
                _editAdministrator = value;
                OnPropertyChanged("EditAdministrator");
            }
        }
        public EditAccountVM(string username,string password,int administrator)
        {
            EditUsername = username;
            EditPassword = password;
            EditAdministrator = administrator;
        }

        public string Name
        {
            get
            {
                return "Edit Account";
            }
        }
    }
}
