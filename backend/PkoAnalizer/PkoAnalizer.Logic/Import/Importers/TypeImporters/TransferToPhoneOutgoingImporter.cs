﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class TransferToPhoneOutgoingImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var supportedTypes = new[] { "Przelew na telefon wychodzący zew.",
                "Przelew na telefon wychodzący wew." };
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
                    Extensions = new RecipientReceiptNameExtension {
                        RecipientReceipt = splittedLine.Index(6).RemoveSubstring("Rachunek odbiorcy:").Trim(),
                        RecipientName = splittedLine.Index(7).RemoveSubstring("Nazwa odbiorcy:").Trim()
                    }.ToJson(),
                    Title = splittedLine.Index(8).RemoveSubstring("Tytuł:").Trim()
                };
            }

            return null;
        }
    }
}
