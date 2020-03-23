using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Read.Transactions
{
    public class ColumnFinder
    {
        private readonly ITransactionReader transactionReader;

        public ColumnFinder(ITransactionReader transactionReader)
        {
            this.transactionReader = transactionReader;
        }

        public async Task<IEnumerable<ColumnViewModel>> FindColumns(Guid userId)
        {
            var extensions = await transactionReader.ReadAllExtensionColumns(userId);

            var staticColumns = GetStaticColumns();
            var columns = extensions.SelectMany(x => GetExtensionColumns(x));

            return staticColumns
                .Union(columns)
                .DistinctBy(x => x.Id);
        }

        private IEnumerable<ColumnViewModel> GetStaticColumns()
        {
            yield return new ColumnViewModel { Id = "Title", Name = "Title" };
        }

        private IEnumerable<ColumnViewModel> GetExtensionColumns(string column)
        {
            return column
                .TrimStart('{')
                .TrimEnd('}')
                .Split("\",\"")
                .Select(x => x.Split(":").First().TrimStart('"').TrimEnd('"'))
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new ColumnViewModel { Id = "Extensions." + x, Name = x });
        }
    }
}
