using System;
using System.Collections.Generic;

namespace QueueTests
{
    public static class DictionaryExtensions
    {
        public static TValue GetWithDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> toCreateDefault)
        {
            return dictionary.ContainsKey(key)
                ? dictionary[key]
                : toCreateDefault(key);
        }
    }
}
