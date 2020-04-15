using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class AccruingInterestImporterTests : BaseUnitTest<AccruingInterestImporter>
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
                yield return new object[] {
                    new[] {
                        "2018-12-05","2018-12-04","Uznanie","+123.28","PLN","+42.47",
                        "45345347345346435346","","","","","",""

                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2018, 12, 5),
                        TransactionDate = new DateTime(2018, 12, 4),
                        TransactionType = "Uznanie",
                        Amount = 123.28M,
                        Currency = "PLN",
                        Title = "45345347345346435346",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2018-04-30","2018-05-01","Podatek od odsetek","-0.01","PLN",
                        "+4345.54","PODATEK OD ODSETEK KAPITAŁOWYCH","","","","","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2018, 4, 30),
                        TransactionDate = new DateTime(2018, 5, 1),
                        TransactionType = "Podatek od odsetek",
                        Amount = -0.01M,
                        Currency = "PLN",
                        Title = "PODATEK OD ODSETEK KAPITAŁOWYCH",
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
