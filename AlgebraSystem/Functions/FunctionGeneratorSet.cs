using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Algebra.Functions.HardcodedFunctionIdentities;

namespace Algebra.Functions
{
    public class FunctionGeneratorSet : IEnumerable<IFunctionGenerator>
    {
        private static readonly IFunctionGenerator _sinFunctionGenerator = new AtomicFunctionGenerator("sin", 1, list => Expression.SinOf(list[0]));
        private static readonly IFunctionGenerator _arcsinFunctionGenerator = new AtomicFunctionGenerator("arcsin", 1, list => Expression.ArcsinOf(list[0]));
        private static readonly IFunctionGenerator _arctanFunctionGenerator = new AtomicFunctionGenerator("arctan", 1, list => Expression.ArctanOf(list[0]));
        private static readonly IFunctionGenerator _lnFunctionGenerator = new AtomicFunctionGenerator("ln", 1, list => Expression.LnOf(list[0]));
        private static readonly IFunctionGenerator _signFunctionGenerator = new AtomicFunctionGenerator("sign", 1, list => Expression.SignOf(list[0]));

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

        private readonly Dictionary<string, IFunctionGenerator> _generators;

        public FunctionGeneratorSet()
            : this(new Dictionary<string, IFunctionGenerator>())
        {

        }

        public FunctionGeneratorSet(FunctionGeneratorSet generators)
            : this(generators._generators)
        {

        }

        public FunctionGeneratorSet(IDictionary<string, IFunctionGenerator> generators)
        {
            this._generators = new Dictionary<string, IFunctionGenerator>(generators);
        }

        public ICollection<string> Names { get => _generators.Keys; }

        public IFunctionGenerator this[string name]
        {
            get => _generators[name];
        }

        public void Add(IFunctionGenerator generator)
        {
            _generators.Add(generator.GetName(), generator);
        }

        public IEnumerator<IFunctionGenerator> GetEnumerator()
        {
            return _generators.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _generators.Values.GetEnumerator();
        }
    }
}
