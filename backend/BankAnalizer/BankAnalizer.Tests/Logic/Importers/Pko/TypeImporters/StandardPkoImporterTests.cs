using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BankAnalizer.Tests.Logic.Importers.Pko.TypeImporters
{
    public class StandardPkoImporterTests : BaseUnitTest<StandardPkoImporter>
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] {
                    new[] {
                        "2015-10-03","2016-01-01","Naliczenie odsetek",
                        "-0.03","PLN","+21.32","KAPIT.ODSETEK KARNYCH-OBCIĄŻENIE","","","","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2015, 10, 3),
                        TransactionDate = new DateTime(2016, 1, 1),
                        TransactionType = "Naliczenie odsetek",
                        Amount = -0.03M,
                        Currency = "PLN",
                        Title = "Naliczenie odsetek",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2013-07-07", "2013-07-06", "Opłata za użytkowanie karty", "-0.56", "PLN", "+123.32", "Tytuł: OPŁATA PROP. ZA KARTĘ 123123123*****323, 19.06-05.07", "", "", "", "", "", ""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2013, 7, 7),
                        TransactionDate = new DateTime(2013, 7, 6),
                        TransactionType = "Opłata za użytkowanie karty",
                        Amount = -0.56M,
                        Currency = "PLN",
                        Title = "OPŁATA PROP. ZA KARTĘ 123123123*****323, 19.06-05.07",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2018-02-22","2018-02-20","Prowizja","-32.21","PLN","+412.34",
                        "Tytuł: 32523523324234234","","","","","",""

                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2018, 2, 22),
                        TransactionDate = new DateTime(2018, 2, 20),
                        TransactionType = "Prowizja",
                        Amount = -32.21M,
                        Currency = "PLN",
                        Title = "32523523324234234",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2013-11-12","2013-11-10","Opłata składki ubezpieczeniowej",
                        "-412.32","PLN","+123.66","465345346ISAFIGASI4365345","","","","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2013, 11, 12),
                        TransactionDate = new DateTime(2013, 11, 10),
                        TransactionType = "Opłata składki ubezpieczeniowej",
                        Amount = -412.32M,
                        Currency = "PLN",
                        Title = "Opłata składki ubezpieczeniowej",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2018-12-05","2018-12-04","Uznanie","+123.28","PLN","+42.47",
                        "45345347345346435346","","","","","",""

                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2018, 12, 5),
                        TransactionDate = new DateTime(2018, 12, 4),
                        TransactionType = "Uznanie",
                        Amount = 123.28M,
                        Currency = "PLN",
                        Title = "Uznanie",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2018-04-30","2018-05-01","Podatek od odsetek","-0.01","PLN",
                        "+4345.54","PODATEK OD ODSETEK KAPITAŁOWYCH","","","","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2018, 4, 30),
                        TransactionDate = new DateTime(2018, 5, 1),
                        TransactionType = "Podatek od odsetek",
                        Amount = -0.01M,
                        Currency = "PLN",
                        Title = "Podatek od odsetek",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2019-09-19", "2019-09-17", "Płatność kartą",
                        "-13.00", "PLN", "+45.94", "Tytuł: Some transaction title",
                        "Lokalizacja: Kraj: POLSKA Miasto: CHORZOW Adres: PIEKARNIA CUKIERNIA RUTA",
                        "Data i czas operacji: 2019-02-05", "Oryginalna kwota operacji: 13,00 PLN",
                        "Numer karty: 123456******7589", "", ""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 9, 19),
                        TransactionDate = new DateTime(2019, 9, 17),
                        TransactionType = "Płatność kartą",
                        Amount = -13.00M,
                        Currency = "PLN",
                        Title = "Some transaction title",
                        Extensions = new
                        {
                            Location = "Kraj: POLSKA Miasto: CHORZOW Adres: PIEKARNIA CUKIERNIA RUTA"
                        }.ToJson()
                    }
                };
                yield return new object[] {
                    new[] {
                        "2018-03-23","2018-03-24","Wpłata gotówki we wpłatomacie","+11.21","PLN","+32.22",
                        "Tytuł: PKO BP 123123123123",
                        "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: STREET",
                        "Data i czas operacji: 2018-03-23 17:07:35","Oryginalna kwota operacji: 11,21 PLN",
                        "Numer karty: 1234*****323","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2018, 3, 23),
                        TransactionDate = new DateTime(2018, 3, 24),
                        TransactionType = "Wpłata gotówki we wpłatomacie",
                        Amount = 11.21M,
                        Currency = "PLN",
                        Title = "PKO BP 123123123123",
                        Extensions = new
                        {
                            Location = "Kraj: COUNTRY Miasto: CITY Adres: STREET"
                        }.ToJson()
                    }
                };
                yield return new object[] {
                    new[] {
                        "2019-01-11","2019-01-12","Obciążenie","-11.32","PLN","+21.83",
                        "1231255123123151231235123123","","","","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 1, 11),
                        TransactionDate = new DateTime(2019, 1, 12),
                        TransactionType = "Obciążenie",
                        Amount = -11.32M,
                        Currency = "PLN",
                        Title = "Obciążenie",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2019-01-11","2019-01-12","Opłata","-11.32","PLN","+21.83",
                        "1231255123123151231235123123","","","","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 1, 11),
                        TransactionDate = new DateTime(2019, 1, 12),
                        TransactionType = "Opłata",
                        Amount = -11.32M,
                        Currency = "PLN",
                        Title = "Opłata",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2019-03-23","2019-03-24","Polecenie Zapłaty","-21.32","PLN","+21.48",
                        "Rachunek odbiorcy: 88 7777 2222 4444 0000 6666 9999","Nazwa odbiorcy: SOME RECIPIENT",
                        "Adres odbiorcy: SOME RECIPIENT ADDRESS",
                        "Tytuł: SOME TITLE","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 23),
                        TransactionDate = new DateTime(2019, 3, 24),
                        TransactionType = "Polecenie Zapłaty",
                        Amount = -21.32M,
                        Currency = "PLN",
                        Extensions = new {
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
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2016, 12, 23),
                        TransactionDate = new DateTime(2016, 12, 22),
                        TransactionType = "Przelew podatkowy",
                        Amount = -231.32M,
                        Currency = "PLN",
                        Extensions = new {
                            RecipientReceipt = "44 5555 3333 2222 5555 4444 3333",
                            RecipientName = "PIERWSZY URZĄD SKARBOWY",
                            RecipientAddress = "OPOLE"
                        }.ToJson(),
                        Title = "Przelew podatkowy",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2019-03-23","2019-03-24","Przelew Natychmiastowy na rachunek","+123.12","PLN","+32.06",
                        "Rachunek nadawcy: 44 2222 5555 1111 7777 4444 3333","Nazwa nadawcy: SOME RECIPIENT",
                        "Tytuł: PRZELEW ŚRODKÓW","","","",""

                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 23),
                        TransactionDate = new DateTime(2019, 3, 24),
                        TransactionType = "Przelew Natychmiastowy na rachunek",
                        Amount = 123.12M,
                        Currency = "PLN",
                        Extensions = new
                        {
                            SenderReceipt = "44 2222 5555 1111 7777 4444 3333",
                            SenderName = "SOME RECIPIENT"
                        }.ToJson(),
                        Title = "PRZELEW ŚRODKÓW",
                    }
                };
                yield return new object[] {
                    new[] { "2019-02-15","2019-02-16","Spłata kredytu",
                        "-123.48","PLN","+2342.21",
                        "Tytuł: KAPITAŁ: 123,48 ODSETKI: 0,00 ODSETKI SKAPIT.: 0,00 ODSETKI KARNE: 0,00 321232123","","","","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 2, 15),
                        TransactionDate = new DateTime(2019, 2, 16),
                        TransactionType = "Spłata kredytu",
                        Amount = -123.48M,
                        Currency = "PLN",
                        Title = "KAPITAŁ: 123,48 ODSETKI: 0,00 ODSETKI SKAPIT.: 0,00 ODSETKI KARNE: 0,00 321232123",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2019-03-23","2019-03-24","Zwrot płatności kartą","+9.72","PLN","+22.54","Tytuł: 5425465 55487541",
                        "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: ADDRESS",
                        "Data i czas operacji: 2019-03-24","Oryginalna kwota operacji: 2,75 USD",
                        "Data przetworzenia: 2019-03-24","Numer karty: 2156452*****458",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 23),
                        TransactionDate = new DateTime(2019, 3, 24),
                        TransactionType = "Zwrot płatności kartą",
                        Amount = 9.72M,
                        Currency = "PLN",
                        Title = "5425465 55487541",
                        Extensions = new
                        {
                            Location = "Kraj: COUNTRY Miasto: CITY Adres: ADDRESS"
                        }.ToJson()
                    }
                };
                yield return new object[] {
                    new[] {
                        "2019-04-13","2019-04-12","Wpłata gotówkowa w kasie","+234.32","PLN","+2564.00",
                        "Nazwa nadawcy: NAME SURNAME","Adres nadawcy: - -","Tytuł: WPŁATA WŁASNA","","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 04, 13),
                        TransactionDate = new DateTime(2019, 04, 12),
                        TransactionType = "Wpłata gotówkowa w kasie",
                        Amount = 234.32M,
                        Currency = "PLN",
                        Extensions = new {
                            SenderName = "NAME SURNAME",
                            SenderAddress = "- -"
                        }.ToJson(),
                        Title = "WPŁATA WŁASNA",
                    }
                };
                yield return new object[] {
                    new[] { "2019-03-01","2019-03-02","Zlecenie stałe","-73.21","PLN",
                        "+321.07","Rachunek odbiorcy: 11 2222 3333 0000 9999 8888 7777",
                        "Nazwa odbiorcy: SOME RECIPIENT NAME",
                        "Adres odbiorcy: SOME ADDRESS 32/45 CITY",
                        "Tytuł: NAME SURNAMESTREET 32/441-506 CHORZÓW","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 1),
                        TransactionDate = new DateTime(2019, 3, 2),
                        TransactionType = "Zlecenie stałe",
                        Amount = -73.21M,
                        Currency = "PLN",
                        Extensions = new
                        {
                            RecipientReceipt = "11 2222 3333 0000 9999 8888 7777",
                            RecipientName = "SOME RECIPIENT NAME",
                            RecipientAddress = "SOME ADDRESS 32/45 CITY",
                        }.ToJson(),
                        Title = "NAME SURNAMESTREET 32/441-506 CHORZÓW",
                    }
                };
                yield return new object[] {
                    new[] { "2019-03-06","2019-03-07","Przelew z rachunku",
                        "-43.47","PLN","+123.04","Rachunek odbiorcy: 99 8888 5555 2222 0001 7777 4444",
                        "Nazwa odbiorcy: RECIPIENT NAME","Adres odbiorcy: SOME RECIPIENT ADDRESS 32/43 54-322 CITY",
                        "Tytuł: SOME TITLE","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 6),
                        TransactionDate = new DateTime(2019, 3, 7),
                        TransactionType = "Przelew z rachunku",
                        Amount = -43.47M,
                        Currency = "PLN",
                        Extensions = new
                        {
                            RecipientReceipt = "99 8888 5555 2222 0001 7777 4444",
                            RecipientName = "RECIPIENT NAME",
                            RecipientAddress = "SOME RECIPIENT ADDRESS 32/43 54-322 CITY",
                        }.ToJson(),
                        Title = "SOME TITLE",
                    }
                };
                yield return new object[] {
                    new[] { "2019-09-05","2019-09-06","Przelew na rachunek",
                        "+213.32","PLN","+123.52","Rachunek nadawcy: 33 4444 0001 4444 5555 6666 9999",
                        "Nazwa nadawcy: SENDER NAME","Adres nadawcy: Street 4A/23 43-532 City",
                        "Tytuł: Some title","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 9, 5),
                        TransactionDate = new DateTime(2019, 9, 6),
                        TransactionType = "Przelew na rachunek",
                        Amount = 213.32M,
                        Currency = "PLN",
                        Extensions = new {
                            SenderReceipt = "33 4444 0001 4444 5555 6666 9999",
                            SenderName = "SENDER NAME",
                            SenderAddress = "Street 4A/23 43-532 City",
                        }.ToJson(),
                        Title = "Some title",
                    }
                };
                yield return new object[] {
                    new[] { "2019-03-26","2019-03-27","Przelew na telefon przychodz. zew.",
                        "+11.73","PLN","+123.90","Rachunek nadawcy: 11 3333 6666 2222 1111 8888 6666",
                        "Nazwa nadawcy: SNAME SSURNAME","Tytuł: PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME","","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 26),
                        TransactionDate = new DateTime(2019, 3, 27),
                        TransactionType = "Przelew na telefon przychodz. zew.",
                        Amount = 11.73M,
                        Currency = "PLN",
                        Extensions = new
                        {
                            SenderReceipt = "11 3333 6666 2222 1111 8888 6666",
                            SenderName = "SNAME SSURNAME",
                        }.ToJson(),
                        Title = "PRZELEW NA TELEFON +48XXXXXX123 BOLT DLA RNAME RSURNAME OD SNAME SSURNAME",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2019-03-25","2019-03-26","Przelew na telefon wychodzący zew.",
                        "-32.12","PLN","+321.87","Rachunek odbiorcy: 44 5555 7777 1111 3333 6666 7777",
                        "Nazwa odbiorcy: NAME SURNAME","Tytuł: PRZELEW NA TELEFON OD: 48930293023 DO: 48950495023","","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 3, 25),
                        TransactionDate = new DateTime(2019, 3, 26),
                        TransactionType = "Przelew na telefon wychodzący zew.",
                        Amount = -32.12M,
                        Currency = "PLN",
                        Extensions = new {
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
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2018, 2, 12),
                        TransactionDate = new DateTime(2018, 2, 11),
                        TransactionType = "Przelew na telefon wychodzący wew.",
                        Amount = -123.42M,
                        Currency = "PLN",
                        Extensions = new {
                            RecipientReceipt = "33 4444 5555 7777 8888 9999 3333",
                            RecipientName = "ODBIORCA PRZELEWU NA TELEFON"
                        }.ToJson(),
                        Title = "PIŁKA OD: 48384959485 DO: 48594859483",
                    }
                };
                yield return new object[] {
                    new[] {
                        "2019-04-18","2019-04-17","Płatność web - kod mobilny",
                        "-10.19","PLN","+507.94","Numer telefonu: +48 568 457 587 ","Lokalizacja: Adres: www.skycash.com",
                        "Data i czas operacji: 2019-04-17 17:16:10","Numer referencyjny: 5458568544654","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 4, 18),
                        TransactionDate = new DateTime(2019, 4, 17),
                        TransactionType = "Płatność web - kod mobilny",
                        Amount = -10.19M,
                        Currency = "PLN",
                        Title =  "Płatność web - kod mobilny",
                        Extensions = new {
                            PhoneNumber = "+48 568 457 587",
                            Location = "Adres: www.skycash.com"
                        }.ToJson()
                    }
                };

                yield return new object[] {
                    new[] {
                        "2017-02-20","2017-02-19","Wypłata w bankomacie - kod mobilny",
                        "-21.32","PLN","+12.76","Numer telefonu: +48 422 532 432 ",
                        "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 38",
                        "Data i czas operacji: 2017-02-19 13:34:32",
                        "Bankomat: ASDAGAWD","Numer referencyjny: 00123102512031203","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2017, 2, 20),
                        TransactionDate = new DateTime(2017, 2, 19),
                        TransactionType = "Wypłata w bankomacie - kod mobilny",
                        Amount = -21.32M,
                        Currency = "PLN",
                        Title = "Wypłata w bankomacie - kod mobilny",
                        Extensions = new {
                            PhoneNumber = "+48 422 532 432",
                            Location = "Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 38"
                        }.ToJson()
                    }
                };

                yield return new object[] {
                    new[] {
                        "2014-11-15","2014-11-14","Anulowanie wypłaty w bankomacie - kod mobilny",
                        "+12.32","PLN","+54.43","Numer telefonu: +48 435 542 543 ",
                        "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 45",
                        "Data i czas operacji: 2014-11-07 03:21:21","Bankomat: 123125412",
                        "Numer referencyjny: 5325346346345345364","",""

                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2014, 11, 15),
                        TransactionDate = new DateTime(2014, 11, 14),
                        TransactionType = "Anulowanie wypłaty w bankomacie - kod mobilny",
                        Amount = 12.32M,
                        Currency = "PLN",
                        Title = "Anulowanie wypłaty w bankomacie - kod mobilny",
                        Extensions = new {
                            PhoneNumber = "+48 435 542 543",
                            Location = "Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 45"
                        }.ToJson()
                    }
                };
                yield return new object[] {
                    new[] {
                        "2018-12-06","2018-12-05","Zakup w terminalu - kod mobilny",
                        "-23.32","PLN","+1111.50","Numer telefonu: +48 423 425 423 ",
                        "Lokalizacja: Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 45",
                        "Data i czas operacji: 2018-32-23 22:26:51",
                        "Numer referencyjny: 1423125124","","",""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2018, 12, 06),
                        TransactionDate = new DateTime(2018, 12, 05),
                        TransactionType = "Zakup w terminalu - kod mobilny",
                        Amount = -23.32M,
                        Currency = "PLN",
                        Title = "Zakup w terminalu - kod mobilny",
                        Extensions = new {
                            PhoneNumber = "+48 423 425 423",
                            Location = "Kraj: COUNTRY Miasto: CITY Adres: UL. STREET 45"
                        }.ToJson()
                    }
                };
                yield return new object[] {
                    new[] { "2019-02-09", "2019-02-08", "Wypłata z bankomatu", "-321.32", "PLN", "+32.73",
                            "Tytuł: PKO BP 123123123", "Lokalizacja: Kraj: POLSKA Miasto: SOMECITY Adres: UL. SOMNEADDRESS 45",
                            "Data i czas operacji: 2019-02-08 11:34:51", "Oryginalna kwota operacji: 32,73 PLN", "Numer karty: 425125******4284", "", ""
                    },
                    new ImportedBankTransaction
                    {
                        OperationDate = new DateTime(2019, 2, 9),
                        TransactionDate = new DateTime(2019, 2, 8),
                        TransactionType = "Wypłata z bankomatu",
                        Amount = -321.32M,
                        Currency = "PLN",
                        Title = "PKO BP 123123123",
                        Extensions = new
                        {
                            Location = "Kraj: POLSKA Miasto: SOMECITY Adres: UL. SOMNEADDRESS 45"
                        }.ToJson()
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_import_transactions(string[] splittedLine, ImportedBankTransaction expectedResult)
        {
            //act
            var result = Sut.Import(splittedLine);

            //assert
            result.Should().BeEquivalentTo(expectedResult);
        }

    }
}
