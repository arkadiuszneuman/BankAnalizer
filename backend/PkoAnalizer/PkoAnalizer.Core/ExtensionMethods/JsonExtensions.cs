using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PkoAnalizer.Core.ExtensionMethods
{
    public static class JsonExtensions
    {
        public static string ToJson(this object @object)
        {
            return JsonSerializer.Serialize(@object);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
