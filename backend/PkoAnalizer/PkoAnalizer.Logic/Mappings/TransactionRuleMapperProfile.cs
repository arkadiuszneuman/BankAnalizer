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
        }
    }
}
