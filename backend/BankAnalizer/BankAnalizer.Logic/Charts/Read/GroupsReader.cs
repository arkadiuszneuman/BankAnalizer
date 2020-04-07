using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Db;
using BankAnalizer.Logic.Charts.ViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Charts.Read
{
    public class GroupsReader
    {
        private readonly IConnectionFactory connectionFactory;

        public GroupsReader(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<GroupsViewModel>> GetGroups(DateTime? dateFrom, DateTime? dateTo, Guid userId)
        {
            using var connection = connectionFactory.CreateConnection();

            var queryString = $@"SELECT Name as GroupName, SUM(Amount) * -1 as Amount FROM Groups
                    JOIN BankTransactionGroups ON Groups.Id = BankTransactionGroups.GroupId
                    JOIN BankTransactions ON BankTransactionGroups.BankTransactionId = BankTransactions.Id /**filterBankTransactions**/
                    GROUP BY Name, BankTransactions.UserId
                    HAVING SUM(Amount) < 0 AND BankTransactions.UserId = '{userId}'";
            string filterBankTransactionsQuery = GetFilterTransactionsQuery(dateFrom, dateTo);

            queryString = queryString.Replace("/**filterBankTransactions**/", filterBankTransactionsQuery);

            return await connection.QueryAsync<GroupsViewModel>(queryString);
        }

        private static string GetFilterTransactionsQuery(DateTime? dateFrom, DateTime? dateTo) =>
            (dateFrom, dateTo) switch
            {
                (null, null) => "",
                (_, null) => $"AND BankTransactions.TransactionDate >= '{dateFrom.Value.ToSqlDateTimeString()}'",
                (null, _) => $"AND BankTransactions.TransactionDate <= '{dateTo.Value.ToSqlDateTimeString()}'",
                (_, _) => $"AND BankTransactions.TransactionDate BETWEEN '{dateFrom.Value.ToSqlDateTimeString()}' AND '{dateTo.Value.ToSqlDateTimeString()}'",
            };
    }
}
