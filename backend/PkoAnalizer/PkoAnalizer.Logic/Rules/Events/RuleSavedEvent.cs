using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Db.Models;

namespace PkoAnalizer.Logic.Rules.Events
{
    public class RuleSavedEvent : IEvent
    {
        public Rule Rule { get; private set; }

        public RuleSavedEvent(Rule rule)
        {
            Rule = rule;
        }
    }
}
