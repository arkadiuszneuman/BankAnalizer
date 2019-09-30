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
    public class TransferToAccountImporterTests
    {
        [Fact]
        public void Should_import_valid_transfer_to_account_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToAccountImporter>();

            //act
            var result = sut.Import(new[] { "2019-09-05","2019-09-06","Przelew na rachunek",
                "+213.32","PLN","+123.52","Rachunek nadawcy: 33 4444 0001 4444 5555 6666 9999",
                "Nazwa nadawcy: SENDER NAME","Adres nadawcy: Street 4A/23 43-532 City",
                "Tytuł: Some title","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 9, 5),
                TransactionDate = new DateTime(2019, 9, 6),
                TransactionType = "Przelew na rachunek",
                Amount = 213.32M,
                Currency = "PLN",
                SenderReceipt = "Rachunek nadawcy: 33 4444 0001 4444 5555 6666 9999",
                SenderName = "Nazwa nadawcy: SENDER NAME",
                SenderAddress = "Adres nadawcy: Street 4A/23 43-532 City",
                Title = "Tytuł: Some title",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToAccountImporter>();

            //act
            var result = sut.Import(new[] { "2019-09-05","2019-09-06","Some other type",
                "+213.32","PLN","+123.52","Rachunek nadawcy: 33 4444 0001 4444 5555 6666 9999",
                "Nazwa nadawcy: SENDER NAME","Adres nadawcy: Street 4A/23 43-532 City",
                "Tytuł: Some title","","",""
            });
            
            //assert
            result.Should().BeNull();
        }
    }
}
