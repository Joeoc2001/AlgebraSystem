using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    internal static class LazyFunctions
    {
        /// <summary>
        /// Loops over all ways of splitting a set of elements into seperate disjoint sets
        /// </summary>
        public static IEnumerable<List<List<T>>> Partition<T>(IList<T> elements, int maxlen = -1)
        {
            if (maxlen < 0)
            {
                maxlen = elements.Count;
            }

            if (maxlen == 0)
            {
                yield return new List<List<T>>();
            }
            else
            {
                T elem = elements[maxlen - 1];
                var shorter = Partition(elements, maxlen - 1);
                foreach (var part in shorter)
                {
                    foreach (var list in part.ToArray())
                    {
                        list.Add(elem);
                        yield return part;
                        list.RemoveAt(list.Count - 1);
                    }
                    var newlist = new List<T>
                    {
                        elem
                    };
                    part.Add(newlist);
                    yield return part;
                    part.RemoveAt(part.Count - 1);
                }
            }
        }

        /// <summary>
        /// Loops over all orderings of a set of elements
        /// </summary>
        public static IEnumerable<List<T>> Permute<T>(IList<T> elements)
        {
            if (elements.Count == 0)
            {
                yield return new List<T>();
            }

            for (int i = 0; i < elements.Count; i++)
            {
                T other = elements[0];
                elements.RemoveAt(0);

                foreach (List<T> otherPerm in Permute(elements))
                {
                    otherPerm.Add(other);
                    yield return otherPerm;
                    otherPerm.RemoveAt(otherPerm.Count - 1);
                }

                elements.Add(other);
            }
        }

        /// <summary>
        /// Loops over all pairs of [values and all other values] in a collection
        /// </summary>
        public static IEnumerable<(T, List<T>)> TakeOne<T>(ICollection<T> all)
        {
            List<T> vals = new List<T>(all);

            for (int i = 0; i < all.Count; i++)
            {
                T val = vals[0];
                vals.RemoveAt(0);
                yield return (val, vals);
                vals.Add(val);
            }
        }

        private static IEnumerable<(int largestIndex, List<T> left, List<T> right)> DualPartitionOfLength<T>(ICollection<T> all, int leftSize)
        {
            if (leftSize == 0)
            {
                yield return (0, new List<T>(), new List<T>(all));
            }
            else
            {
                foreach ((int largestIndex, List<T> left, List<T> right) in DualPartitionOfLength(all, leftSize - 1))
                {
                    for (int i = largestIndex; i < right.Count; i++)
                    {
                        T moved = right[i];
                        right.RemoveAt(i);
                        left.Add(moved);
                        yield return (i, left, right);
                        left.RemoveAt(left.Count - 1);
                        right.Insert(i, moved);
                    }
                }
            }
        }

        /// <summary>
        /// Loops over all ways of splitting a set of elements into 2 seperate disjoint sets
        /// </summary>
        public static IEnumerable<(List<T>, List<T>)> DualPartition<T>(ICollection<T> all)
        {
            for (int length = 0; length <= all.Count; length++)
            {
                foreach ((int _, List<T> left, List<T> right) in DualPartitionOfLength(all, length))
                {
                    yield return (left, right);
                }
            }
        }

        /// <summary>
        /// Loops over all ways of splitting a set of elements into 2 seperate disjoint sets which both have length >= 1
        /// </summary>
        public static IEnumerable<(List<T>, List<T>)> DualPartitionNonEmpty<T>(ICollection<T> all)
        {
            for (int length = 1; length <= all.Count - 1; length++)
            {
                foreach ((int _, List<T> left, List<T> right) in DualPartitionOfLength(all, length))
                {
                    yield return (left, right);
                }
            }
        }
    }
}
