using BankAnalizer.Core;

namespace BankAnalizer.Logic.Users.UsersConnections.Exceptions
{
    public class UsernameDoesNotExistException : BankAnalizerException
    {
        public UsernameDoesNotExistException(string username) : base($"Username {username} doesn't exist")
        {

        }
    }
}
