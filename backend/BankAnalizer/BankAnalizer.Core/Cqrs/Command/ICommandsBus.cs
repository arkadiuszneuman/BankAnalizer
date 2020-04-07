using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankAnalizer.Core.Cqrs.Command
{
    public interface ICommandsBus
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task SendAsync(Type commandType, ICommand command);
    }
}
