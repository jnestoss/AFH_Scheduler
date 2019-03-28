using AFH_Scheduler.Data;
using AFH_Scheduler.Database.LoginDB;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.HelperClasses
{
    public class LoginDBhelper
    {

        /*public void Create_User(string username, string password,int administrator)
        {
            UserLoginEntities userLogin = new UserLoginEntities();
            CryptSharp.BlowfishCrypter crypt = new CryptSharp.BlowfishCrypter();
            Database.LoginDB.Login newLogin = new Database.LoginDB.Login(username, password, crypt.GenerateSalt(),administrator);
            userLogin.Logins.Add(newLogin);
            userLogin.SaveChanges();
        }*/
        public List<AccountModel> LoadAccounts()
        {
            UserLoginEntities userLogin = new UserLoginEntities();
            List<AccountModel> accounts = new List<AccountModel>();
            AccountModel accountModel;
            foreach(Database.LoginDB.Login login in userLogin.Logins.ToList())
            {
                accountModel = new AccountModel(login.Username,login.Password,Int32.Parse(login.Administrator.Value.ToString()));
                accounts.Add(accountModel);
            }
            return accounts;
        }
    }
}
