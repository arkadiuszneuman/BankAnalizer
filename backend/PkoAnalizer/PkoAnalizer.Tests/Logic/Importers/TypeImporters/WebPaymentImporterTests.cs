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
    public class WebPaymentImporterTests
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] {
                    new[] {
                        "2019-04-18","2019-04-17","Płatność web - kod mobilny",
                        "-10.19","PLN","+507.94","Numer telefonu: +48 568 457 587 ","Lokalizacja: Adres: www.skycash.com",
                        "Data i czas operacji: 2019-04-17 17:16:10","Numer referencyjny: 5458568544654","","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2019, 4, 18),
                        TransactionDate = new DateTime(2019, 4, 17),
                        TransactionType = "Płatność web - kod mobilny",
                        Amount = -10.19M,
                        Currency = "PLN",
                        Extensions = new PhoneNumberLocationExtension {
                            Location = "Adres: www.skycash.com",
                            PhoneNumber = "+48 568 457 587",
                        }.ToJson()
                    }
                };

                yield return new object[] {
                    new[] {
                        "2017-02-20","2017-02-19","Wypłata w bankomacie - kod mobilny",
                        "-21.32","PLN","+12.76","Numer telefonu: +48 422 532 432 ",
                        "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 38",
                        "Data i czas operacji: 2017-02-19 13:34:32",
                        "Bankomat: ASDAGAWD","Numer referencyjny: 00123102512031203","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2017, 2, 20),
                        TransactionDate = new DateTime(2017, 2, 19),
                        TransactionType = "Wypłata w bankomacie - kod mobilny",
                        Amount = -21.32M,
                        Currency = "PLN",
                        Extensions = new PhoneNumberLocationExtension {
                            Location = "Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 38",
                            PhoneNumber = "+48 422 532 432",
                        }.ToJson()
                    }
                };

                yield return new object[] {
                    new[] {
                        "2014-11-15","2014-11-14","Anulowanie wypłaty w bankomacie - kod mobilny",
                        "+12.32","PLN","+54.43","Numer telefonu: +48 435 542 543 ",
                        "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 45",
                        "Data i czas operacji: 2014-11-07 03:21:21","Bankomat: 123125412",
                        "Numer referencyjny: 5325346346345345364","",""

                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2014, 11, 15),
                        TransactionDate = new DateTime(2014, 11, 14),
                        TransactionType = "Anulowanie wypłaty w bankomacie - kod mobilny",
                        Amount = 12.32M,
                        Currency = "PLN",
                        Extensions = new PhoneNumberLocationExtension {
                            Location = "Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 45",
                            PhoneNumber = "+48 435 542 543",
                        }.ToJson()
                    }
                };

                yield return new object[] {
                    new[] {
                        "2018-12-06","2018-12-05","Zakup w terminalu - kod mobilny",
                        "-23.32","PLN","+1111.50","Numer telefonu: +48 423 425 423 ",
                        "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 45",
                        "Data i czas operacji: 2018-32-23 22:26:51",
                        "Numer referencyjny: 1423125124","","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2018, 12, 06),
                        TransactionDate = new DateTime(2018, 12, 05),
                        TransactionType = "Zakup w terminalu - kod mobilny",
                        Amount = -23.32M,
                        Currency = "PLN",
                        Extensions = new PhoneNumberLocationExtension {
                            Location = "Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 45",
                            PhoneNumber = "+48 423 425 423",
                        }.ToJson()
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_import_web_payment_transactions(string[] splittedLine, PkoTransaction expectedResult)
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<WebPaymentImporter>();

            //act
            var result = sut.Import(splittedLine);

            //assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<WebPaymentImporter>();

            //act
            var result = sut.Import(new[] { "2019-04-18","2019-04-17","Some other transaction",
                "-10.19","PLN","+507.94","Numer telefonu: +48 568 457 587 ","Lokalizacja: Adres: www.skycash.com",
                "Data i czas operacji: 2019-04-17 17:16:10","Numer referencyjny: 5458568544654","","","" });

            result.Should().BeNull();
        }
    }
}
