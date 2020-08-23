using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Algebra
{
    namespace Functions
    {
        /// <summary>
        /// Used to generate an atomic function as is, without a function wrapper.
        /// Used specifically for sin, sign and ln
        /// </summary>
        internal class AtomicFunctionGenerator : FunctionGenerator
        {
            public delegate Expression AtomicFunctionGeneratorDelegate(List<Expression> parameters);

            private readonly AtomicFunctionGeneratorDelegate gen;

            public AtomicFunctionGenerator(string name, int parameterCount, AtomicFunctionGeneratorDelegate gen)
                : base(name, GenerateRequiredParameters(parameterCount))
            {
                this.gen = gen;
            }

            private static ReadOnlyCollection<string> GenerateRequiredParameters(int parameterCount)
            {
                char start = 'a';
                List<string> names = new List<string>();
                for (int i = 0; i < parameterCount; i++)
                {
                    names.Add("" + start);
                    start = (char)(start + 1);
                }
                return names.AsReadOnly();
            }

            protected override Expression CreateExpressionImpl(IDictionary<string, Expression> parameters)
            {
                IList<string> names = GetRequiredParameters();
                List<Expression> orderedParameters = new List<Expression>();

                foreach (string name in names)
                {
                    orderedParameters.Add(parameters[name]);
                }

                return gen(orderedParameters);
            }
        }
    }
}
