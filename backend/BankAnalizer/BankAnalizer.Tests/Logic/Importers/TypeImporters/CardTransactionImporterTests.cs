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
    public class CardTransactionImporterTests
    {
        [Fact]
        public void Should_import_valid_card_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<CardTransactionImporter>();

            //act
            var result = sut.Import(new[] { "2019-09-19", "2019-09-17", "Płatność kartą",
                "-13.00", "PLN", "+45.94", "Tytuł: Some transaction title",
                "Lokalizacja: Kraj: POLSKA Miasto: CHORZOW Adres: PIEKARNIA CUKIERNIA RUTA",
                "Data i czas operacji: 2019-02-05", "Oryginalna kwota operacji: 13,00 PLN",
                "Numer karty: 123456******7589", "", "" });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 9, 19),
                TransactionDate = new DateTime(2019, 9, 17),
                TransactionType = "Płatność kartą",
                Amount = -13.00M,
                Currency = "PLN",
                Title = "Some transaction title",
                Extensions = new LocationExtension
                {
                    Location = "Kraj: POLSKA Miasto: CHORZOW Adres: PIEKARNIA CUKIERNIA RUTA"
                }.ToJson()
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<CardTransactionImporter>();

            //act
            var result = sut.Import(new[] { "2019-09-19", "2019-09-17", "Some other type",
                "-13.00", "PLN", "+45.94", "Tytuł: Some transaction title",
                "Lokalizacja: Kraj: POLSKA Miasto: CHORZOW Adres: PIEKARNIA CUKIERNIA RUTA",
                "Data i czas operacji: 2019-02-05", "Oryginalna kwota operacji: 13,00 PLN",
                "Numer karty: 123456******7589", "", "" });

            //assert
            result.Should().BeNull();
        }
    }
}
