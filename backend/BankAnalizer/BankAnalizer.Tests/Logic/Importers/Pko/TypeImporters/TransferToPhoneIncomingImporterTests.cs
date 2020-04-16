using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class TransferToPhoneIncomingImporterTests : BaseUnitTest<TransferToPhoneIncomingImporter>
    {
        [Fact]
        public void Should_import_valid_transfer_to_phone_incoming_transactions()
        {
            //act
            var result = Sut.Import(new[] { "2019-03-26","2019-03-27","Przelew na telefon przychodz. zew.",
                "+11.73","PLN","+123.90","Rachunek nadawcy: 11 3333 6666 2222 1111 8888 6666",
                "Nazwa nadawcy: SNAME SSURNAME","Tytuł: PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME","","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new ImportedBankTransaction
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
            //act
            var result = Sut.Import(new[] { "2019-03-26","2019-03-27","Some other type",
                "+11.73","PLN","+123.90","Rachunek nadawcy: 11 3333 6666 2222 1111 8888 6666",
                "Nazwa nadawcy: SNAME SSURNAME","Tytuł: PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME","","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
