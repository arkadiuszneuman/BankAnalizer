﻿using Microsoft.EntityFrameworkCore;
using PkoAnalizer.Core.Commands.Users;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Db;
using PkoAnalizer.Logic.Users.UsersConnections.Exceptions;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Users.UsersConnections
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
                c.UserRequestedToConnectId == command.CurrentUserId &&
                c.UserRequestingConnectionId == command.HostUserIdToAcceptConnection);

            if (connectionToAccept == null)
                throw new ConnectionDoesNotExistsException(command.HostUserIdToAcceptConnection, command.CurrentUserId);

            connectionToAccept.IsRequestApproved = true;

            await context.SaveChangesAsync();
        }
    }
}
