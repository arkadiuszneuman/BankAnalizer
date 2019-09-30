using AutofacContrib.NSubstitute;
using FluentAssertions;
using PkoAnalizer.Logic.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Import.Models;
using System;
using Xunit;

namespace PkoAnalizer.Tests.Logic.Importers.TypeImporters
{
    public class WebPaymentImporterTests
    {
        [Fact]
        public void Should_import_web_payment_transactions()
        {
            //arrange
            var sut = new AutoSubstitute().Resolve<WebPaymentImporter>();

            //act
            var result = sut.Import(new[] { "2019-04-18","2019-04-17","Płatność web - kod mobilny",
                "-10.19","PLN","+507.94","Numer telefonu: +48 568 457 587 ","Lokalizacja: Adres: www.skycash.com",
                "Data i czas operacji: 2019-04-17 17:16:10","Numer referencyjny: 5458568544654","","",""
            });

            //assert
            result.Should().BeEquivalentTo(new PkoTransaction
            {
                OperationDate = new DateTime(2019, 4, 18),
                TransactionDate = new DateTime(2019, 4, 17),
                TransactionType = "Płatność web - kod mobilny",
                Amount = -10.19M,
                Currency = "PLN",
                Location = "Lokalizacja: Adres: www.skycash.com",
                PhoneNumber = "Numer telefonu: +48 568 457 587 ",
            });
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
