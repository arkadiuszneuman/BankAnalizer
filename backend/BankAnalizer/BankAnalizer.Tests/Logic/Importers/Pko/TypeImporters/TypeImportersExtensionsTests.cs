using BankAnalizer.Logic.Transactions.Import.Importers;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class TypeImportersExtensionsTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_throw_when_index_is_bigger_or_same_than_length(int index)
        {
            //arrange
            var lines = new[] { "a", "b" };

            //act
            Action action = () => lines.Index(index);

            //assert
            action.Should().ThrowExactly<ImportException>();
        }

        [Fact]
        public void Should_get_when_index_is_in_range()
        {
            //arrange
            var lines = new[] { "a", "b" };

            //act
            var result = lines.Index(1);

            //assert
            result.Should().Be("b");
        }
    }
}
