using AutofacContrib.NSubstitute;
using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class TransferToPhoneIncomingImporterTests
    {
        [Fact]
        public void Should_import_valid_transfer_to_phone_incoming_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToPhoneIncomingImporter>();

            //act
            var result = sut.Import(new[] { "2019-03-26","2019-03-27","Przelew na telefon przychodz. zew.",
                "+11.73","PLN","+123.90","Rachunek nadawcy: 11 3333 6666 2222 1111 8888 6666",
                "Nazwa nadawcy: SNAME SSURNAME","Tytuł: PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME","","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 3, 26),
                TransactionDate = new DateTime(2019, 3, 27),
                TransactionType = "Przelew na telefon przychodz. zew.",
                Amount = 11.73M,
                Currency = "PLN",
                Extensions = new SenderReceiptNameExtension
                {
                    SenderReceipt = "11 3333 6666 2222 1111 8888 6666",
                    SenderName = "SNAME SSURNAME",
                }.ToJson(),
                Title = "PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME",
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

            //assert
            result.Should().BeNull();
        }
    }
}
