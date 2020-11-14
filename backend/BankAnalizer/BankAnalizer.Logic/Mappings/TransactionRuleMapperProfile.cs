using AutoMapper;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Transactions.Import.Models;
using BankAnalizer.Logic.Transactions.Read.Containers;
using BankAnalizer.Logic.Transactions.Read.ViewModels;

namespace BankAnalizer.Logic.Mappings
{
    public class TransactionRuleMapperProfile : Profile
    {
        public TransactionRuleMapperProfile()
        {
            CreateMap<BankTransaction, ImportedBankTransaction>();
            CreateMap<TransactionGroupsContainer, TransactionViewModel>();

            CreateMap<BankTransaction, TransactionViewModel>()
                .ForMember(x => x.TransactionId, z => z.MapFrom(b => b.Id))
                .ForMember(x => x.Name, z => z.MapFrom(b => b.Title))
                .ForMember(x => x.Type, z => z.MapFrom(b => b.BankTransactionType.Name))
                .ForMember(x => x.BankName, z => z.MapFrom(b => b.Bank.Name));
        }
    }
}
