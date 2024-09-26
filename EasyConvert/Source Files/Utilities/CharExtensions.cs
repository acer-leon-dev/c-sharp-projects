using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class CharExtensions
    {
        public static string Join(this char separator, params string?[] values) => string.Join(separator, values);

        public static string Join<T>(this char separator, IEnumerable<T> values) => string.Join(separator, values);

        public static string Join(this char separator, params object?[] values) => string.Join(separator, values);

        public static string Join(this char separator, string?[] value, int startIndex, int count) => string.Join(separator, value, startIndex, count);
    }
}
