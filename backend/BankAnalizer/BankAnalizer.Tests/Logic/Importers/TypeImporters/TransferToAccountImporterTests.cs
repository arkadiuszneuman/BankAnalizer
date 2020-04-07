using AutofacContrib.NSubstitute;
using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class TransferToAccountImporterTests
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] {
                    new[] {
                        "2019-09-05","2019-09-06","Przelew na rachunek",
                        "+213.32","PLN","+123.52","Rachunek nadawcy: 33 4444 0001 4444 5555 6666 9999",
                        "Nazwa nadawcy: SENDER NAME","Adres nadawcy: Street 4A/23 43-532 City",
                        "Tytuł: Some title","","",""
                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2019, 9, 5),
                        TransactionDate = new DateTime(2019, 9, 6),
                        TransactionType = "Przelew na rachunek",
                        Amount = 213.32M,
                        Currency = "PLN",
                        Extensions = new SenderExtension {
                            SenderReceipt = "33 4444 0001 4444 5555 6666 9999",
                            SenderName = "SENDER NAME",
                            SenderAddress = "Street 4A/23 43-532 City",
                        }.ToJson(),
                        Title = "Some title",
                    }
                };

                yield return new object[] {
                    new[] {
                        "2019-03-17","2019-03-16","Przelew na telefon przychodz. wew.","+12.32","PLN","+123.42","Rachunek nadawcy: 33 4444 2222 4444 2222 1111 2088","Nazwa nadawcy: NAME SURNAME","Adres nadawcy: SOME ADDRESS 445/34","Tytuł: PRZELEW NA TELEFON OD: 48394504932 DO: 48745493943","Referencje własne zleceniodawcy: 1231241123","",""

                    },
                    new PkoTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 17),
                        TransactionDate = new DateTime(2019, 3, 16),
                        TransactionType = "Przelew na telefon przychodz. wew.",
                        Amount = 12.32M,
                        Currency = "PLN",
                        Extensions = new SenderExtension {
                            SenderReceipt = "33 4444 2222 4444 2222 1111 2088",
                            SenderName = "NAME SURNAME",
                            SenderAddress = "SOME ADDRESS 445/34",
                        }.ToJson(),
                        Title = "PRZELEW NA TELEFON OD: 48394504932 DO: 48745493943",
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_import_valid_transfer_to_account_transactions(string[] splittedLine, PkoTransaction expectedResult)
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToAccountImporter>();

            //act
            var result = sut.Import(splittedLine);

            //assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<TransferToAccountImporter>();

            //act
            var result = sut.Import(new[] { "2019-09-05","2019-09-06","Some other type",
                "+213.32","PLN","+123.52","Rachunek nadawcy: 33 4444 0001 4444 5555 6666 9999",
                "Nazwa nadawcy: SENDER NAME","Adres nadawcy: Street 4A/23 43-532 City",
                "Tytuł: Some title","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}
