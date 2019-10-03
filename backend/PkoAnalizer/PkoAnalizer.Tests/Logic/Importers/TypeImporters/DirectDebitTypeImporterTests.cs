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
    public class DirectDebitTypeImporterTests
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] {
                    new[] {
                        "2019-03-23","2019-03-24","Polecenie Zapłaty","-21.32","PLN","+21.48",
                        "Rachunek odbiorcy: 88 7777 2222 4444 0000 6666 9999","Nazwa odbiorcy: SOME RECIPIENT",
                        "Adres odbiorcy: SOME RECIPIENT ADDRESS",
                        "Tytuł: SOME TITLE","","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 23),
                        TransactionDate = new DateTime(2019, 3, 24),
                        TransactionType = "Polecenie Zapłaty",
                        Amount = -21.32M,
                        Currency = "PLN",
                        Extensions = new RecipientExtension {
                            RecipientReceipt = "88 7777 2222 4444 0000 6666 9999",
                            RecipientName = "SOME RECIPIENT",
                            RecipientAddress = "SOME RECIPIENT ADDRESS"
                        }.ToJson(),
                        Title = "SOME TITLE",
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_import_web_payment_transactions(string[] splittedLine, PkoTransaction expectedResult)
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<DirectDebitTypeImporter>();

            //act
            var result = sut.Import(splittedLine);

            //assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<DirectDebitTypeImporter>();

            //act
            var result = sut.Import(new[] { "2019-03-23","2019-03-24","Other type","-21.32","PLN","+21.48",
                "Rachunek odbiorcy: 88 7777 2222 4444 0000 6666 9999","Nazwa odbiorcy: SOME RECIPIENT",
                "Adres odbiorcy: SOME RECIPIENT ADDRESS",
                "Tytuł: SOME TITLE","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
