using Dapper;
using PkoAnalizer.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Read.Charts.Groups
{
    public class GroupsReader
    {
        private readonly ConnectionFactory connectionFactory;

        public GroupsReader(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<GroupsViewModel>> GetGroups()
        {
            using var connection = connectionFactory.CreateConnection();

            return await connection.QueryAsync<GroupsViewModel>(
                    @"SELECT Name as GroupName, SUM(Amount) * -1 as Amount FROM Groups
                    JOIN BankTransactionGroups ON Groups.Id = BankTransactionGroups.GroupId
                    JOIN BankTransactions ON BankTransactionGroups.BankTransactionId = BankTransactions.Id
                    GROUP BY Name
                    HAVING SUM(Amount) < 0");
        }
    }
}
