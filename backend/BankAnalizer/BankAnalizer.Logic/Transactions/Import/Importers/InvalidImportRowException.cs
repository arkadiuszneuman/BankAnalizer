using BankAnalizer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAnalizer.Logic.Transactions.Import.Importers
{
    public class InvalidImportRowException : GenericBankAnalizerException
    {
        public InvalidImportRowException(string message) : base(message) { }
    }
}
