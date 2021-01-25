using System;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class EnumerableExtensao
    {
        public static bool AnySafe<TSource>(this IEnumerable<TSource> source)
        {
            return source?.Any() ?? false;
        }
        public static bool AnySafe<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source?.Any(predicate) ?? false;
        }

    }
}
