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
		IAsyncEnumerable<TransactionViewModel> ReadTransactions(TransactionsFilter filter);
		Task<IEnumerable<TransactionTypeViewModel>> ReadTransactionTypes();
	}
	public class TransactionReader : ITransactionReader
	{
		private readonly ConnectionFactory connectionFactory;
		private readonly IMapper mapper;

		public TransactionReader(ConnectionFactory connectionFactory,
			IMapper mapper)
		{
			this.connectionFactory = connectionFactory;
			this.mapper = mapper;
		}

		public async IAsyncEnumerable<TransactionViewModel> ReadTransactions(TransactionsFilter filter)
		{
			using var connection = connectionFactory.CreateConnection();

			var builder = new SqlBuilder();
			var selector = builder.AddTemplate(@"
				SELECT top 100 bt.Id as TransactionId, bt.Title as Name, bt.TransactionDate, btt.Name as Type, g.Name as GroupName, g.RuleId as RuleId, bt.Extensions as Extensions, bt.Amount FROM BankTransactions bt
				JOIN BankTransactionTypes btt ON bt.BankTransactionTypeId = btt.Id
				LEFT JOIN BankTransactionGroups btg ON bt.Id = btg.BankTransactionId
				LEFT JOIN Groups g ON btg.GroupId = g.Id
				/**where**/
				ORDER BY bt.[Order] desc");

			if (filter.OnlyWithoutGroup)
				builder.Where("btg.BankTransactionId IS NULL");

			if (filter.GroupName != null)
				builder.Where("g.Name = @GroupName", new { filter.GroupName });

			var trasactionGroupsContainers = await connection.QueryAsync<TransactionGroupsContainer>(selector.RawSql, selector.Parameters);

			foreach (var transactionGroups in trasactionGroupsContainers.GroupBy(g => g.TransactionId))
			{
				var transactionGroup = transactionGroups.First();
				var viewModel = mapper.Map<TransactionViewModel>(transactionGroup);
				viewModel.Groups = transactionGroups.Where(t => t.GroupName != null).Select(t =>
					new TransactionViewModel.TransactionGroupViewModel(t.GroupName, t.RuleId == null)).ToList();
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
