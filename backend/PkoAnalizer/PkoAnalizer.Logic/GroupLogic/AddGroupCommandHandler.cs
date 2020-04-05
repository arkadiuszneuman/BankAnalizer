using BankAnalizer.Infrastructure.Commands;
using PkoAnalizer.Core;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.GroupLogic.Db;
using PkoAnalizer.Logic.GroupLogic.Events;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.GroupLogic
{
    public class AddGroupCommandHandler : ICommandHandler<AddGroupCommand>
    {
        private readonly GroupAccess groupAccess;
        private readonly IEventsBus eventsBus;

        public AddGroupCommandHandler(GroupAccess groupAccess,
            IEventsBus eventsBus)
        {
            this.groupAccess = groupAccess;
            this.eventsBus = eventsBus;
        }

        public async Task Handle(AddGroupCommand command)
        {
            var bankTransaction = await groupAccess.GetBankTransactionById(command.BankTransactionId);
            if (bankTransaction == null)
                throw new BankAnalizerException($"Bank transaction id {command.BankTransactionId} doesn't exist");

            var group = command.RuleId == default ?
                await groupAccess.GetGroupByName(command.GroupName, command.UserId) :
                await groupAccess.GetGroupByNameAndRuleId(command.GroupName, command.RuleId, command.UserId);

            if (group == null)
            {
                group = new Group { Name = command.GroupName };
                group.User = await groupAccess.GetUser(command.UserId);
                if (command.RuleId != default)
                    group.Rule = new Rule { Id = command.RuleId };
            }

            await groupAccess.AddBankTransactionToGroup(bankTransaction, group);
            _ = eventsBus.Publish(new GroupToTransactionAddedEvent(bankTransaction, group, command.UserId));
        }
    }
}
