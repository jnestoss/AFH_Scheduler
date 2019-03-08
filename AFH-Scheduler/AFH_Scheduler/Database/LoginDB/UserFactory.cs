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
                string hashedpass = CryptSharp.Sha512Crypter.Blowfish.Crypt(password, salt);
                string check = "Select Username,Password,Administrator From Login Where Username = @username AND Password = @password";
                SQLiteConnection connect = GetConnection();
                connect.Open();
                SQLiteCommand command = new SQLiteCommand(check, connect);
                SQLiteParameter temp = new SQLiteParameter("@username");
                temp.Value = username;
                command.Parameters.Add(temp);
                temp = new SQLiteParameter("@password");
                temp.Value = hashedpass;
                command.Parameters.Add(temp);
                SQLiteDataReader reader;
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (SQLiteException)
                {
                    connect.Close();
                    return null;
                }
                if (reader.Read())
                { 
                    bool insert;
                    if(reader.GetInt16(2) == 0)
                    {
                        insert = false;
                    }
                    else
                    {
                        insert = true;
                    }
                    User user = new User(reader.GetString(0), reader.GetString(1), insert);
                    reader.Close();
                    return user;
                }
                connect.Close();
                return null;
            }
            else
            {
                return null;
            }

        }
        private static SQLiteConnection GetConnection()
        {
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"..\..\..\Database\LoginDB\";
            string filter = "UserLogin.db";
            string[] files = Directory.GetFiles(folder, filter);
            return new SQLiteConnection(("Data Source=" + files[0] + ";Version=3;FailIfMissing=True"));
        }
        public static User DebugCreate_User(string username,string password)
        {
            string insertUser = "Insert INTO Login (Username,Password,Salt,Administrator) Values (@username,@password,@salt,0);";
            SQLiteConnection connect = GetConnection();
            connect.Open();
            string salt = CryptSharp.Sha512Crypter.Blowfish.GenerateSalt();
            string hashedpass = CryptSharp.Sha512Crypter.Blowfish.Crypt(password, salt);
            SQLiteCommand command = new SQLiteCommand(insertUser, connect);
            SQLiteParameter temp = new SQLiteParameter("@username");
            temp.Value = username;
            command.Parameters.Add(temp);
            temp = new SQLiteParameter("@password");
            temp.Value = hashedpass;
            command.Parameters.Add(temp);
            temp = new SQLiteParameter("@salt");
            temp.Value = hashedpass;
            command.Parameters.Add(temp);
            try
            {
                command.ExecuteNonQuery();
                connect.Close();
                return new User(username,password,true);
            }
            catch (Exception e)
            {
                connect.Close();
                return null;
            }
        }
        private static string GetSalt(string username)
        {
            // return new User("admin", "password",true);
            string check = "Select Salt From Login Where Username = @username";
            SQLiteConnection connect = GetConnection();
            connect.Open();
            SQLiteCommand command = new SQLiteCommand(check, connect);
            SQLiteParameter temp = new SQLiteParameter("@username");
            temp.Value = username;
            command.Parameters.Add(temp);
            SQLiteDataReader reader;
            try
            {
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string ret = reader.GetString(0);
                    reader.Close();
                    return ret;
                }
                else
                {
                    connect.Close();
                    return "Not Found";
                }
            }
            catch (SQLiteException)
            {
                connect.Close();
                return "Not Found";
            }
        }
        public static bool CreateUser(string username, string password, bool admin)
        {
            return false;
        }

        public static User[] LoadUsers()
        {
            return null;
        }
    }
}
