using System;
using System.Collections.Generic;
using System.Text;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class WebPaymentImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Płatność web - kod mobilny")
            {
                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Extensions = new PhoneNumberLocationExtension {
                        PhoneNumber = splittedLine.Index(6),
                        Location = splittedLine.Index(7),
                    }.ToJson()
                };
            }

            return null;
        }
    }
}
