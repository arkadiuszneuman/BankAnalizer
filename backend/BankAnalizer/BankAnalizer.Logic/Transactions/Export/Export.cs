using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Db;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Transactions.Export
{
    public class Export
    {
        private readonly IContextFactory contextFactory;

        public Export(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<string> GetExportedJson()
        {
            using var context = contextFactory.GetContext();
            var bankTransactionTypes = await context.BankTransactionTypes.ToListAsync();
            var rules = await context.Rules.ToListAsync();
            var groups = await context.Groups.ToListAsync();

            return "!BANKTRANSACTIONTYPES!" + "\r\n" + bankTransactionTypes.ToJson() + "\r\n" +
                "!RULES!" + "\r\n" + rules.ToJson() + "\r\n" +
                "!GROUPS!" + "\r\n" + groups.ToJson();
        }
    }
}
