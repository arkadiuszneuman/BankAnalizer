using PkoAnalizer.Core;

namespace PkoAnalizer.Logic.Users.UsersConnections.Exceptions
{
    public class UsersConnectionAlreadyExistsException : BankAnalizerException
    {
        public UsersConnectionAlreadyExistsException() : base("Users connection already exists")
        {

        }
    }
}
