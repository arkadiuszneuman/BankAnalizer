using PkoAnalizer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Users.UsersConnections.Exceptions
{
    public class UsernameDoesNotExistException : BankAnalizerException
    {
        public UsernameDoesNotExistException(string username) : base($"Username {username} doesn't exist")
        {

        }
    }
}
