using BankAnalizer.Core.Api;
using BankAnalizer.Core.Cqrs.Event;
using System;

namespace BankAnalizer.Core.SignalR.Cqrs.Events
{
    public class CommandExceptionEvent : IEvent
    {
        public string ErrorMessage { get; }
        public Guid UserId { get;  }
        public Guid CommandId { get; }

        public CommandExceptionEvent(string errorMessage, Guid userId, Guid commandId)
        {
            ErrorMessage = errorMessage;
            UserId = userId;
            CommandId = commandId;
        }

        public static CommandExceptionEvent FromCommand(Command command, Exception exception) =>
            new CommandExceptionEvent(exception.Message, command.UserId, command.CommandId);

        public static CommandExceptionEvent FromCommand(Command command, string errorMessage) =>
            new CommandExceptionEvent(errorMessage, command.UserId, command.UserId);

    }
}
