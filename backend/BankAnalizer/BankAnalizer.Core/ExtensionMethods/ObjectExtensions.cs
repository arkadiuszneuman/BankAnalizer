using System;
using System.Collections.Generic;
using System.Text;

namespace BankAnalizer.Core.ExtensionMethods
{
    public static class ObjectExtensions
    {
        public static List<T> AsList<T>(this T @object)
        {
            return new List<T>(1) { @object };
        }
    }
}
