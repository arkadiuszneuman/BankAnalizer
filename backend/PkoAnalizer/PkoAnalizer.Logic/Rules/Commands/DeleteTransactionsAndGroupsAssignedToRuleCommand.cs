using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Db.Models;

namespace PkoAnalizer.Logic.Rules.Commands
{
    public class DeleteTransactionsAndGroupsAssignedToRuleCommand : ICommand
    {
        public Rule Rule { get; set; }

        public DeleteTransactionsAndGroupsAssignedToRuleCommand(Rule rule)
        {
            Rule = rule;
        }
    }
}
