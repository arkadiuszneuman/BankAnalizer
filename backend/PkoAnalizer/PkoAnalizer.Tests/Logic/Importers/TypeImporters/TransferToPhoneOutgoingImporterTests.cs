using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class TransferToPhoneOutgoingImporterTests
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] {
                    new[] {
                        "2019-03-25","2019-03-26","Przelew na telefon wychodzący zew.",
                        "-32.12","PLN","+321.87","Rachunek odbiorcy: 44 5555 7777 1111 3333 6666 7777",
                        "Nazwa odbiorcy: NAME SURNAME","Tytuł: PRZELEW NA TELEFON OD: 48930293023 DO: 48950495023","","","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 25),
                        TransactionDate = new DateTime(2019, 3, 26),
                        TransactionType = "Przelew na telefon wychodzący zew.",
                        Amount = -32.12M,
                        Currency = "PLN",
                        Extensions = new RecipientReceiptNameExtension {
                            RecipientReceipt = "44 5555 7777 1111 3333 6666 7777",
                            RecipientName = "NAME SURNAME",
                        }.ToJson(),
                        Title = "PRZELEW NA TELEFON OD: 48930293023 DO: 48950495023",
                    }
                };

                yield return new object[] {
                    new[] {
                        "2018-02-12","2018-02-11","Przelew na telefon wychodzący wew.",
                        "-123.42","PLN","+12.32","Rachunek odbiorcy: 33 4444 5555 7777 8888 9999 3333",
                        "Nazwa odbiorcy: ODBIORCA PRZELEWU NA TELEFON",
                        "Tytuł: PIŁKA OD: 48384959485 DO: 48594859483","Referencje własne zleceniodawcy: 123123123","","",""

                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2018, 2, 12),
                        TransactionDate = new DateTime(2018, 2, 11),
                        TransactionType = "Przelew na telefon wychodzący wew.",
                        Amount = -123.42M,
                        Currency = "PLN",
                        Extensions = new RecipientReceiptNameExtension {
                            RecipientReceipt = "33 4444 5555 7777 8888 9999 3333",
                            RecipientName = "ODBIORCA PRZELEWU NA TELEFON"
                        }.ToJson(),
                        Title = "PIŁKA OD: 48384959485 DO: 48594859483",
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_import_web_payment_transactions(string[] splittedLine, PkoTransaction expectedResult)
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToPhoneOutgoingImporter>();

            //act
            var result = sut.Import(splittedLine);

            //assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToPhoneOutgoingImporter>();

            //act
            var result = sut.Import(new[] {"2019-03-25","2019-03-26","Other type",
                "-32.12","PLN","+321.87","Rachunek odbiorcy: 44 5555 7777 1111 3333 6666 7777",
                "Nazwa odbiorcy: NAME SURNAME","Tytuł: PRZELEW NA TELEFON OD: 48930293023 DO: 48950495023","","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
