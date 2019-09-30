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
    public class StandingOrderImporterTests
    {
        [Fact]
        public void Should_import_valid_standing_order_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<StandingOrderImporter>();

            //act
            var result = sut.Import(new[] { "2019-03-01","2019-03-02","Zlecenie stałe","-73.21","PLN",
                "+321.07","Rachunek odbiorcy: 11 2222 3333 0000 9999 8888 7777",
                "Nazwa odbiorcy: SOME RECIPIENT NAME",
                "Adres odbiorcy: SOME ADDRESS 32/45 CITY",
                "Tytuł: NAME SURNAMESTREET 32/441-506 CHORZÓW","","",""
            });

            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 3, 1),
                TransactionDate = new DateTime(2019, 3, 2),
                TransactionType = "Zlecenie stałe",
                Amount = -73.21M,
                Currency = "PLN",
                RecipientReceipt = "Rachunek odbiorcy: 11 2222 3333 0000 9999 8888 7777",
                RecipientName = "Nazwa odbiorcy: SOME RECIPIENT NAME",
                RecipientAddress = "Adres odbiorcy: SOME ADDRESS 32/45 CITY",
                Title = "Tytuł: NAME SURNAMESTREET 32/441-506 CHORZÓW",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<StandingOrderImporter>();

            //act
            var result = sut.Import(new[] { "2019-03-01","2019-03-02","Some other type","-73.21","PLN",
                "+321.07","Rachunek odbiorcy: 11 2222 3333 0000 9999 8888 7777",
                "Nazwa odbiorcy: SOME RECIPIENT NAME",
                "Adres odbiorcy: SOME ADDRESS 32/45 CITY",
                "Tytuł: NAME SURNAMESTREET 32/441-506 CHORZÓW","","",""
            });

            result.Should().BeNull();
        }
    }
}
