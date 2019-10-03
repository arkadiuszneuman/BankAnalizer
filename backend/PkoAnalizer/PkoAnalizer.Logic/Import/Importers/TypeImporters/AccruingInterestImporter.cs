using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Linq;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class AccruingInterestImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var supportedTypes = new[] { "Naliczenie odsetek", "Opłata za użytkowanie karty", "Prowizja",
                "Opłata składki ubezpieczeniowej", "Uznanie", "Podatek od odsetek" };
            var type = splittedLine.Index(2);
            if (supportedTypes.Contains(type))
            {
                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Title = splittedLine.Index(6).RemoveSubstring("Tytuł:").Trim()
                };
            }

            return null;
        }
    }
}
