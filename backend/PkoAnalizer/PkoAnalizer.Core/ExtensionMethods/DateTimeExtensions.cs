using System;

namespace PkoAnalizer.Core.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static string ToSqlDateTimeString(this DateTime dateTime) =>
            dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
}
