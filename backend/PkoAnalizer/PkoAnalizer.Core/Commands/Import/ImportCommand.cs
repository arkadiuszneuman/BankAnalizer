using PkoAnalizer.Core.Cqrs.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Core.Commands.Import
{
    public class ImportCommand : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ConnectionId { get; set; }

        public ImportCommand(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}
