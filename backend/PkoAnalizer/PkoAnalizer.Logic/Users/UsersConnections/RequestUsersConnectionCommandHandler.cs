using Microsoft.EntityFrameworkCore;
using PkoAnalizer.Core.Commands.Users;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Db;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Users.UsersConnections.Exceptions;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Users.UsersConnections
{
    public class RequestUsersConnectionCommandHandler : ICommandHandler<RequestUserConnectionCommand>
    {
        private readonly IContextFactory contextFactory;

        public RequestUsersConnectionCommandHandler(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task Handle(RequestUserConnectionCommand command)
        {
            using var context = contextFactory.GetContext();
            var requestedUser = await context.Users.SingleOrDefaultAsync(u => u.Username == command.RequestedConnectionToUsername);

            await Validate(command, context, requestedUser);

            var connection = new UsersConnection()
            {
                UserRequestedToConnectId = requestedUser.Id,
                UserRequestingConnectionId = command.RequestingUserId
            };

            await context.AddAsync(connection);
            await context.SaveChangesAsync();
        }

        private static async Task Validate(RequestUserConnectionCommand command, IContext context, User requestedUser)
        {
            if (requestedUser == null)
                throw new UsernameDoesNotExistException(command.RequestedConnectionToUsername);

            if (requestedUser.Id == command.RequestingUserId)
                throw new UsernameCannotBeCurrentUserException();

            if (await context.UsersConnections.AnyAsync(u =>
                u.UserRequestedToConnectId == requestedUser.Id &&
                u.UserRequestingConnectionId == command.RequestingUserId))
            {
                throw new UsersConnectionAlreadyExistsException();
            }
        }
    }
}
