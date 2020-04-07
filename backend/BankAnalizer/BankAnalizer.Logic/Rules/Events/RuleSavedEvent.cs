using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Db.Models;
using System;

namespace BankAnalizer.Logic.Rules.Events
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
