using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Algebra.Functions.FunctionIdentities;

namespace Algebra.Functions
{
    public class FunctionGeneratorSet : IEnumerable<FunctionGenerator>
    {
        private static readonly FunctionGenerator _sinFunctionGenerator = new AtomicFunctionGenerator("sin", 1, list => Expression.SinOf(list[0]));
        private static readonly FunctionGenerator _arcsinFunctionGenerator = new AtomicFunctionGenerator("arcsin", 1, list => Expression.ArcsinOf(list[0]));
        private static readonly FunctionGenerator _arctanFunctionGenerator = new AtomicFunctionGenerator("arctan", 1, list => Expression.ArctanOf(list[0]));
        private static readonly FunctionGenerator _lnFunctionGenerator = new AtomicFunctionGenerator("ln", 1, list => Expression.LnOf(list[0]));
        private static readonly FunctionGenerator _signFunctionGenerator = new AtomicFunctionGenerator("sign", 1, list => Expression.SignOf(list[0]));

        public static readonly FunctionGeneratorSet DefaultFunctions = new FunctionGeneratorSet()
        {
            { _sinFunctionGenerator },
            { CosIdentity.Instance },
            { TanIdentity.Instance },
            { _arcsinFunctionGenerator },
            { ArccosIdentity.Instance },
            { _arctanFunctionGenerator },
            { _lnFunctionGenerator },
            { LogIdentity.Instance },
            { _signFunctionGenerator },
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
            this._generators = new Dictionary<string, FunctionGenerator>(generators);
        }

        public ICollection<string> Names { get => _generators.Keys; }

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
            return _generators.Values.GetEnumerator();
        }
    }
}
