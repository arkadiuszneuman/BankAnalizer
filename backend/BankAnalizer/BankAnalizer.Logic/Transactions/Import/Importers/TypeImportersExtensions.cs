using System;
using System.Globalization;

namespace BankAnalizer.Logic.Transactions.Import.Importers
{
    public static class TypeImportersExtensions
    {
        public static string Index(this string[] lines, int index)
        {
            if (index >= lines.Length)
                throw new InvalidImportRowException($"Invalid index {index}. Max index: {lines.Length - 1}. Lines: {string.Join(Environment.NewLine, lines)}");

            return lines[index];
        }

        public static decimal ConvertToDecimal(this string value)
        {
            if (decimal.TryParse(value.Replace(',','.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var returnValue))
                return returnValue;

            throw new InvalidImportRowException($"Cannot convert to decimal value: {value}");
        }

        public static DateTime ConvertToDate(this string value)
        {
            if (DateTime.TryParseExact(value, "yyyy-MM-dd", null, DateTimeStyles.None, out var returnValue))
                return returnValue;
            
            if (DateTime.TryParseExact(value, "yyyyMMdd", null, DateTimeStyles.None, out returnValue))
                return returnValue;

            if (DateTime.TryParseExact(value, "dd.MM.yyyy", null, DateTimeStyles.None, out returnValue))
                return returnValue;

            throw new InvalidImportRowException($"Cannot convert to DateTime value: {value}");
        }
    }
}
