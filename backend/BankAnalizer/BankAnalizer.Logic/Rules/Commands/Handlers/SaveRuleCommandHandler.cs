using AutoMapper;
using BankAnalizer.Core;
using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Db;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.CommonEvents;
using BankAnalizer.Logic.Rules.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace BankAnalizer.Logic.Rules.Commands.Handlers
{
    public class SaveRuleCommandHandler : ICommandHandler<SaveRuleCommand>
    {
        private readonly IContextFactory contextFactory;
        private readonly IMapper mapper;
        private readonly IEventsBus eventsBus;

        public SaveRuleCommandHandler(IContextFactory contextFactory,
            IMapper mapper,
            IEventsBus eventsBus)
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
            this.eventsBus = eventsBus;
        }

        public async Task Handle(SaveRuleCommand command)
        {
            using var context = contextFactory.GetContext();
            Rule rule;
            if (command.Id != default)
                rule = await context.Rules.SingleOrDefaultAsync(r => r.Id == command.Id) ?? new Rule();
            else
                rule = new Rule();

            var isNewRule = rule.Id == default;
            mapper.Map(command, rule);

            if (isNewRule)
            {
                rule.Id = Guid.NewGuid();
                await context.AddAsync(rule);
            }

            if (command.BankTransactionTypeId != null)
                rule.BankTransactionType = await context.BankTransactionTypes.SingleAsync(b => b.Id == command.BankTransactionTypeId);
            else
                rule.BankTransactionType = null;

            rule.User = await context.Users.FindAsync(command.UserId);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await context.SaveChangesAsync();
                await eventsBus.Publish(new RuleSavedEvent(rule, command.UserId));

                scope.Complete();
            }

            await eventsBus.Publish(new CommandCompletedEvent(command));
        }
    }
}
