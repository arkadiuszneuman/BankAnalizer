using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Commands.Rules;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.ViewModels.Rules;
using PkoAnalizer.Db;
using PkoAnalizer.Db.Models;
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

        public SaveRuleCommandHandler(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task Handle(SaveRuleCommand command)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RuleParsedViewModel, Rule>());
            var mapper = config.CreateMapper();

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
        }
    }
}
