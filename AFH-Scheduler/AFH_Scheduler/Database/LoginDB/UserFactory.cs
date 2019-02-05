using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Database.LoginDB
{
    public static class UserFactory
    {
        private static readonly string connection = "metadata=res://*/Database.LoginDB.csdl|res://*/Database.LoginDB.ssdl|res://*/Database.LoginDB.msl;provider=System.Data.SQLite.EF6;provider connection string=&quot;data source=..\\..\\Database\\LoginDB\\LoginDB.db&quot;Password=Afh";

        public static bool CheckPassword(string username, string password)
        {
            return true;
        }

        public static bool CreateUser(string username,string password,bool admin)
        {
            return false;
        }

        public static User[] LoadUsers()
        {
            return null;
        }
    }
}
