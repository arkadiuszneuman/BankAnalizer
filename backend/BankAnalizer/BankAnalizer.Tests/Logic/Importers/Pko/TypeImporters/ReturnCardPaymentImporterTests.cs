using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class ReturnCardPaymentImporterTests : BaseUnitTest<ReturnCardPaymentImporter>
    {
        [Fact]
        public void Should_import_return_card_payment_transactions()
        {
            //act
            var result = Sut.Import(new[] {
                "2019-03-23","2019-03-24","Zwrot płatności kartą","+9.72","PLN","+22.54","Tytuł: 5425465 55487541",
                "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: ADDRESS",
                "Data i czas operacji: 2019-03-24","Oryginalna kwota operacji: 2,75 USD",
                "Data przetworzenia: 2019-03-24","Numer karty: 2156452*****458",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 3, 23),
                TransactionDate = new DateTime(2019, 3, 24),
                TransactionType = "Zwrot płatności kartą",
                Amount = 9.72M,
                Currency = "PLN",
                Title = "Tytuł: 5425465 55487541",
                Extensions = new RecipientAddresExtension()
                {
                    RecipientAddress = "Kraj: COUNTRY Miasto: CITY Adres: ADDRESS"
                }.ToJson()
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
