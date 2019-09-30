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
    public class TransferToPhoneIncomingImporterTests
    {
        [Fact]
        public void Should_import_valid_card_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToPhoneIncomingImporter>();

            //act
            var result = sut.Import(new[] { "2019-03-26","2019-03-27","Przelew na telefon przychodz. zew.",
                "+11.73","PLN","+123.90","Rachunek nadawcy: 11 3333 6666 2222 1111 8888 6666",
                "Nazwa nadawcy: SNAME SSURNAME","Tytuł: PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME","","","",""
            });

            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 3, 26),
                TransactionDate = new DateTime(2019, 3, 27),
                TransactionType = "Przelew na telefon przychodz. zew.",
                Amount = 11.73M,
                Currency = "PLN",
                SenderReceipt = "Rachunek nadawcy: 11 3333 6666 2222 1111 8888 6666",
                SenderName = "Nazwa nadawcy: SNAME SSURNAME",
                Title = "Tytuł: PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToPhoneIncomingImporter>();

            //act
            var result = sut.Import(new[] { "2019-03-26","2019-03-27","Some other type",
                "+11.73","PLN","+123.90","Rachunek nadawcy: 11 3333 6666 2222 1111 8888 6666",
                "Nazwa nadawcy: SNAME SSURNAME","Tytuł: PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME","","","",""
            });

            result.Should().BeNull();
        }
    }
}
