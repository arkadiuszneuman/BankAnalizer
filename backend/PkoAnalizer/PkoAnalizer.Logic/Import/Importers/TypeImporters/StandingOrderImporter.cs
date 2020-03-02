﻿using System;
using System.Collections.Generic;
using System.Text;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class StandingOrderImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Zlecenie stałe")
            {
                var isAddressExists = splittedLine.Index(8).Contains("Adres odbiorcy:");
                var titleIndex = isAddressExists ? 9 : 8;

                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Extensions = new RecipientExtension {
                        RecipientReceipt = splittedLine.Index(6).RemoveSubstring("Rachunek odbiorcy:").Trim(),
                        RecipientName = splittedLine.Index(7).RemoveSubstring("Nazwa odbiorcy:").Trim(),
                        RecipientAddress = isAddressExists ? splittedLine.Index(8).RemoveSubstring("Adres odbiorcy:").Trim() : null,
                    }.ToJson(),
                    Title = splittedLine.Index(titleIndex).RemoveSubstring("Tytuł:").Trim()
                };
            }

            return null;
        }
    }
}
