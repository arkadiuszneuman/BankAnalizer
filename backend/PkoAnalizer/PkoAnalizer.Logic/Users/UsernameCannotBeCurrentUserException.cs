using PkoAnalizer.Core;
using System;
using System.Runtime.Serialization;

namespace PkoAnalizer.Logic.Users
{
    public class UsernameCannotBeCurrentUserException : BankAnalizerException
    {
        public UsernameCannotBeCurrentUserException() : base("Username cannot be from current user")
        { 
        }
    }
}