using AutoMapper;
using Dapper;
using PkoAnalizer.Db;
using PkoAnalizer.Logic.Transactions.Read.Containers;
using PkoAnalizer.Logic.Transactions.Read.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Transactions.Read
{
	public interface ITransactionReader
	{
		Task<IEnumerable<string>> ReadAllExtensionColumns(Guid userId);
		IAsyncEnumerable<TransactionViewModel> ReadTransactions(TransactionsFilter filter, Guid userId);
		Task<IEnumerable<TransactionTypeViewModel>> ReadTransactionTypes(Guid userId);
	}
	public class TransactionReader : ITransactionReader
	{
		private readonly IConnectionFactory connectionFactory;
		private readonly IMapper mapper;

		public TransactionReader(IConnectionFactory connectionFactory,
			IMapper mapper)
		{
			this.connectionFactory = connectionFactory;
			this.mapper = mapper;
		}

		public async IAsyncEnumerable<TransactionViewModel> ReadTransactions(TransactionsFilter filter, Guid userId)
		{
			if (filter.Users?.Any() ?? false)
			{
				using var connection = connectionFactory.CreateConnection();

				var builder = new SqlBuilder();
				var selector = builder.AddTemplate(@"
				SELECT bt.Id as TransactionId, bt.Title as Name, bt.TransactionDate, btt.Name as Type, g.Name as GroupName, g.RuleId as RuleId, bt.Extensions as Extensions, bt.Amount FROM BankTransactions bt
				JOIN BankTransactionTypes btt ON bt.BankTransactionTypeId = btt.Id
				LEFT JOIN BankTransactionGroups btg ON bt.Id = btg.BankTransactionId
				LEFT JOIN Groups g ON btg.GroupId = g.Id
				/**where**/
				ORDER BY bt.[Order] desc");

				if (filter.OnlyWithoutGroup)
					builder.Where("btg.BankTransactionId IS NULL");

				if (filter.GroupName != null)
					builder.Where("g.Name = @GroupName", new { filter.GroupName });

				if (filter.DateFrom != null && filter.DateTo != null)
					builder.Where("bt.TransactionDate BETWEEN @DateFrom AND @DateTo", new { filter.DateFrom, filter.DateTo });

				builder.Where("bt.UserId IN @usersIds", new { usersIds = filter.Users });

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
		}

		public async Task<IEnumerable<TransactionTypeViewModel>> ReadTransactionTypes(Guid userId)
		{
			using var connection = connectionFactory.CreateConnection();

			return await connection.QueryAsync<TransactionTypeViewModel>(
				@"SELECT Id, Name FROM BankTransactionTypes WHERE UserId = @userId", new { userId });
		}

		public async Task<IEnumerable<string>> ReadAllExtensionColumns(Guid userId)
		{
			using var connection = connectionFactory.CreateConnection();
			return await connection.QueryAsync<string>(@"SELECT DISTINCT Extensions FROM BankTransactions 
				WHERE Extensions IS NOT NULL AND UserId = @userId", new { userId });
		}
	}
}
