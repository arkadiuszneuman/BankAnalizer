using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class SenderNameImporterTests : BaseUnitTest<SenderNameImporter>
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
            //act
            var result = Sut.Import(splittedLine);

            //assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //act
            var result = Sut.Import(new[] { "2015-10-03","2016-01-01","SOME Other transaction","-0.03","PLN","+21.32","KAPIT.ODSETEK KARNYCH-OBCIĄŻENIE","","","","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}