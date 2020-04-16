using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class InstantTransferToAccountImporterTests : BaseUnitTest<InstantTransferToAccountImporter>
    {
        [Fact]
        public void Should_import_instant_transfer_to_transactions()
        {
            //act
            var result = Sut.Import(new[] {
                "2019-03-23","2019-03-24","Przelew Natychmiastowy na rachunek","+123.12","PLN","+32.06",
                "Rachunek nadawcy: 44 2222 5555 1111 7777 4444 3333","Nazwa nadawcy: SOME RECIPIENT",
                "Tytuł: PRZELEW ŚRODKÓW","","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new ImportedBankTransaction
            {
                OperationDate = new DateTime(2019, 3, 23),
                TransactionDate = new DateTime(2019, 3, 24),
                TransactionType = "Przelew Natychmiastowy na rachunek",
                Amount = 123.12M,
                Currency = "PLN",
                Extensions = new RecipientReceiptNameExtension
                {
                    RecipientReceipt = "44 2222 5555 1111 7777 4444 3333",
                    RecipientName = "SOME RECIPIENT"
                }.ToJson(),
                Title = "PRZELEW ŚRODKÓW",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //act
            var result = Sut.Import(new[] { "2019-03-23","2019-03-24","Other type","+9.72","PLN","+22.54","Tytuł: 5425465 55487541",
                "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: ADDRESS",
                "Data i czas operacji: 2019-03-24","Oryginalna kwota operacji: 2,75 USD",
                "Data przetworzenia: 2019-03-24","Numer karty: 2156452*****458",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
