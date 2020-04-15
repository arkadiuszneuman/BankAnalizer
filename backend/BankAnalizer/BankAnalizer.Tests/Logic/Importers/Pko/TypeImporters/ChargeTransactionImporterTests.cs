using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class ChargeTransactionImporterTests : BaseUnitTest<ChargeTransactionImporter>
    {
        [Theory]
        [InlineData("Obciążenie")]
        [InlineData("Opłata")]
        public void Should_import_charge_transactions(string typeName)
        {
            //act
            var result = Sut.Import(new[] {
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
            //act
            var result = Sut.Import(new[] { "2019-01-11","2019-01-12","Some other transaction","-11.32","PLN","+21.83",
                "1231255123123151231235123123","","","","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
