using Dapper;
using Dapper.Contrib.Extensions;
using PkoAnalizer.Db;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Rules.ViewModels;
using System;
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

        public async Task<IEnumerable<RuleViewModel>> GetRules(Guid userId)
        {
            using var connection = connectionFactory.CreateConnection();
            return await connection.QueryAsync<RuleViewModel>("SELECT * FROM Rules WHERE UserId = @userId", new { userId });
        }

        public async Task<IEnumerable<BankTransaction>> GetBankTransactions(Guid userId)
        {
            using var connection = connectionFactory.CreateConnection();
            return await connection.QueryAsync<BankTransaction>("SELECT * FROM BankTransactions WHERE UserId = @userId", new { userId });
        }
    }
}
