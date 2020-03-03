using PkoAnalizer.Core.Cqrs.Command;
using System;
using System.IO;

namespace PkoAnalizer.Core.Commands.Import
{
    public class ImportCommand : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ConnectionId { get; set; }
        public Guid UserId { get; }
        public string FileText { get; }

        public ImportCommand(string connectionId, Guid userId, string file)
        {
            ConnectionId = connectionId;
            UserId = userId;
            FileText = file;
        }
    }
}
