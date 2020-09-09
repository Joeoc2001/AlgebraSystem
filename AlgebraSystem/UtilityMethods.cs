using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    static internal class UtilityMethods
    {
        /// <summary>
        /// Used to patch the gap between .net versions so we can target lower
        /// </summary>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}
