using Dapper;
using PkoAnalizer.Db;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Read.Transactions
{
    public interface ITransactionReader
    {
        Task<IEnumerable<string>> ReadAllExtensionColumns();
        Task<IEnumerable<TransactionViewModel>> ReadTransactions();
        Task<IEnumerable<TransactionTypeViewModel>> ReadTransactionTypes();
    }

    public class TransactionReader : ITransactionReader
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

        public async Task<IEnumerable<TransactionTypeViewModel>> ReadTransactionTypes()
        {
            using var connection = connectionFactory.CreateConnection();

            return await connection.QueryAsync<TransactionTypeViewModel>(@"SELECT Id, Name FROM BankTransactionTypes");
        }

        public async Task<IEnumerable<string>> ReadAllExtensionColumns()
        {
            using var connection = connectionFactory.CreateConnection();
            return await connection.QueryAsync<string>(@"SELECT DISTINCT Extensions FROM BankTransactions 
                WHERE Extensions IS NOT NULL");
        }
    }
}
