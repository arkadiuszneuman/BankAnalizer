using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class CashPaymentInAtmImporterTests : BaseUnitTest<CashPaymentInAtmImporter>
    {
        [Fact]
        public void Should_import_cash_payment_in_atc_transactions()
        {
            //act
            var result = Sut.Import(new[] {
                "2018-03-23","2018-03-24","Wpłata gotówki we wpłatomacie","+11.21","PLN","+32.22",
                "Tytuł: PKO BP 123123123123",
                "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: STREET",
                "Data i czas operacji: 2018-03-23 17:07:35","Oryginalna kwota operacji: 11,21 PLN",
                "Numer karty: 1234*****323","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2018, 3, 23),
                TransactionDate = new DateTime(2018, 3, 24),
                TransactionType = "Wpłata gotówki we wpłatomacie",
                Amount = 11.21M,
                Currency = "PLN",
                Title = "PKO BP 123123123123",
                Extensions = new LocationExtension
                {
                    Location = "Kraj: COUNTRY Miasto: CITY Adres: STREET"
                }.ToJson()
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //act
            var result = Sut.Import(new[] { "2018-03-23","2018-03-24","Other type","+11.21","PLN","+32.22",
                "Tytuł: PKO BP 123123123123",
                "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: STREET",
                "Data i czas operacji: 2018-03-23 17:07:35","Oryginalna kwota operacji: 11,21 PLN",
                "Numer karty: 1234*****323","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
