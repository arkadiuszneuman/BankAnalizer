using AutofacContrib.NSubstitute;
using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class WithdrawalFromAtmTests
    {
        [Fact]
        public void Should_import_withdrawal_from_atm_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<WithdrawalFromAtm>();

            //act
            var result = sut.Import(new[] { "2019-02-09", "2019-02-08", "Wypłata z bankomatu", "-321.32", "PLN", "+32.73",
                    "Tytuł: PKO BP 123123123", "Lokalizacja: Kraj: POLSKA Miasto: SOMECITY Adres: UL. SOMNEADDRESS 45",
                    "Data i czas operacji: 2019-02-08 11:34:51", "Oryginalna kwota operacji: 32,73 PLN", "Numer karty: 425125******4284", "", ""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 2, 9),
                TransactionDate = new DateTime(2019, 2, 8),
                TransactionType = "Wypłata z bankomatu",
                Amount = -321.32M,
                Currency = "PLN",
                Title = "Tytuł: PKO BP 123123123",
                Extensions = new LocationExtension
                {
                    Location = "Kraj: POLSKA Miasto: SOMECITY Adres: UL. SOMNEADDRESS 45"
                }.ToJson()
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<WithdrawalFromAtm>();

            //act
            var result = sut.Import(new[] { "2019-04-18","2019-04-17","Some other transaction",
                "-10.19","PLN","+507.94","Numer telefonu: +48 568 457 587 ","Lokalizacja: Adres: www.skycash.com",
                "Data i czas operacji: 2019-04-17 17:16:10","Numer referencyjny: 5458568544654","","","" });

            result.Should().BeNull();
        }
    }
}
