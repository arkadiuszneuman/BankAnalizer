﻿using PkoAnalizer.Core.Commands.Group;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Logic.GroupLogic.Db;
using System.Threading.Tasks;
using System.Transactions;

namespace PkoAnalizer.Logic.GroupLogic
{
    public class RemoveGroupCommandHandler : ICommandHandler<RemoveGroupCommand>
    {
        private readonly GroupAccess groupAccess;

        public RemoveGroupCommandHandler(GroupAccess groupAccess)
        {
            this.groupAccess = groupAccess;
        }

        public async Task Handle(RemoveGroupCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var group = await groupAccess.GetGroupByName(command.GroupName, command.UserId);
                await groupAccess.RemoveBankTransactionsFromGroup(command.BankTransactionId, group.Id);
                await groupAccess.RemoveGroupIfBankTransactionsDoNotExist(group.Id);

                scope.Complete();
            }
        }
    }
}
