using AutoMapper;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Import.Models;
using PkoAnalizer.Logic.Read.Transactions.Containers;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;

namespace PkoAnalizer.Logic.Mappings
{
    public class TransactionRuleMapperProfile : Profile
    {
        public TransactionRuleMapperProfile()
        {
            CreateMap<BankTransaction, PkoTransaction>();
            CreateMap<TransactionGroupsContainer, TransactionViewModel>();

            CreateMap<BankTransaction, TransactionViewModel>()
                .ForMember(x => x.TransactionId, z => z.MapFrom(b => b.Id))
                .ForMember(x => x.Name, z => z.MapFrom(b => b.Title))
                .ForMember(x => x.Type, z => z.MapFrom(b => "asdasd"));
        }
    }
}
