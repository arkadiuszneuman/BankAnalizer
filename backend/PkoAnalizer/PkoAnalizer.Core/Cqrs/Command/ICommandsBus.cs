using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Core.Cqrs.Command
{
    public interface ICommandsBus
    {
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
