using Microsoft.Extensions.Logging;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Import.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Import.Rules
{
    public class DatabaseRulesEventHandler : IHandleEvent<TransactionSavedEvent>
    {
        private readonly ILogger<DatabaseRulesEventHandler> logger;

        public DatabaseRulesEventHandler(ILogger<DatabaseRulesEventHandler> logger)
        {
            this.logger = logger;
        }

        public async Task Handle(TransactionSavedEvent @event)
        {
            
        }
    }
}
