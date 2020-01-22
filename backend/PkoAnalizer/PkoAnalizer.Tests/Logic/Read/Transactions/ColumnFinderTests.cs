using FluentAssertions;
using NSubstitute;
using PkoAnalizer.Logic.Read.Transactions;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
using System.Threading.Tasks;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Read.Transactions
{
    public class ColumnFinderTests : BaseUnitTest<ColumnFinder>
    {
        [Fact]
        public async Task Should_get_column_names()
        {
            //arrange
            var jsons = new[]
            {
                "{\"SenderReceipt\":\"Some other value\",\"SenderName\":\"Some field value, and other text","SenderAddress\":\"22-800 PEKIN\"}",
                "{\"AnotherColumn\":\"Some value\"}",
                "{\"AnotherColumn\":\"Some other\"}"
            };

            Mock<ITransactionReader>()
                .ReadAllExtensionColumns()
                .Returns(jsons);

            //act
            var result = await Sut.FindColumns();

            //assert
            result.Should().BeEquivalentTo(
                new ColumnViewModel
                {
                    Id = "Title",
                    Name = "Title"
                },
                new ColumnViewModel
                {
                    Id = "Extensions.SenderReceipt",
                    Name = "SenderReceipt"
                },
                new ColumnViewModel
                {
                    Id = "Extensions.SenderName",
                    Name = "SenderName"
                },
                new ColumnViewModel
                {
                    Id = "Extensions.SenderAddress",
                    Name = "SenderAddress"
                },
                new ColumnViewModel
                {
                    Id = "Extensions.AnotherColumn",
                    Name = "AnotherColumn"
                }
            );
        }
    }
}
