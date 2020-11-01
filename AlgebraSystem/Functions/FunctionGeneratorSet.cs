using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Algebra.Functions.FunctionIdentities;

namespace Algebra.Functions
{
    public class FunctionGeneratorSet : IEnumerable<FunctionGenerator>, IDictionary<string, FunctionGenerator>
    {
        private static readonly FunctionGenerator _sinFunctionGenerator = new AtomicFunctionGenerator("sin", 1, list => Expression.SinOf(list[0]));
        private static readonly FunctionGenerator _arcsinFunctionGenerator = new AtomicFunctionGenerator("arcsin", 1, list => Expression.ArcsinOf(list[0]));
        private static readonly FunctionGenerator _arctanFunctionGenerator = new AtomicFunctionGenerator("arctan", 1, list => Expression.ArctanOf(list[0]));
        private static readonly FunctionGenerator _lnFunctionGenerator = new AtomicFunctionGenerator("ln", 1, list => Expression.LnOf(list[0]));
        private static readonly FunctionGenerator _signFunctionGenerator = new AtomicFunctionGenerator("sign", 1, list => Expression.SignOf(list[0]));

        public static readonly FunctionGeneratorSet _defaultFunctionGenerators = new FunctionGeneratorSet(FunctionIdentitySet.DefaultFunctionIdentities)
        {
            { _sinFunctionGenerator },
            { _arcsinFunctionGenerator },
            { _arctanFunctionGenerator },
            { _lnFunctionGenerator },
            { _signFunctionGenerator },
        };

        public static FunctionGeneratorSet DefaultFunctionGenerators
        {
            get => new FunctionGeneratorSet(_defaultFunctionGenerators);
        }

        private readonly Dictionary<string, FunctionGenerator> _generators;

        public FunctionGeneratorSet()
            : this(new Dictionary<string, FunctionGenerator>())
        {

        }

        public FunctionGeneratorSet(FunctionGeneratorSet generators)
            : this(generators._generators)
        {

        }

        public FunctionGeneratorSet(IDictionary<string, FunctionGenerator> generators)
        {
            _generators = new Dictionary<string, FunctionGenerator>(generators);
        }

        public FunctionGeneratorSet(IEnumerable<FunctionGenerator> generators)
            : this()
        {
            foreach (var generator in generators)
            {
                Add(generator);
            }
        }

        public ICollection<string> Names { get => _generators.Keys; }

        public ICollection<string> Keys => ((IDictionary<string, FunctionGenerator>)_generators).Keys;

        public ICollection<FunctionGenerator> Values => ((IDictionary<string, FunctionGenerator>)_generators).Values;

        public int Count => ((ICollection<KeyValuePair<string, FunctionGenerator>>)_generators).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, FunctionGenerator>>)_generators).IsReadOnly;

        FunctionGenerator IDictionary<string, FunctionGenerator>.this[string key] { get => ((IDictionary<string, FunctionGenerator>)_generators)[key]; set => ((IDictionary<string, FunctionGenerator>)_generators)[key] = value; }
        public FunctionGenerator this[string name]
        {
            get => _generators[name];
        }

        public void Add(FunctionGenerator generator)
        {
            _generators.Add(generator.GetName(), generator);
        }

        public IEnumerator<FunctionGenerator> GetEnumerator()
        {
            return _generators.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _generators.GetEnumerator();
        }

        public void Add(string key, FunctionGenerator value)
        {
            ((IDictionary<string, FunctionGenerator>)_generators).Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, FunctionGenerator>)_generators).ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, FunctionGenerator>)_generators).Remove(key);
        }

        public bool TryGetValue(string key, out FunctionGenerator value)
        {
            return ((IDictionary<string, FunctionGenerator>)_generators).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, FunctionGenerator> item)
        {
            ((ICollection<KeyValuePair<string, FunctionGenerator>>)_generators).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, FunctionGenerator>>)_generators).Clear();
        }

        public bool Contains(KeyValuePair<string, FunctionGenerator> item)
        {
            return ((ICollection<KeyValuePair<string, FunctionGenerator>>)_generators).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, FunctionGenerator>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, FunctionGenerator>>)_generators).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, FunctionGenerator> item)
        {
            return ((ICollection<KeyValuePair<string, FunctionGenerator>>)_generators).Remove(item);
        }

        IEnumerator<KeyValuePair<string, FunctionGenerator>> IEnumerable<KeyValuePair<string, FunctionGenerator>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, FunctionGenerator>>)_generators).GetEnumerator();
        }
    }
}
