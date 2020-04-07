using Dapper.Contrib.Extensions;

namespace BankAnalizer.Logic.Charts.ViewModels
{
    public class GroupsViewModel
    {
        [Key]
        [ExplicitKey]
        public string GroupName { get; set; }
        public decimal Amount { get; set; }
    }
}
