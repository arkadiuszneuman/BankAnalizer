using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Transactions.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Transactions.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Transactions.Import.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class SenderNameImporterTests
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] {
                    new[] {
                        "2019-04-13","2019-04-12","Wpłata gotówkowa w kasie","+234.32","PLN","+2564.00",
                        "Nazwa nadawcy: NAME SURNAME","Adres nadawcy: - -","Tytuł: WPŁATA WŁASNA","","","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2019, 04, 13),
                        TransactionDate = new DateTime(2019, 04, 12),
                        TransactionType = "Wpłata gotówkowa w kasie",
                        Amount = 234.32M,
                        Currency = "PLN",
                        Extensions = new SenderNameExtension {
                            SenderName = "NAME SURNAME"
                        }.ToJson(),
                        Title = "WPŁATA WŁASNA",
                    }
                };

            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_import_accruing_interest_transactions(string[] splittedLine, PkoTransaction expectedResult)
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<SenderNameImporter>();

            //act
            var result = sut.Import(splittedLine);

            //assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<SenderNameImporter>();

            //act
            var result = sut.Import(new[] { "2015-10-03","2016-01-01","SOME Other transaction","-0.03","PLN","+21.32","KAPIT.ODSETEK KARNYCH-OBCIĄŻENIE","","","","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}