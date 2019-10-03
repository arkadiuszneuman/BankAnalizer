//using AutofacContrib.NSubstitute;
//using FluentAssertions;
//using NSubstitute;
//using PkoAnalizer.Logic.Common;
//using PkoAnalizer.Logic.Import.Importers;
//using PkoAnalizer.Logic.Import.Importers.TypeImporters;
//using PkoAnalizer.Logic.Import.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Xunit;

//namespace PkoAnalizer.Tests.Logic.Importers
//{
//    public class CsvImporterTests
//    {
//        [Fact]
//        public void Should_import_lines_except_last_one()
//        {
//            //arrange
//            var sut = new Fixture()
//                .WithLine(@"""Data operacji"",""Data waluty"",""Typ transakcji"",""Kwota"",""Waluta"",""Saldo po transakcji"",""Opis transakcji"","""","""","""","""","""",""""")
//                .WithLine(@"""2019-09-19"",""2019-09-17"",""NOT EXISTING TYPE"",""-13.00"",""PLN"",""+45.94"",""Tytuł: Some transaction title"",""Lokalizacja: Kraj: POLSKA Miasto: CHORZOW Adres: PIEKARNIA CUKIERNIA RUTA"",""Data i czas operacji: 2019-02-05"", ""Oryginalna kwota operacji: 13,00 PLN"",""Numer karty: 123456******7589"","""", """"")
//                .WithLine(@"""2019-09-19"",""2019-09-17"",""Płatność kartą"",""-13.00"",""PLN"",""+45.94"",""Tytuł: Some transaction title"",""Lokalizacja: Kraj: POLSKA Miasto: CHORZOW Adres: PIEKARNIA CUKIERNIA RUTA"",""Data i czas operacji: 2019-02-05"", ""Oryginalna kwota operacji: 13,00 PLN"",""Numer karty: 123456******7589"","""", """"")
//                .WithLine(@"""2019-02-15"",""2019-02-16"",""Spłata kredytu"",""-123.48"",""PLN"",""+2342.21"",""Tytuł: KAPITAŁ: 123,48 ODSETKI: 0,00 ODSETKI SKAPIT.: 0,00 ODSETKI KARNE: 0,00 321232123"","""","""","""","""","""",""""")
//                .WithLine(@"""2019-03-01"",""2019-03-02"",""Zlecenie stałe"",""-73.21"",""PLN"",""+321.07"",""Rachunek odbiorcy: 11 2222 3333 0000 9999 8888 7777"",""Nazwa odbiorcy: SOME RECIPIENT NAME"",""Adres odbiorcy: SOME ADDRESS 32/45 CITY"",""Tytuł: NAME SURNAMESTREET 32/441-506 CHORZÓW"","""","""",""""")
//                .WithLine(@"""2019-03-06"",""2019-03-07"",""Przelew z rachunku"",""-43.47"",""PLN"",""+123.04"",""Rachunek odbiorcy: 99 8888 5555 2222 0001 7777 4444"",""Nazwa odbiorcy: RECIPIENT NAME"",""Adres odbiorcy: SOME RECIPIENT ADDRESS 32/43 54-322 CITY"",""Tytuł: SOME TITLE"","""","""",""""")
//                .WithLine(@" ""2019-09-05"",""2019-09-06"",""Przelew na rachunek"",""+213.32"",""PLN"",""+123.52"",""Rachunek nadawcy: 33 4444 0001 4444 5555 6666 9999"",""Nazwa nadawcy: SENDER NAME"",""Adres nadawcy: Street 4A/23 43-532 City"",""Tytuł: Some title"","""","""",""""")
//                .WithLine(@"""2019-03-26"",""2019-03-27"",""Przelew na telefon przychodz. zew."",""+11.73"",""PLN"",""+123.90"",""Rachunek nadawcy: 11 3333 6666 2222 1111 8888 6666"",""Nazwa nadawcy: SNAME SSURNAME"",""Tytuł: PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME"","""","""","""",""""")
//                .WithLine(@"""2019-03-25"",""2019-03-26"",""Przelew na telefon wychodzący zew."",""-32.12"",""PLN"",""+321.87"",""Rachunek odbiorcy: 44 5555 7777 1111 3333 6666 7777"",""Nazwa odbiorcy: NAME SURNAME"",""Tytuł: PRZELEW NA TELEFON OD: 48930293023 DO: 48950495023"","""","""","""",""""")
//                .WithLine(@"""2019-04-18"",""2019-04-17"",""Płatność web - kod mobilny"",""-10.19"",""PLN"",""+507.94"",""Numer telefonu: +48 568 457 587 "",""Lokalizacja: Adres: www.skycash.com"",""Data i czas operacji: 2019-04-17 17:16:10"",""Numer referencyjny: 5458568544654"","""","""",""""")
//                .Configure();

//            //act
//            var result = sut.ImportTransactions().ToList();

//            //assert
//            result.Should().BeEquivalentTo(new List<PkoTransaction> {
//                new PkoTransaction
//                {
//                    OperationDate = new DateTime(2019, 4, 18),
//                    TransactionDate = new DateTime(2019, 4, 17),
//                    TransactionType = "Płatność web - kod mobilny",
//                    Amount = -10.19M,
//                    Currency = "PLN",
//                    Location = "Lokalizacja: Adres: www.skycash.com",
//                    PhoneNumber = "Numer telefonu: +48 568 457 587",
//                },
//                new PkoTransaction
//                {
//                    OperationDate = new DateTime(2019, 3, 25),
//                    TransactionDate = new DateTime(2019, 3, 26),
//                    TransactionType = "Przelew na telefon wychodzący zew.",
//                    Amount = -32.12M,
//                    Currency = "PLN",
//                    RecipientReceipt = "Rachunek odbiorcy: 44 5555 7777 1111 3333 6666 7777",
//                    RecipientName = "Nazwa odbiorcy: NAME SURNAME",
//                    Title = "Tytuł: PRZELEW NA TELEFON OD: 48930293023 DO: 48950495023",
//                },
//                new PkoTransaction
//                {
//                    OperationDate = new DateTime(2019, 3, 26),
//                    TransactionDate = new DateTime(2019, 3, 27),
//                    TransactionType = "Przelew na telefon przychodz. zew.",
//                    Amount = 11.73M,
//                    Currency = "PLN",
//                    SenderReceipt = "Rachunek nadawcy: 11 3333 6666 2222 1111 8888 6666",
//                    SenderName = "Nazwa nadawcy: SNAME SSURNAME",
//                    Title = "Tytuł: PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME",
//                },
//                new PkoTransaction
//                {
//                    OperationDate = new DateTime(2019, 9, 5),
//                    TransactionDate = new DateTime(2019, 9, 6),
//                    TransactionType = "Przelew na rachunek",
//                    Amount = 213.32M,
//                    Currency = "PLN",
//                    SenderReceipt = "Rachunek nadawcy: 33 4444 0001 4444 5555 6666 9999",
//                    SenderName = "Nazwa nadawcy: SENDER NAME",
//                    SenderAddress = "Adres nadawcy: Street 4A/23 43-532 City",
//                    Title = "Tytuł: Some title",
//                },
//                new PkoTransaction
//                {
//                    OperationDate = new DateTime(2019, 3, 6),
//                    TransactionDate = new DateTime(2019, 3, 7),
//                    TransactionType = "Przelew z rachunku",
//                    Amount = -43.47M,
//                    Currency = "PLN",
//                    RecipientReceipt = "Rachunek odbiorcy: 99 8888 5555 2222 0001 7777 4444",
//                    RecipientName = "Nazwa odbiorcy: RECIPIENT NAME",
//                    RecipientAddress = "Adres odbiorcy: SOME RECIPIENT ADDRESS 32/43 54-322 CITY",
//                    Title = "Tytuł: SOME TITLE",
//                },
//                new PkoTransaction
//                {
//                    OperationDate = new DateTime(2019, 3, 1),
//                    TransactionDate = new DateTime(2019, 3, 2),
//                    TransactionType = "Zlecenie stałe",
//                    Amount = -73.21M,
//                    Currency = "PLN",
//                    RecipientReceipt = "Rachunek odbiorcy: 11 2222 3333 0000 9999 8888 7777",
//                    RecipientName = "Nazwa odbiorcy: SOME RECIPIENT NAME",
//                    RecipientAddress = "Adres odbiorcy: SOME ADDRESS 32/45 CITY",
//                    Title = "Tytuł: NAME SURNAMESTREET 32/441-506 CHORZÓW",
//                },
//                new PkoTransaction
//                {
//                    OperationDate = new DateTime(2019, 2, 15),
//                    TransactionDate = new DateTime(2019, 2, 16),
//                    TransactionType = "Spłata kredytu",
//                    Amount = -123.48M,
//                    Currency = "PLN",
//                    Title = "Tytuł: KAPITAŁ: 123,48 ODSETKI: 0,00 ODSETKI SKAPIT.: 0,00 ODSETKI KARNE: 0,00 321232123",
//                },
//                new PkoTransaction
//                {
//                    OperationDate = new DateTime(2019, 9, 19),
//                    TransactionDate = new DateTime(2019, 9, 17),
//                    TransactionType = "Płatność kartą",
//                    Amount = -13.00M,
//                    Currency = "PLN",
//                    Title = "Tytuł: Some transaction title",
//                    Location = "Lokalizacja: Kraj: POLSKA Miasto: CHORZOW Adres: PIEKARNIA CUKIERNIA RUTA"
//                },
//            }
//            , options => options.WithStrictOrdering());
//        }

//        private class Fixture
//        {
//            private readonly List<string> lines = new List<string>();

//            public Fixture WithLine(string line)
//            {
//                lines.Add(line);
//                return this;
//            }

//            public CsvImporter Configure()
//            {
//                using (var autoSubstitute = new AutoSubstitute())
//                {
//                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
//                    autoSubstitute.Resolve<IFileReader>().ReadLines(Arg.Any<string>(), Arg.Any<Encoding>()).Returns(lines);
//                    autoSubstitute.Provide<ITypeImporter>(new CardTransactionImporter());
//                    autoSubstitute.Provide<ITypeImporter>(new LoanPaymentImporter());
//                    autoSubstitute.Provide<ITypeImporter>(new StandingOrderImporter());
//                    autoSubstitute.Provide<ITypeImporter>(new TransferFromAccountImporter());
//                    autoSubstitute.Provide<ITypeImporter>(new TransferToAccountImporter());
//                    autoSubstitute.Provide<ITypeImporter>(new TransferToPhoneIncomingImporter());
//                    autoSubstitute.Provide<ITypeImporter>(new TransferToPhoneOutgoingImporter());
//                    autoSubstitute.Provide<ITypeImporter>(new WebPaymentImporter());
//                    return autoSubstitute.Resolve<CsvImporter>();
//                }
//            }
//        }
//    }
//}
