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
    public class DirectDebitTypeImporterTests : BaseUnitTest<DirectDebitTypeImporter>
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

                yield return new object[] {
                    new[] {
                        "2016-12-23","2016-12-22","Przelew podatkowy","-231.32","PLN","+12.34",
                        "Rachunek odbiorcy: 44 5555 3333 2222 5555 4444 3333",
                        "Nazwa odbiorcy: PIERWSZY URZĄD SKARBOWY","Adres odbiorcy: OPOLE",
                        "Nazwa i nr identyfikatora: PESEL, 44345434456",
                        "Symbol formularza: MANDATY","Identyfikator zobowiązania: AB 21523432",
                        "Okres płatności: 0"

                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2016, 12, 23),
                        TransactionDate = new DateTime(2016, 12, 22),
                        TransactionType = "Przelew podatkowy",
                        Amount = -231.32M,
                        Currency = "PLN",
                        Extensions = new RecipientExtension {
                            RecipientReceipt = "44 5555 3333 2222 5555 4444 3333",
                            RecipientName = "PIERWSZY URZĄD SKARBOWY",
                            RecipientAddress = "OPOLE"
                        }.ToJson(),
                        Title = "Nazwa i nr identyfikatora: PESEL, 44345434456",
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_import_web_payment_transactions(string[] splittedLine, PkoTransaction expectedResult)
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
            var result = Sut.Import(new[] { "2019-03-23","2019-03-24","Other type","-21.32","PLN","+21.48",
                "Rachunek odbiorcy: 88 7777 2222 4444 0000 6666 9999","Nazwa odbiorcy: SOME RECIPIENT",
                "Adres odbiorcy: SOME RECIPIENT ADDRESS",
                "Tytuł: SOME TITLE","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
