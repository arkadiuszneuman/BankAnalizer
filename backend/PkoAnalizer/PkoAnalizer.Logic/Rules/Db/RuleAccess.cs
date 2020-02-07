using Dapper.Contrib.Extensions;
using PkoAnalizer.Db;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Rules.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Rules.Db
{
    public class RuleAccess
    {
        private readonly ConnectionFactory connectionFactory;

        public RuleAccess(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<RuleViewModel>> GetRules()
        {
            using var connection = connectionFactory.CreateConnection();
            return await connection.GetAllAsync<RuleViewModel>();
        }

        public async Task<IEnumerable<BankTransaction>> GetBankTransactions()
        {
            using var connection = connectionFactory.CreateConnection();
            return await connection.GetAllAsync<BankTransaction>();
        }
    }
}
