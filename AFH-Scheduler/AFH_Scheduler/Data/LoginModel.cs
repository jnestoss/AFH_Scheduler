using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Login;

namespace AFH_Scheduler.Data
{
    class LoginModel : INotifyPropertyChanged
    {

        private readonly LoginViewVM _loginViewVM;

        public LoginModel(LoginViewVM loginViewVM)
        {
            _loginViewVM = loginViewVM;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
