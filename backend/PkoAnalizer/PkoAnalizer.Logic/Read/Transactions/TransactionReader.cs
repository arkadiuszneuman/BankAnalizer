using AutoMapper;
using Dapper;
using PkoAnalizer.Db;
using PkoAnalizer.Logic.Read.Transactions.Containers;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Read.Transactions
{
	public interface ITransactionReader
	{
		Task<IEnumerable<string>> ReadAllExtensionColumns();
		IAsyncEnumerable<TransactionViewModel> ReadTransactions();
		Task<IEnumerable<TransactionTypeViewModel>> ReadTransactionTypes();
	}

	public class TransactionReader : ITransactionReader
	{
		private readonly ConnectionFactory connectionFactory;

		public TransactionReader(ConnectionFactory connectionFactory)
		{
			this.connectionFactory = connectionFactory;
		}

		public async IAsyncEnumerable<TransactionViewModel> ReadTransactions()
		{
			var config = new MapperConfiguration(cfg => cfg.CreateMap<TransactionGroupsContainer, TransactionViewModel>());
			var mapper = config.CreateMapper();

			using var connection = connectionFactory.CreateConnection();

			var trasactionGroupsContainers = await connection.QueryAsync<TransactionGroupsContainer>(@"
				SELECT bt.Id as TransactionId, bt.Title as Name, btt.Name as Type, g.Name as GroupName, bt.Extensions as Extensions FROM BankTransactions bt
				JOIN BankTransactionTypes btt ON bt.BankTransactionTypeId = btt.Id
				LEFT JOIN BankTransactionGroups btg ON bt.Id = btg.BankTransactionId
				LEFT JOIN Groups g ON btg.GroupId = g.Id
				ORDER BY bt.[Order] desc");

			foreach (var transactionGroups in trasactionGroupsContainers.GroupBy(g => g.TransactionId))
			{
				var transactionGroup = transactionGroups.First();
				var viewModel = mapper.Map<TransactionViewModel>(transactionGroup);
				viewModel.Groups = transactionGroups.Select(t => t.GroupName).ToList();
				yield return viewModel;
			}
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
