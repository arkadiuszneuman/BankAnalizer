using PkoAnalizer.Logic.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PkoAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class ImportersFixture
    {
        public IEnumerable<(Type, string[], PkoTransaction)> GetTransactions()
        {
            yield return
                (typeof(WithdrawalFromAtm),
                new[] {
                    "2019-02-09", "2019-02-08", "Wypłata z bankomatu", "-321.32", "PLN", "+32.73",
                    "Tytuł: PKO BP 123123123", "Lokalizacja: Kraj: POLSKA Miasto: SOMECITY Adres: UL. SOMNEADDRESS 45",
                    "Data i czas operacji: 2019-02-08 11:34:51", "Oryginalna kwota operacji: 32,73 PLN", "Numer karty: 425125******4284", "", ""
                },
                new PkoTransaction
                {
                    OperationDate = new DateTime(2019, 2, 9),
                    TransactionDate = new DateTime(2019, 2, 8),
                    TransactionType = "Wypłata z bankomatu",
                    Amount = -321.32M,
                    Currency = "PLN",
                    Title = "Tytuł: PKO BP 123123123",
                    Location = "Lokalizacja: Kraj: POLSKA Miasto: SOMECITY Adres: UL. SOMNEADDRESS 45",
                });
        }

        public (string[], PkoTransaction) GetTransaction<T>()
        {
            return GetTransactions()
                .Where(t => t.Item1 == typeof(T))
                .Select(x => (x.Item2, x.Item3))
                .Single();
        }
    }
}
