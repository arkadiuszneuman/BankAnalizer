using PkoAnalizer.Core;
using PkoAnalizer.Core.Commands.Group;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.GroupLogic.Db;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.GroupLogic
{
    public class AddGroupCommandHandler : ICommandHandler<AddGroupCommand>
    {
        private readonly GroupAccess groupAccess;

        public AddGroupCommandHandler(GroupAccess groupAccess)
        {
            this.groupAccess = groupAccess;
        }

        public async Task Handle(AddGroupCommand command)
        {
            var bankTransaction = await groupAccess.GetBankTransactionById(command.BankTransactionId);
            if (bankTransaction == null)
                throw new PkoAnalizerException($"Bank transaction id {command.BankTransactionId} doesn't exist");

            var group = command.RuleId == default ?
                await groupAccess.GetGroupByName(command.GroupName) :
                await groupAccess.GetGroupByNameAndRuleId(command.GroupName, command.RuleId);

            if (group == null)
            {
                group = new Group { Name = command.GroupName };
                if (command.RuleId != default)
                    group.Rule = new Rule { Id = command.RuleId };
            }

            await groupAccess.AddBankTransactionToGroup(bankTransaction, group);
        }
    }
}
