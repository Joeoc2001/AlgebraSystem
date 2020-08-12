using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Algebra.Functions
{
    /// <summary>
    /// Used to generate an atomic function as is, without a function wrapper.
    /// Used specifically for sin, sign and ln
    /// </summary>
    public class AtomicFunctionGenerator : FunctionGenerator
    {
        public delegate Expression AtomicFunctionGeneratorDelegate(List<Expression> parameters);

        private readonly int parameterCount;
        private readonly AtomicFunctionGeneratorDelegate gen;

        private readonly ReadOnlyCollection<string> parameterNames;

        public AtomicFunctionGenerator(int parameterCount, AtomicFunctionGeneratorDelegate gen)
        {
            this.parameterCount = parameterCount;
            this.gen = gen;
            parameterNames = GenerateRequiredParameters();
        }

        private ReadOnlyCollection<string> GenerateRequiredParameters()
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

        public override ReadOnlyCollection<string> GetRequiredParameters()
        {
            return parameterNames;
        }

        protected override Expression CreateExpressionImpl(Dictionary<string, Expression> parameters)
        {
            ReadOnlyCollection<string> names = GetRequiredParameters();
            List<Expression> orderedParameters = new List<Expression>();

            foreach (string name in names)
            {
                orderedParameters.Add(parameters[name]);
            }

            return gen(orderedParameters);
        }
    }
}
