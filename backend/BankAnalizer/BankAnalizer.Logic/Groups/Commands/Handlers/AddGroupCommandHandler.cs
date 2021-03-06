﻿using BankAnalizer.Core;
using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Groups.Db;
using BankAnalizer.Logic.Groups.Events;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Groups.Commands.Handlers
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
