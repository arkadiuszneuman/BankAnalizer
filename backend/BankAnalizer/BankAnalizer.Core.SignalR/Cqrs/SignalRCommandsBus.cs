using BankAnalizer.Core.Api;
using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Core.SignalR.Cqrs.Events;
using System;
using System.Threading.Tasks;

namespace BankAnalizer.Core.SignalR.Cqrs
{
    public class SignalRCommandsBus : ICommandsBus
    {
        private readonly ICommandsBus commandsBus;
        private readonly IEventsBus eventsBus;

        public SignalRCommandsBus(ICommandsBus commandsBus, IEventsBus eventsBus)
        {
            this.commandsBus = commandsBus;
            this.eventsBus = eventsBus;
        }

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            try
            {
                await commandsBus.SendAsync(command);
            }
            catch (NonGenericException e) when (command is Command)
            {
                await eventsBus.Publish(CommandExceptionEvent.FromCommand(command as Command, e));
            }
            catch when (command is Command)
            {
                await eventsBus.Publish(CommandExceptionEvent.FromCommand(command as Command, "Unknown error"));
            }
        }

        public async Task SendAsync(Type commandType, ICommand command)
        {
            try
            {
                await commandsBus.SendAsync(commandType, command);
            }
            catch (NonGenericException e) when (command is Command)
            {
                await eventsBus.Publish(CommandExceptionEvent.FromCommand(command as Command, e));
            }
            catch when (command is Command)
            {
                await eventsBus.Publish(CommandExceptionEvent.FromCommand(command as Command, "Unknown error"));
            }
        }
    }
}
