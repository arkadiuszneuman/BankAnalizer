﻿using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class TransferFromAccountImporterTests : BaseUnitTest<TransferFromAccountImporter>
    {
        [Fact]
        public void Should_import_valid_transfer_from_account_transactions()
        {
            //act
            var result = Sut.Import(new[] { "2019-03-06","2019-03-07","Przelew z rachunku",
                "-43.47","PLN","+123.04","Rachunek odbiorcy: 99 8888 5555 2222 0001 7777 4444",
                "Nazwa odbiorcy: RECIPIENT NAME","Adres odbiorcy: SOME RECIPIENT ADDRESS 32/43 54-322 CITY",
                "Tytuł: SOME TITLE","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new ImportedBankTransaction
            {
                OperationDate = new DateTime(2019, 3, 6),
                TransactionDate = new DateTime(2019, 3, 7),
                TransactionType = "Przelew z rachunku",
                Amount = -43.47M,
                Currency = "PLN",
                Extensions = new RecipientExtension
                {
                    RecipientReceipt = "99 8888 5555 2222 0001 7777 4444",
                    RecipientName = "RECIPIENT NAME",
                    RecipientAddress = "SOME RECIPIENT ADDRESS 32/43 54-322 CITY",
                }.ToJson(),
                Title = "SOME TITLE",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //act
            var result = Sut.Import(new[] { "2019-03-06","2019-03-07","Some other type",
                "-43.47","PLN","+123.04","Rachunek odbiorcy: 99 8888 5555 2222 0001 7777 4444",
                "Nazwa odbiorcy: RECIPIENT NAME","Adres odbiorcy: SOME RECIPIENT ADDRESS 32/43 54-322 CITY",
                "Tytuł: SOME TITLE","","",""
            });

            //assert
            result.Should().BeNull();
        }
    }
}