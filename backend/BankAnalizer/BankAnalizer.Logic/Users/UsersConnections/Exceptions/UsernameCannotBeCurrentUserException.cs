using BankAnalizer.Core;

namespace BankAnalizer.Logic.Users.UsersConnections.Exceptions
{
    public class UsernameCannotBeCurrentUserException : BankAnalizerException
    {
        public UsernameCannotBeCurrentUserException() : base("Username cannot be from current user")
        { 
        }
    }
}