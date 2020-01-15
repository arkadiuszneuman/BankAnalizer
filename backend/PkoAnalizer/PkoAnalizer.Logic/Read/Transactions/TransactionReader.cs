using Dapper;
using Dapper.Contrib.Extensions;
using PkoAnalizer.Db;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Read.Transactions
{
    public class TransactionReader
    {
        private readonly ConnectionFactory connectionFactory;

        public TransactionReader(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TransactionViewModel>> ReadTransactions()
        {
            using var connection = connectionFactory.CreateConnection();

            return await connection.QueryAsync<TransactionViewModel>(@"
                SELECT bt.Id as TransactionId, bt.Title as Name, btt.Name as Type FROM BankTransactions bt
                JOIN BankTransactionTypes btt ON bt.BankTransactionTypeId = btt.Id
                ORDER BY bt.[Order] desc");
        }
    }
}
