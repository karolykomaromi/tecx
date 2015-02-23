namespace Hydra.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> Wrap<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            Contract.Requires(dict != null);
            Contract.Ensures(Contract.Result<IDictionary<TKey, TValue>>() != null);

            return new DictionaryEnhancements<TKey, TValue>(dict);
        }
    }
}