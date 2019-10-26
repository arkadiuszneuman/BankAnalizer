using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Core.Cqrs.Command
{
    public interface ICommandHandler
    {

    }

    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}
