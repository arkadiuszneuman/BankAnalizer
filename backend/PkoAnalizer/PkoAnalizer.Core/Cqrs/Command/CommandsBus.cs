using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PkoAnalizer.Core.Cqrs.Command
{
    public class CommandsBus : ICommandsBus
    {
        private readonly Func<Type, ICommandHandler> _handlersFactory;
        private readonly ILogger<CommandsBus> logger;

        public CommandsBus(Func<Type, ICommandHandler> handlersFactory,
            ILogger<CommandsBus> logger)
        {
            _handlersFactory = handlersFactory;
            this.logger = logger;
        }

        public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = (ICommandHandler<TCommand>)_handlersFactory(typeof(TCommand));

            if (handler == null)
            {
                logger.LogWarning("No command handler for command {command}", typeof(TCommand).FullName);
                return;
            }

            await handler.Handle(command);
        }
    }
}
