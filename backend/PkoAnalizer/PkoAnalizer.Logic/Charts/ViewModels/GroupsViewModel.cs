using Dapper.Contrib.Extensions;

namespace PkoAnalizer.Logic.Charts.ViewModels
{
    public class GroupsViewModel
    {
        [Key]
        [ExplicitKey]
        public string GroupName { get; set; }
        public decimal Amount { get; set; }
    }
}
