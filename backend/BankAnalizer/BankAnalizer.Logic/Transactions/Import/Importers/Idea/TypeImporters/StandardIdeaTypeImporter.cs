using System.Dynamic;
using System.Text;
using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Models;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Idea.TypeImporters
{
    public class StandardIdeaTypeImporter : IIdeaTypeImporter
    {
        public ImportedBankTransaction Import(string[] splittedLine)
        {
            return new ImportedBankTransaction
            {
                TransactionDate = string.IsNullOrEmpty(splittedLine.Index(0))
                    ? splittedLine.Index(1).ConvertToDate()
                    : splittedLine.Index(0).ConvertToDate(),
                OperationDate = splittedLine.Index(1).ConvertToDate(),
                TransactionType = TransactionTypeEnum.Transfer.ToString(),
                Amount = splittedLine.Index(9).ConvertToDecimal() * OperationType(splittedLine),
                Currency = splittedLine.Index(10),
                Title = splittedLine.Index(21),
                Extensions = GetExtensions(splittedLine).ToJson()
            };
        }

        private decimal OperationType(string[] splittedLine)
        {
            return splittedLine.Index(23).Trim().ToLower() == "uznanie" ? 1 : -1;
        }

        private static object GetExtensions(string[] splittedLine)
        {
            dynamic extensions = new ExpandoObject();

            var senderReceipt = splittedLine.Index(2).Trim();
            if (!string.IsNullOrEmpty(senderReceipt))
                extensions.SenderReceipt = senderReceipt;

            var sender = JoinMultipleColumnsIntoOne(splittedLine.Index(4), splittedLine.Index(5).Trim(),
                splittedLine.Index(6).Trim(), splittedLine.Index(6).Trim());
            if (!string.IsNullOrEmpty(sender))
                extensions.Sender = sender;

            var recipientReceipt = splittedLine.Index(13).Trim();
            if (!string.IsNullOrEmpty(recipientReceipt))
                extensions.RecipientReceipt = recipientReceipt;

            var recipient = JoinMultipleColumnsIntoOne(splittedLine.Index(15), splittedLine.Index(16).Trim(),
                splittedLine.Index(17).Trim(), splittedLine.Index(18).Trim());
            if (!string.IsNullOrEmpty(recipientReceipt))
                extensions.RecipientReceipt = recipientReceipt;

            return extensions;
        }

        private static string JoinMultipleColumnsIntoOne(params string[] columns)
        {
            var sb = new StringBuilder();

            foreach (var column in columns)
            {
                var columnValue = column.Trim();
                if (!string.IsNullOrEmpty(columnValue))
                {
                    sb.Append(columnValue);
                    if (columnValue.Length < 36)
                        sb.Append(" ");
                }
            }

            return sb.ToString().Trim();
        }
    }
}