using BankAnalizer.Core.Cqrs.Event;
using System;

namespace BankAnalizer.Logic.CommonEvents
{
    public class ErrorEvent : IEvent
    {
        public string ErrorMessage { get; set; }
        public Guid UserId { get; set; }

        public ErrorEvent(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
