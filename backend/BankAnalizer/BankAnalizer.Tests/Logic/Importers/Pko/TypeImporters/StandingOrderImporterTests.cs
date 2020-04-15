using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class StandingOrderImporterTests : BaseUnitTest<StandingOrderImporter>
    {
        [Fact]
        public void Should_import_valid_standing_order_transactions()
        {
            //act
            var result = Sut.Import(new[] { "2019-03-01","2019-03-02","Zlecenie stałe","-73.21","PLN",
                "+321.07","Rachunek odbiorcy: 11 2222 3333 0000 9999 8888 7777",
                "Nazwa odbiorcy: SOME RECIPIENT NAME",
                "Adres odbiorcy: SOME ADDRESS 32/45 CITY",
                "Tytuł: NAME SURNAMESTREET 32/441-506 CHORZÓW","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 3, 1),
                TransactionDate = new DateTime(2019, 3, 2),
                TransactionType = "Zlecenie stałe",
                Amount = -73.21M,
                Currency = "PLN",
                Extensions = new RecipientExtension
                {
                    RecipientReceipt = "11 2222 3333 0000 9999 8888 7777",
                    RecipientName = "SOME RECIPIENT NAME",
                    RecipientAddress = "SOME ADDRESS 32/45 CITY",
                }.ToJson(),
                Title = "NAME SURNAMESTREET 32/441-506 CHORZÓW",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //act
            var result = Sut.Import(new[] { "2019-03-01","2019-03-02","Some other type","-73.21","PLN",
                "+321.07","Rachunek odbiorcy: 11 2222 3333 0000 9999 8888 7777",
                "Nazwa odbiorcy: SOME RECIPIENT NAME",
                "Adres odbiorcy: SOME ADDRESS 32/45 CITY",
                "Tytuł: NAME SURNAMESTREET 32/441-506 CHORZÓW","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
