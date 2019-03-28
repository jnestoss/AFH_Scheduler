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
        private bool _userIsAdmin;
        public bool UserIsAdmin
        {
            get { return _userIsAdmin; }
            set
            {
                _userIsAdmin = value;
                if (_userIsAdmin)
                    NewAdministrator = 0;
                else
                    NewAdministrator = 1;

                OnPropertyChanged("UserIsAdmin");
            }
        }
        public AddAccountVM()
        {
            NewUsername = "";
            NewPassword = "";
            NewAdministrator = 1;
            UserIsAdmin = false;
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
