using Algebra.Functions.FunctionIdentities;
using System.Collections;
using System.Collections.Generic;

namespace Algebra.Functions
{
    public class FunctionIdentitySet : IEnumerable<FunctionIdentity>, IDictionary<string, FunctionIdentity>
    {
        public static readonly FunctionIdentitySet _defaultFunctionIdentities = new FunctionIdentitySet()
        {
            { CosIdentity.Instance },
            { TanIdentity.Instance },
            { ArccosIdentity.Instance },
            { LogIdentity.Instance },
            { MaxIdentity.Instance },
            { MinIdentity.Instance },
            { SelectIdentity.Instance },
            { AbsIdentity.Instance },
            { SinhIdentity.Instance },
            { CoshIdentity.Instance },
            { TanhIdentity.Instance },
            { ArsinhIdentity.Instance },
            { ArcoshIdentity.Instance },
            { ArtanhIdentity.Instance },
            { SqrtIdentity.Instance },
        };

        public static FunctionIdentitySet DefaultFunctionIdentities
        {
            get => new FunctionIdentitySet(_defaultFunctionIdentities);
        }

        private readonly Dictionary<string, FunctionIdentity> _identities;

        public FunctionIdentitySet()
            : this(new Dictionary<string, FunctionIdentity>())
        {

        }

        public FunctionIdentitySet(IDictionary<string, FunctionIdentity> identities)
        {
            _identities = new Dictionary<string, FunctionIdentity>(identities);
        }

        public FunctionIdentity this[string key] { get => ((IDictionary<string, FunctionIdentity>)_identities)[key]; set => ((IDictionary<string, FunctionIdentity>)_identities)[key] = value; }

        public ICollection<string> Keys => ((IDictionary<string, FunctionIdentity>)_identities).Keys;

        public ICollection<FunctionIdentity> Values => ((IDictionary<string, FunctionIdentity>)_identities).Values;

        public int Count => ((ICollection<KeyValuePair<string, FunctionIdentity>>)_identities).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, FunctionIdentity>>)_identities).IsReadOnly;

        public void Add(FunctionIdentity identity)
        {
            _identities.Add(identity.GetName(), identity);
        }

        public void Add(string key, FunctionIdentity value)
        {
            ((IDictionary<string, FunctionIdentity>)_identities).Add(key, value);
        }

        public void Add(KeyValuePair<string, FunctionIdentity> item)
        {
            ((ICollection<KeyValuePair<string, FunctionIdentity>>)_identities).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, FunctionIdentity>>)_identities).Clear();
        }

        public bool Contains(KeyValuePair<string, FunctionIdentity> item)
        {
            return ((ICollection<KeyValuePair<string, FunctionIdentity>>)_identities).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, FunctionIdentity>)_identities).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, FunctionIdentity>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, FunctionIdentity>>)_identities).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, FunctionIdentity>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, FunctionIdentity>>)_identities).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, FunctionIdentity>)_identities).Remove(key);
        }

        public bool Remove(KeyValuePair<string, FunctionIdentity> item)
        {
            return ((ICollection<KeyValuePair<string, FunctionIdentity>>)_identities).Remove(item);
        }

        public bool TryGetValue(string key, out FunctionIdentity value)
        {
            return ((IDictionary<string, FunctionIdentity>)_identities).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_identities).GetEnumerator();
        }

        IEnumerator<FunctionIdentity> IEnumerable<FunctionIdentity>.GetEnumerator()
        {
            return _identities.Values.GetEnumerator();
        }
    }
}
