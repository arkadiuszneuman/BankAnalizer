using PkoAnalizer.Core.Cqrs.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Import.Events
{
    public class SignalRTransactionsImported : IEvent
    {
        public string ConnectionId { get; }
        public Guid Id { get; }

        public SignalRTransactionsImported(string connectionId, Guid id)
        {
            ConnectionId = connectionId;
            Id = id;
        }

    }
}
