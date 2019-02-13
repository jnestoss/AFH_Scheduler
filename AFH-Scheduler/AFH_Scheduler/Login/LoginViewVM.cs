using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Data;
using System.Windows.Input;
using AFH_Scheduler.Database.LoginDB;

namespace AFH_Scheduler.Login
{
    class LoginViewVM : ObservableObject, IPageViewModel
    {
        private MainVM _main;

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
            _main.Usr = UserFactory.CheckPassword("","");
            _main.CurrentPageViewModel = _main.PageViewModels[1];   
        }
    }
}
