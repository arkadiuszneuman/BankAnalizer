using System;
using System.Collections.Generic;
using System.Text;

namespace BankAnalizer.Core.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string RemoveSubstring(this string @string, string stringToRemove)
        {
            return @string.Replace(stringToRemove, "");
        }
    }
}
