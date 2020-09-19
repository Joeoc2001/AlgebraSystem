using Algebra.Atoms;
using Algebra.Functions.HardcodedFunctionIdentities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Functions
{
    public class FunctionIdentity : FunctionGenerator
    {
        private readonly int _hashSeed;
        private readonly Expression _atomicExpression;

        public FunctionIdentity(string name, int hashSeed, Expression alternateExpression)
            : base(name, alternateExpression.GetVariableNames())
        {
            _hashSeed = hashSeed;
            _atomicExpression = alternateExpression.GetAtomicExpression();
        }

        protected override Expression CreateExpressionImpl(IDictionary<string, Expression> nodes)
        {
            return new Function(this, nodes);
        }

        public int GetHashSeed()
        {
            return _hashSeed;
        }

        public Expression GetBodyAsAtomicExpression()
        {
            return _atomicExpression;
        }
    }
}
