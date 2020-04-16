using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BankAnalizer.Core.ExtensionMethods
{
    public static class JsonExtensions
    {
        public static string ToJson(this object @object)
        {
            return JsonSerializer.Serialize(@object, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
        }

        public static T FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
        }
    }
}
