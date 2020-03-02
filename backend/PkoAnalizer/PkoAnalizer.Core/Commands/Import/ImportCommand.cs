using PkoAnalizer.Core.Cqrs.Command;
using System;
using System.IO;

namespace PkoAnalizer.Core.Commands.Import
{
    public class ImportCommand : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ConnectionId { get; set; }
        public string FileText { get; }

        public ImportCommand(string connectionId, string file)
        {
            ConnectionId = connectionId;
            FileText = file;
        }
    }
}
