﻿using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Logic.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class AccruingInterestImporterTests
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] { 
                    new[] { 
                        "2015-10-03","2016-01-01","Naliczenie odsetek",
                        "-0.03","PLN","+21.32","KAPIT.ODSETEK KARNYCH-OBCIĄŻENIE","","","","","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2015, 10, 3),
                        TransactionDate = new DateTime(2016, 1, 1),
                        TransactionType = "Naliczenie odsetek",
                        Amount = -0.03M,
                        Currency = "PLN",
                        Title = "KAPIT.ODSETEK KARNYCH-OBCIĄŻENIE",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2013-07-07", "2013-07-06", "Opłata za użytkowanie karty", "-0.56", "PLN", "+123.32", "Tytuł: OPŁATA PROP. ZA KARTĘ 123123123*****323, 19.06-05.07", "", "", "", "", "", ""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2013, 7, 7),
                        TransactionDate = new DateTime(2013, 7, 6),
                        TransactionType = "Opłata za użytkowanie karty",
                        Amount = -0.56M,
                        Currency = "PLN",
                        Title = "OPŁATA PROP. ZA KARTĘ 123123123*****323, 19.06-05.07",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2018-02-22","2018-02-20","Prowizja","-32.21","PLN","+412.34",
                        "Tytuł: 32523523324234234","","","","","",""

                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2018, 2, 22),
                        TransactionDate = new DateTime(2018, 2, 20),
                        TransactionType = "Prowizja",
                        Amount = -32.21M,
                        Currency = "PLN",
                        Title = "32523523324234234",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2013-11-12","2013-11-10","Opłata składki ubezpieczeniowej",
                        "-412.32","PLN","+123.66","465345346ISAFIGASI4365345","","","","","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2013, 11, 12),
                        TransactionDate = new DateTime(2013, 11, 10),
                        TransactionType = "Opłata składki ubezpieczeniowej",
                        Amount = -412.32M,
                        Currency = "PLN",
                        Title = "465345346ISAFIGASI4365345",
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_import_accruing_interest_transactions(string[] splittedLine, PkoTransaction expectedResult)
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<AccruingInterestImporter>();

            //act
            var result = sut.Import(splittedLine);

            //assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<AccruingInterestImporter>();

            //act
            var result = sut.Import(new[] { "2015-10-03","2016-01-01","SOME Other transaction","-0.03","PLN","+21.32","KAPIT.ODSETEK KARNYCH-OBCIĄŻENIE","","","","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
