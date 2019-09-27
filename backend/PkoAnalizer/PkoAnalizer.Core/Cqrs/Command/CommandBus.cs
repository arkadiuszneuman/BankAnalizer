using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Core.Cqrs.Command
{
    public class CommandsBus : ICommandsBus
    {
        private readonly Func<Type, IHandleCommand> _handlersFactory;
        public CommandsBus(Func<Type, IHandleCommand> handlersFactory)

        {
            _handlersFactory = handlersFactory;
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = (IHandleCommand<TCommand>)_handlersFactory(typeof(TCommand));
            handler.Handle(command);
        }
    }
}
