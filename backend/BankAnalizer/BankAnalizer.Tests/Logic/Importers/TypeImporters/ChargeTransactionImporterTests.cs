using AutofacContrib.NSubstitute;
using BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class ChargeTransactionImporterTests
    {
        [Theory]
        [InlineData("Obciążenie")]
        [InlineData("Opłata")]
        public void Should_import_charge_transactions(string typeName)
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<ChargeTransactionImporter>();

            //act
            var result = sut.Import(new[] {
                "2019-01-11","2019-01-12",typeName,"-11.32","PLN","+21.83",
                "1231255123123151231235123123","","","","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 1, 11),
                TransactionDate = new DateTime(2019, 1, 12),
                TransactionType = typeName,
                Amount = -11.32M,
                Currency = "PLN",
                Title = "1231255123123151231235123123",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<ChargeTransactionImporter>();

            //act
            var result = sut.Import(new[] { "2019-01-11","2019-01-12","Some other transaction","-11.32","PLN","+21.83",
                "1231255123123151231235123123","","","","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
