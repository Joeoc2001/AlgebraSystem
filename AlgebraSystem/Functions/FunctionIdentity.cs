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
        private readonly IAtomicExpression atomicExpression;

        public FunctionIdentity(string name, int hashSeed, IExpression alternateExpression)
            : base(name, alternateExpression.GetVariables())
        {
            this.hashSeed = hashSeed;
            this.atomicExpression = alternateExpression.GetAtomicExpression();
        }

        protected override IExpression CreateExpressionImpl(IDictionary<string, IExpression> nodes)
        {
            return new Function(this, nodes);
        }

        public int GetHashSeed()
        {
            return hashSeed;
        }

        public IAtomicExpression GetBodyAsAtomicExpression()
        {
            return atomicExpression;
        }
    }
}
