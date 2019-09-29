using FluentAssertions;
using PkoAnalizer.Logic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Common
{
    public class FileReaderTests
    {
        [Fact]
        public void Should_load_file_in_valid_encoding()
        {
            //arrange
            var sut = new FileReader();

            //act
            var result = sut.ReadLines("csv_test.csv", Encoding.GetEncoding(1250)).ToList();

            //assert
            result.Should().BeEquivalentTo("\"2015-04-13\",\"2015-04-13\",\"Wpłata gotówkowa w kasie\",\"+2222.00\",\"PLN\",\"+3333.00\",\"Nazwa nadawcy: SOME NAME\",\"Adres nadawcy: - -\",\"Tytuł: WPŁATA WŁASNA\",\"\",\"\",\"\",\"\"");
        }
    }
}
