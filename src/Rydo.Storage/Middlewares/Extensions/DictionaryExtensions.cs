namespace Rydo.Storage.Middlewares.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static TValue SafeGetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, Func<TKey, TValue> addFactory)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            lock (dictionary)
            {
                if (dictionary.TryGetValue(key, out value))
                {
                    return value;
                }

                value = addFactory(key);

                dictionary.Add(key, value);

                return value;
            }
        }
    }
}