using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class StringExtensions
    {
        public static string Join(this string? separator, params string?[] values) => string.Join(separator, values);

        public static string Join(this string? separator, IEnumerable<string?> values) =>  string.Join(separator, values);

        public static string Join<T>(this string? separator, IEnumerable<T> values) => string.Join(separator, values);

        public static string Join(this string? separator, params object?[] values) => string.Join(separator, values);

        public static string Join(this string? separator, string?[] value, int startIndex, int count) => string.Join(separator, value, startIndex, count);
        
    }
}
