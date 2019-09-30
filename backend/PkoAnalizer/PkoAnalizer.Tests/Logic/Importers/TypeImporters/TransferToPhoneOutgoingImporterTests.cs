using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Logic.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class TransferToPhoneOutgoingImporterTests
    {
        [Fact]
        public void Should_import_valid_card_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToPhoneOutgoingImporter>();

            //act
            var result = sut.Import(new[] {"2019-03-25","2019-03-26","Przelew na telefon wychodzący zew.",
                "-32.12","PLN","+321.87","Rachunek odbiorcy: 44 5555 7777 1111 3333 6666 7777",
                "Nazwa odbiorcy: NAME SURNAME","Tytuł: PRZELEW NA TELEFON OD: 48930293023 DO: 48950495023","","","",""
            });

            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 3, 25),
                TransactionDate = new DateTime(2019, 3, 26),
                TransactionType = "Przelew na telefon wychodzący zew.",
                Amount = -32.12M,
                Currency = "PLN",
                RecipientReceipt = "Rachunek odbiorcy: 44 5555 7777 1111 3333 6666 7777",
                RecipientName = "Nazwa odbiorcy: NAME SURNAME",
                Title = "Tytuł: PRZELEW NA TELEFON OD: 48930293023 DO: 48950495023",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToPhoneOutgoingImporter>();

            //act
            var result = sut.Import(new[] {"2019-03-25","2019-03-26","Other type",
                "-32.12","PLN","+321.87","Rachunek odbiorcy: 44 5555 7777 1111 3333 6666 7777",
                "Nazwa odbiorcy: NAME SURNAME","Tytuł: PRZELEW NA TELEFON OD: 48930293023 DO: 48950495023","","","",""
            });

            result.Should().BeNull();
        }
    }
}
