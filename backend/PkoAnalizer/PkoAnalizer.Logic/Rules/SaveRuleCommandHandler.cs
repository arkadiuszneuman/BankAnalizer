using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Commands.Rules;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Core.ViewModels.Rules;
using PkoAnalizer.Db;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Rules.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Rules
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
            if (command.Rule.Id != Guid.Empty)
            {
                rule = await context.Rules.SingleOrDefaultAsync(r => r.Id == command.Rule.Id);
                mapper.Map(command.Rule, rule);
            }
            else
            {
                rule = new Rule();
                mapper.Map(command.Rule, rule);
                await context.AddAsync(rule);
            }

            rule.RuleDefinition = $"{command.Rule.ColumnId} {command.Rule.Type} {command.Rule.Text}";

            if (command.Rule.BankTransactionTypeId != null)
                rule.BankTransactionType = await context.BankTransactionTypes.SingleAsync(b => b.Id == command.Rule.BankTransactionTypeId);
            else
                rule.BankTransactionType = null;

            await context.SaveChangesAsync();
            await eventsBus.Publish(new RuleSavedEvent(rule));
        }
    }
}
