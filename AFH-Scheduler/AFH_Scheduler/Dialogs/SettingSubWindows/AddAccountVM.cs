using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Dialogs.Confirmation;
using AFH_Scheduler.Dialogs.Errors;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class AddAccountVM : ObservableObject, IPageViewModel
    {
        private string _newUsername;
        public string NewUsername
        {
            get { return _newUsername; }
            set
            {
                _newUsername = value;
                OnPropertyChanged("NewUsername");
            }
        }
        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }
        private int _newAdministrator;
        public int NewAdministrator
        {
            get { return _newAdministrator; }
            set
            {
                _newAdministrator = value;
                OnPropertyChanged("NewAdministrator");
            }
        }
        public AddAccountVM()
        {
            NewUsername = "";
            NewPassword = "";
            NewAdministrator = 0;
        }
        public AddAccountVM(string username, string password, int administrator)
        {
            NewUsername = username;
            NewPassword = password;
            NewAdministrator = administrator;
        }

        public string Name
        {
            get
            {
                return "New Account";
            }
        }
    }
}
