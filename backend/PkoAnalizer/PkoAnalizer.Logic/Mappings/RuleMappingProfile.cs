using AutoMapper;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Core.ViewModels.Rules;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Rules;
using PkoAnalizer.Logic.Rules.ViewModels;

namespace PkoAnalizer.Logic.Mappings
{
    public class RuleMappingProfile : Profile
    {
        public RuleMappingProfile()
        {
            CreateMap<RuleParsedViewModel, Rule>()
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