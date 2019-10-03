using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;
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
