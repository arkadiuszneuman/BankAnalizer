using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Db.Models;
using System;

namespace PkoAnalizer.Logic.Rules.Events
{
    public class RuleSavedEvent : IEvent
    {
        public Rule Rule { get; private set; }
        public Guid UserId { get; }

        public RuleSavedEvent(Rule rule, Guid userId)
        {
            Rule = rule;
            UserId = userId;
        }
    }
}
