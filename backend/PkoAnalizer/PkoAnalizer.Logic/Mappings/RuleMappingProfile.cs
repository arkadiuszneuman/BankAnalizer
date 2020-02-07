using AutoMapper;
using PkoAnalizer.Core.ViewModels.Rules;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Rules.ViewModels;

namespace PkoAnalizer.Logic.Mappings
{
    public class RuleMappingProfile : Profile
    {
        public RuleMappingProfile()
        {
            CreateMap<RuleParsedViewModel, Rule>();
            CreateMap<Rule, RuleViewModel>();
        }
    }
}