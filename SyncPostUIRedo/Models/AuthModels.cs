using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncPostUI.Models
{
    public class TokenModel
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get;  set; }
    }
    public class RegistrationModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public RegistrationModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
    public class ChangePWModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public ChangePWModel(string oldpw, string newpw)
        {
            OldPassword = oldpw;
            NewPassword = newpw;
        }
    }


}
