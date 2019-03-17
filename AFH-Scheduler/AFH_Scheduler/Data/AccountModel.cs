using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    public class AccountModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private int _administrator;

        public AccountModel(string username,string password,int administrator)
        {
            Username = username;

        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username == value) return;
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        public int Administrator
        {
            get { return _administrator; }
            set
            {
                if (_administrator == value) return;
                _administrator = value;
                OnPropertyChanged("Administrator");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
