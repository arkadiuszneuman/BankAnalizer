using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Core.Cqrs.Command
{
    public class CommandsBus : ICommandsBus
    {
        private readonly Func<Type, ICommandHandler> _handlersFactory;
        public CommandsBus(Func<Type, ICommandHandler> handlersFactory)
        {
            _handlersFactory = handlersFactory;
        }

        public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = (ICommandHandler<TCommand>)_handlersFactory(typeof(TCommand));
            await handler.Handle(command);
        }
    }
}
