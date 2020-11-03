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

        public static void Deconstruct<T1, T2, T3>(this Tuple<T1, T2, T3> tuple, out T1 t1, out T2 t2, out T3 t3)
        {
            t1 = tuple.Item1;
            t2 = tuple.Item2;
            t3 = tuple.Item3;
        }

        public static double Select(double a, double b, double c)
        {
            if (double.IsNaN(c))
            {
                return double.NaN;
            }

            if (c < 0)
            {
                return a;
            }
            if (c > 0)
            {
                return b;
            }
            return (a + b) / 2;
        }
    }
}
