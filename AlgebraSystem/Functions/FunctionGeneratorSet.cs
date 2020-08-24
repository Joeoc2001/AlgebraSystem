using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Algebra.Functions.HardcodedFunctionIdentities;

namespace Algebra.Functions
{
    public class FunctionGeneratorSet : IEnumerable<IFunctionGenerator>
    {
        private static readonly IFunctionGenerator sinFunctionGenerator = new AtomicFunctionGenerator("sin", 1, list => Expression.SinOf(list[0]));
        private static readonly IFunctionGenerator arcsinFunctionGenerator = new AtomicFunctionGenerator("arcsin", 1, list => Expression.ArcsinOf(list[0]));
        private static readonly IFunctionGenerator arctanFunctionGenerator = new AtomicFunctionGenerator("arctan", 1, list => Expression.ArctanOf(list[0]));
        private static readonly IFunctionGenerator lnFunctionGenerator = new AtomicFunctionGenerator("ln", 1, list => Expression.LnOf(list[0]));
        private static readonly IFunctionGenerator signFunctionGenerator = new AtomicFunctionGenerator("sign", 1, list => Expression.SignOf(list[0]));

        public static readonly FunctionGeneratorSet DefaultFunctions = new FunctionGeneratorSet()
        {
            { sinFunctionGenerator },
            { CosIdentity.Instance },
            { TanIdentity.Instance },
            { arcsinFunctionGenerator },
            { ArccosIdentity.Instance },
            { arctanFunctionGenerator },
            { lnFunctionGenerator },
            { LogIdentity.Instance },
            { signFunctionGenerator },
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

        private readonly Dictionary<string, IFunctionGenerator> generators;

        public FunctionGeneratorSet()
            : this(new Dictionary<string, IFunctionGenerator>())
        {

        }

        public FunctionGeneratorSet(FunctionGeneratorSet generators)
            : this(generators.generators)
        {

        }

        public FunctionGeneratorSet(IDictionary<string, IFunctionGenerator> generators)
        {
            this.generators = new Dictionary<string, IFunctionGenerator>(generators);
        }

        public ICollection<string> Names { get => generators.Keys; }

        public IFunctionGenerator this[string name]
        {
            get => generators[name];
        }

        public void Add(IFunctionGenerator generator)
        {
            generators.Add(generator.GetName(), generator);
        }

        public IEnumerator<IFunctionGenerator> GetEnumerator()
        {
            return generators.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return generators.Values.GetEnumerator();
        }
    }
}
