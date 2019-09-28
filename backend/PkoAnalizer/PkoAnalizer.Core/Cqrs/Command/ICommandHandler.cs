using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Core.Cqrs.Command
{
    public interface ICommandHandler
    {

    }

    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}
