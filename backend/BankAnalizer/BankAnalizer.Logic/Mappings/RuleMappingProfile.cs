using AutoMapper;
using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Rules;
using BankAnalizer.Logic.Rules.Commands;
using BankAnalizer.Logic.Rules.ViewModels;

namespace BankAnalizer.Logic.Mappings
{
    public class RuleMappingProfile : Profile
    {
        public RuleMappingProfile()
        {
            CreateMap<SaveRuleCommand, Rule>()
                .ForMember(x => x.RuleDefinition, z => z.MapFrom(rule => $"{rule.ColumnId} {rule.Type} {rule.Text}"));
            CreateMap<RuleParsedViewModel, ParsedRule>()
                .ForMember(x => x.Value, z => z.MapFrom(r => r.Text))
                .ForMember(x => x.RuleType, z => z.MapFrom(r => r.Type))
                .ForMember(x => x.Column, z => z.MapFrom(r => r.ColumnId.RemoveSubstring("Extensions.")))
                .ForMember(x => x.IsColumnInExtensions, z => z.MapFrom(r => r.ColumnId.StartsWith("Extensions.")));
            CreateMap<ParsedRule, RuleParsedViewModel>()
                .ForMember(x => x.Text, z => z.MapFrom(r => r.Value))
                .ForMember(x => x.Type, z => z.MapFrom(r => r.RuleType.ToString()))
                .ForMember(x => x.ColumnId, z => z.MapFrom(r => r.IsColumnInExtensions ? "Extensions." + r.Column : r.Column));
            CreateMap<Rule, RuleViewModel>();
        }
    }
}