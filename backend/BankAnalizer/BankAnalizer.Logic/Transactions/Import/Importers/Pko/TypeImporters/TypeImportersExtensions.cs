using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters
{
    public static class TypeImportersExtensions
    {
        public static string Index(this string[] lines, int index)
        {
            if (index >= lines.Length)
                throw new ImportException($"Invalid index {index}. Max index: {lines.Length - 1}. Lines: {string.Join(Environment.NewLine, lines)}");

            return lines[index];
        }

        public static decimal ConvertToDecimal(this string value)
        {
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var returnValue))
            {
                return returnValue;
            }

            throw new ImportException($"Cannot convert to decimal value: {value}");
        }

        public static DateTime ConvertToDate(this string value)
        {
            if (DateTime.TryParseExact(value, "yyyy-MM-dd", null, DateTimeStyles.None, out var returnValue))

            {
                return returnValue;
            }

            throw new ImportException($"Cannot convert to DateTime value: {value}");
        }
    }
}
