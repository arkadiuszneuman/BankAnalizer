using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Db;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Users.UsersConnections.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Users.UsersConnections.Commands.Handlers
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
            await using var context = contextFactory.GetContext();
            var requestedUser = await context.Users.SingleOrDefaultAsync(u => u.Username == command.RequestedUsername);

            await Validate(command, context, requestedUser);

            var connection = new UsersConnection()
            {
                UserRequestedToConnectId = requestedUser.Id,
                UserRequestingConnectionId = command.UserId
            };

            await context.AddAsync(connection);
            await context.SaveChangesAsync();
        }

        private static async Task Validate(RequestUserConnectionCommand command, IContext context, User requestedUser)
        {
            if (requestedUser == null)
                throw new UsernameDoesNotExistException(command.RequestedUsername);

            if (requestedUser.Id == command.UserId)
                throw new UsernameCannotBeCurrentUserException();

            if (await context.UsersConnections.AnyAsync(u =>
                u.UserRequestedToConnectId == requestedUser.Id &&
                u.UserRequestingConnectionId == command.UserId))
            {
                throw new UsersConnectionAlreadyExistsException();
            }
        }
    }
}
