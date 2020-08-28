using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.PatternMatching
{
    public class PatternMatchingResultSet : IReadOnlyCollection<PatternMatchingResult>
    {
        public static PatternMatchingResultSet None = new PatternMatchingResultSet(new HashSet<PatternMatchingResult>());
        public static PatternMatchingResultSet All = new PatternMatchingResultSet(PatternMatchingResult.Empty);

        private readonly HashSet<PatternMatchingResult> _results;

        public PatternMatchingResultSet(PatternMatchingResult result)
            : this(new HashSet<PatternMatchingResult>() { result })
        {

        }

        public PatternMatchingResultSet(ISet<PatternMatchingResult> results)
        {
            this._results = new HashSet<PatternMatchingResult>(results);
        }

        public PatternMatchingResultSet Intersect(PatternMatchingResultSet other)
        {
            if (this.IsNone || other.IsNone)
            {
                return None;
            }

            if (this.IsAny)
            {
                return other;
            }

            if (other.IsAny)
            {
                return this;
            }

            HashSet<PatternMatchingResult> intersections = new HashSet<PatternMatchingResult>();
            foreach (PatternMatchingResult result1 in this)
            {
                foreach (PatternMatchingResult result2 in other)
                {
                    PatternMatchingResult intersection = result1.CalculateJoin(result2);
                    if (intersection != null)
                    {
                        intersections.Add(intersection);
                    }
                }
            }

            return new PatternMatchingResultSet(intersections);
        }

        public PatternMatchingResultSet Union(PatternMatchingResultSet other)
        {
            HashSet<PatternMatchingResult> union = new HashSet<PatternMatchingResult>();
            union.UnionWith(_results);
            union.UnionWith(other._results);
            return new PatternMatchingResultSet(union);
        }

        public bool IsNone
        {
            get
            {
                return Count == 0;
            }
        }

        public bool IsAny
        {
            get
            {
                return _results.Contains(PatternMatchingResult.Empty);
            }
        }

        public int Count
        {
            get
            {
                return _results.Count;
            }
        }

        public IEnumerator<PatternMatchingResult> GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _results.GetEnumerator();
        }
    }
}
