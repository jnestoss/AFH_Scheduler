using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Data;
using System.Windows;
using System.Windows.Input;
using AFH_Scheduler.Database.LoginDB;

namespace AFH_Scheduler.Login
{
    class LoginViewVM : ObservableObject, IPageViewModel
    {
        private MainVM _main;
        private Visibility _invalidLogin;

        public string Name
        {
            get
            {
                return "Login View";
            }
        }
        public LoginViewVM(MainVM main)
        {
            _main = main;
            _username = "";
            _password = "";
            _invalidLogin = Visibility.Hidden;
            //_login = new LoginModel();
        }
        private LoginModel _login;
        private LoginModel Login
        {
            get { return _login; }
            set
            {
                if (_login == value) return;
                _login = value;
            }
        }
        private RelayCommand _loggingIn;
        public ICommand LoginngInCommand
        {
            get
            {
                if (_loggingIn == null)
                    _loggingIn = new RelayCommand(LoginIn);
                return _loggingIn;
            }
        }
        private void LoginIn(object obj)//passes in username,password
        {
            //_main.LoggedIn(UserFactory.CheckPassword(Username, Password));
            User user = UserFactory.CheckPassword(Username, Password);
            if (user != null)
            {
                InvalidLogin = Visibility.Hidden;
                _main.LoggedIn(user);
            }
            else
            {
                InvalidLogin = Visibility.Visible;
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        public Visibility InvalidLogin
        {
            get
            {
                return _invalidLogin;
            }
            set
            {
                if (_invalidLogin != value)
                {
                    _invalidLogin = value;
                    OnPropertyChanged("InvalidLogin");
                }
            }
        }
    }
}
