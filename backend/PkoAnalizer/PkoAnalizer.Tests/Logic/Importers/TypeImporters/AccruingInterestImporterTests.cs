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
    public class AccruingInterestImporterTests
    {
        [Fact]
        public void Should_import_accruing_interest_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<AccruingInterestImporter>();

            //act
            var result = sut.Import(new[] { "2015-10-03","2016-01-01","Naliczenie odsetek","-0.03","PLN","+21.32","KAPIT.ODSETEK KARNYCH-OBCIĄŻENIE","","","","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2015, 10, 3),
                TransactionDate = new DateTime(2016, 1, 1),
                TransactionType = "Naliczenie odsetek",
                Amount = -0.03M,
                Currency = "PLN",
                Title = "KAPIT.ODSETEK KARNYCH-OBCIĄŻENIE",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<WebPaymentImporter>();

            //act
            var result = sut.Import(new[] { "2015-10-03","2016-01-01","SOME Other transaction","-0.03","PLN","+21.32","KAPIT.ODSETEK KARNYCH-OBCIĄŻENIE","","","","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
