using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BankAnalizer.Core.Cqrs.Command
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

        public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            return SendAsync(typeof(TCommand), command);
        }

        public async Task SendAsync(Type commandType, ICommand command)
        {
            try
            {
                var handler = _handlersFactory(commandType);

                if (handler == null)
                {
                    logger.LogWarning("No command handler for command {command}", commandType.FullName);
                    return;
                }

                var handlerType = handler.GetType();
                var handleMethod = handlerType.GetMethod("Handle");
                Task task = (Task)handleMethod.Invoke(handler, new[] { command });
                await task;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);
                throw;
            }
        }
    }
}
