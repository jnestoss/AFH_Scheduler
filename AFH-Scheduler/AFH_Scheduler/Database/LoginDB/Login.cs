//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AFH_Scheduler.Database.LoginDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Login
    {
        public Login(string username, string password,string salt,int administrator)
        {
            Username = username;
            Password = password;
            Salt = salt;
            Administrator = 0;
        }
        public Login()
        {
            Username = "";
            Password = "";
            Salt = "";
            Administrator = 0;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public Nullable<long> Administrator { get; set; }
    }
}
