using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class InstantTransferToAccountImporterTests
    {
        [Fact]
        public void Should_import_instant_transfer_to_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<InstantTransferToAccountImporter>();

            //act
            var result = sut.Import(new[] {
                "2019-03-23","2019-03-24","Przelew Natychmiastowy na rachunek","+123.12","PLN","+32.06",
                "Rachunek nadawcy: 44 2222 5555 1111 7777 4444 3333","Nazwa nadawcy: SOME RECIPIENT",
                "Tytuł: PRZELEW ŚRODKÓW","","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 3, 23),
                TransactionDate = new DateTime(2019, 3, 24),
                TransactionType = "Przelew Natychmiastowy na rachunek",
                Amount = 123.12M,
                Currency = "PLN",
                 Extensions = new RecipientReceiptNameExtension {
                     RecipientReceipt = "Rachunek nadawcy: 44 2222 5555 1111 7777 4444 3333",
                     RecipientName = "Nazwa nadawcy: SOME RECIPIENT"
                 }.ToJson(),
                Title = "Tytuł: PRZELEW ŚRODKÓW",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<InstantTransferToAccountImporter>();

            //act
            var result = sut.Import(new[] { "2019-03-23","2019-03-24","Other type","+9.72","PLN","+22.54","Tytuł: 5425465 55487541",
                "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: ADDRESS",
                "Data i czas operacji: 2019-03-24","Oryginalna kwota operacji: 2,75 USD",
                "Data przetworzenia: 2019-03-24","Numer karty: 2156452*****458",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
