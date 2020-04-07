using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Logic.Transactions.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Transactions.Import.Models;
using System;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class LoanPaymentImporterTests
    {
        [Fact]
        public void Should_import_valid_loan_payment_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<LoanPaymentImporter>();

            //act
            var result = sut.Import(new[] { "2019-02-15","2019-02-16","Spłata kredytu",
                "-123.48","PLN","+2342.21",
                "Tytuł: KAPITAŁ: 123,48 ODSETKI: 0,00 ODSETKI SKAPIT.: 0,00 ODSETKI KARNE: 0,00 321232123","","","","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 2, 15),
                TransactionDate = new DateTime(2019, 2, 16),
                TransactionType = "Spłata kredytu",
                Amount = -123.48M,
                Currency = "PLN",
                Title = "KAPITAŁ: 123,48 ODSETKI: 0,00 ODSETKI SKAPIT.: 0,00 ODSETKI KARNE: 0,00 321232123",
            });
        }

        [Fact]
        public void Should_not_import_different_type()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<LoanPaymentImporter>();

            //act
            var result = sut.Import(new[] {  "2019-02-15","2019-02-16","Other type",
                "-123.48","PLN","+2342.21",
                "Tytuł: KAPITAŁ: 123,48 ODSETKI: 0,00 ODSETKI SKAPIT.: 0,00 ODSETKI KARNE: 0,00 321232123","","","","","","" });

            //assert
            result.Should().BeNull();
        }
    }
}
