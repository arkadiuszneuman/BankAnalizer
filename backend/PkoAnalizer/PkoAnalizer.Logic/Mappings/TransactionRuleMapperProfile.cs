using AutoMapper;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Import.Models;

namespace PkoAnalizer.Logic.Mappings
{
    public class TransactionRuleMapperProfile : Profile
    {
        public TransactionRuleMapperProfile()
        {
            CreateMap<BankTransaction, PkoTransaction>();
        }
    }
}
