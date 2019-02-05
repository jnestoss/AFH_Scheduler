using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Database.LoginDB
{
    public class User
    {
        private string _username;
        private string _password;
        private bool _admin;

        public User(String username, String password, bool admin)
        {
            _username = username;
            _password = password;
            _admin = admin;
        }

        public String Username
        {
            get { return _username; }
            set
            {
                if (value != _username)
                {
                    _username = value;
                }
            }
        }
        public String Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                }
            }
        }
        public bool Admin
        {
            get { return _admin; }
            set
            {
                if(value != _admin)
                {
                    _admin = value;
                }
            }
        }
    }
}
