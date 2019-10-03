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
    public class TransferFromAccountImporterTests
    {
        [Fact]
        public void Should_import_valid_transfer_from_account_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferFromAccountImporter>();

            //act
            var result = sut.Import(new[] { "2019-03-06","2019-03-07","Przelew z rachunku",
                "-43.47","PLN","+123.04","Rachunek odbiorcy: 99 8888 5555 2222 0001 7777 4444",
                "Nazwa odbiorcy: RECIPIENT NAME","Adres odbiorcy: SOME RECIPIENT ADDRESS 32/43 54-322 CITY",
                "Tytuł: SOME TITLE","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 3, 6),
                TransactionDate = new DateTime(2019, 3, 7),
                TransactionType = "Przelew z rachunku",
                Amount = -43.47M,
                Currency = "PLN",
                Extensions = new RecipientExtension {
                    RecipientReceipt = "Rachunek odbiorcy: 99 8888 5555 2222 0001 7777 4444",
                    RecipientName = "Nazwa odbiorcy: RECIPIENT NAME",
                    RecipientAddress = "Adres odbiorcy: SOME RECIPIENT ADDRESS 32/43 54-322 CITY",
                }.ToJson(),
                Title = "Tytuł: SOME TITLE",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferFromAccountImporter>();

            //act
            var result = sut.Import(new[] { "2019-03-06","2019-03-07","Some other type",
                "-43.47","PLN","+123.04","Rachunek odbiorcy: 99 8888 5555 2222 0001 7777 4444",
                "Nazwa odbiorcy: RECIPIENT NAME","Adres odbiorcy: SOME RECIPIENT ADDRESS 32/43 54-322 CITY",
                "Tytuł: SOME TITLE","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
