using Algebra.Atoms;
using Algebra.Functions.HardcodedFunctionIdentities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Functions
{
    public class FunctionIdentity : FunctionGenerator, IFunctionIdentity
    {
        private readonly int hashSeed;
        private readonly Expression atomicExpression;

        public FunctionIdentity(string name, List<string> parameterNames, int hashSeed, Expression alternateExpression)
            : base(name, parameterNames)
        {
            this.hashSeed = hashSeed;
            this.atomicExpression = alternateExpression.GetAtomicExpression();
        }

        protected override Expression CreateExpressionImpl(IDictionary<string, Expression> nodes)
        {
            return new Function(this, nodes);
        }

        public int GetHashSeed()
        {
            return hashSeed;
        }

        public Expression GetBodyAsAtomicExpression()
        {
            return atomicExpression;
        }
    }
}
