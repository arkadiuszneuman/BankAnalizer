using BankAnalizer.Core;

namespace BankAnalizer.Logic.Users.UsersConnections.Exceptions
{
    public class UsersConnectionAlreadyExistsException : BankAnalizerException
    {
        public UsersConnectionAlreadyExistsException() : base("Users connection already exists")
        {

        }
    }
}
