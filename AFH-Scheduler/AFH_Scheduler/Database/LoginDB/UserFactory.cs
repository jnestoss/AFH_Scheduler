using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptSharp;

namespace AFH_Scheduler.Database.LoginDB
{
    public static class UserFactory
    {
        public static User CheckPassword(string username, string password)
        {
            // return new User("admin", "password",true);
            string salt = GetSalt(username);
            if (salt != "Not Found")
            {
                UserLoginEntities userLogin = new UserLoginEntities();
                string hashedpass = CryptSharp.Sha512Crypter.Blowfish.Crypt(password, salt);
                LoginDB.Login checkUser = userLogin.Logins.First(x => x.Username == username);
                if (checkUser.Password == hashedpass)
                { 
                    User user = new User(checkUser.Username,checkUser.Password,false);
                    return user;
                }
                return null;
            }
            else
            {
                return null;
            }

        }

        private static string GetSalt(string username)
        {
            UserLoginEntities userLogin = new UserLoginEntities();
            string salt = userLogin.Logins.First(x => x.Username == username).Salt;
            if(salt == null || salt == "")
            {
                return "Not Found";
            }
            return salt; ;
        }

        public static User[] LoadUsers()
        {
            return null;
        }
    }
}
