using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Db;
using BankAnalizer.Logic.Users.UsersConnections.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Users.UsersConnections.Commands.Handlers
{
    public class AcceptConnectionCommandHandler : ICommandHandler<AcceptConnectionCommand>
    {
        private readonly IContextFactory contextFactory;

        public AcceptConnectionCommandHandler(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task Handle(AcceptConnectionCommand command)
        {
            using var context = contextFactory.GetContext();

            var connectionToAccept = await context.UsersConnections.SingleOrDefaultAsync(c =>
                c.UserRequestedToConnectId == command.UserId &&
                c.UserRequestingConnectionId == command.HostUserIdToAcceptConnection);

            if (connectionToAccept == null)
                throw new ConnectionDoesNotExistsException(command.HostUserIdToAcceptConnection, command.UserId);

            connectionToAccept.IsRequestApproved = true;

            await context.SaveChangesAsync();
        }
    }
}
