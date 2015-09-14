namespace Hydra.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;

    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> Enhance<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            Contract.Requires(source != null);
            Contract.Ensures(Contract.Result<IDictionary<TKey, TValue>>() != null);

            return new HelpfulExceptionsDictionary<TKey, TValue>(source);
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(
            this IDictionary<TKey, TValue> first,
            IDictionary<TKey, TValue> second,
            Func<IEnumerable<TValue>, TValue> mergeValues)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            Contract.Ensures(Contract.Result<IDictionary<TKey, TValue>>() != null);

            return Merge(first, second, mergeValues, EqualityComparer<TKey>.Default);
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(
            this IDictionary<TKey, TValue> first,
            IDictionary<TKey, TValue> second,
            Func<IEnumerable<TValue>, TValue> mergeValues,
            IEqualityComparer<TKey> comparer)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            Contract.Ensures(Contract.Result<IDictionary<TKey, TValue>>() != null);

            var dictionaries = new[] { first, second };

            var merged = dictionaries
                .SelectMany(dict => dict)
                .ToLookup(pair => pair.Key, pair => pair.Value, comparer)
                .ToDictionary(g => g.Key, g => mergeValues(g), comparer);

            return merged;
        }
    }
}