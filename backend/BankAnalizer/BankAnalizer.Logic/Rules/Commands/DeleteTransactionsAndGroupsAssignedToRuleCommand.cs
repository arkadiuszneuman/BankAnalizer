using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Db.Models;

namespace BankAnalizer.Logic.Rules.Commands
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
