using System;
using System.Collections.Generic;
using System.Text;

namespace BankAnalizer.Logic.Users.Models.User
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
