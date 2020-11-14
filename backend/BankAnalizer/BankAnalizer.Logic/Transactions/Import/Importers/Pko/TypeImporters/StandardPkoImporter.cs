using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Models;
using System.Dynamic;
using System.Linq;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters
{
    public class StandardPkoImporter : IPkoTypeImporter
    {
        public ImportedBankTransaction Import(string[] splittedLine)
        {
            var extensions = GetExtensions(splittedLine).ToJson();
            if (extensions == "{}")
                extensions = null;

            return new ImportedBankTransaction
            {
                BankName = "PKO BP",
                OperationDate = splittedLine.Index(0).ConvertToDate(),
                TransactionDate = splittedLine.Index(1).ConvertToDate(),
                TransactionType = splittedLine.Index(2),
                Amount = splittedLine.Index(3).ConvertToDecimal(),
                Currency = splittedLine.Index(4),
                Title = GetTitle(splittedLine),
                Extensions = extensions
            };
        }

        private static string GetTitle(string[] splittedLine)
        {
            var line = splittedLine.FirstOrDefault(l => l.StartsWith("Tytuł:"));
            if (line != null)
                return line.RemoveSubstring("Tytuł:").Trim();

            return splittedLine.Index(2).Trim();
        }

        private static object GetExtensions(string[] splittedLine)
        {
            dynamic extensions = new ExpandoObject();

            foreach (var line in splittedLine)
            {
                if (line.StartsWith("Lokalizacja:"))
                    extensions.Location = line.RemoveSubstring("Lokalizacja:").Trim();

                if (line.StartsWith("Numer telefonu:"))
                    extensions.PhoneNumber = line.RemoveSubstring("Numer telefonu:").Trim();

                if (line.StartsWith("Rachunek odbiorcy:"))
                    extensions.RecipientReceipt = line.RemoveSubstring("Rachunek odbiorcy:").Trim();

                if (line.StartsWith("Nazwa odbiorcy:"))
                    extensions.RecipientName = line.RemoveSubstring("Nazwa odbiorcy:").Trim();

                if (line.StartsWith("Adres odbiorcy:"))
                    extensions.RecipientAddress = line.RemoveSubstring("Adres odbiorcy:").Trim();

                if (line.StartsWith("Rachunek nadawcy:"))
                    extensions.SenderReceipt = line.RemoveSubstring("Rachunek nadawcy:").Trim();

                if (line.StartsWith("Nazwa nadawcy:"))
                    extensions.SenderName = line.RemoveSubstring("Nazwa nadawcy:").Trim();

                if (line.StartsWith("Adres nadawcy:"))
                    extensions.SenderAddress = line.RemoveSubstring("Adres nadawcy:").Trim();
            }

            return extensions;
        }
    }
}
