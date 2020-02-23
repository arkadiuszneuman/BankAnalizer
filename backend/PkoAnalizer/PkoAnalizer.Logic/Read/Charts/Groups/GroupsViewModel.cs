using Dapper.Contrib.Extensions;

namespace PkoAnalizer.Logic.Read.Charts.Groups
{
    public class GroupsViewModel
    {
        [Key]
        [ExplicitKey]
        public string GroupName { get; set; }
        public decimal Amount { get; set; }
    }
}
