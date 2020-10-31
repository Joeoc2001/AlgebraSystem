using Algebra.Atoms;
using Algebra.Functions.FunctionIdentities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Algebra.Functions
{
    public class FunctionIdentity : FunctionGenerator
    {
        public class MismatchedParametersException : Exception
        {
            public MismatchedParametersException(string message)
                : base(message)
            {
            }
        }

        private readonly int _hashSeed;
        private readonly Expression _atomicExpression;

        public FunctionIdentity(string name, int hashSeed, Expression alternateExpression, IEnumerable<string> parameters)
            : base(name, parameters)
        {
            _hashSeed = hashSeed;
            _atomicExpression = alternateExpression.GetAtomicExpression();

            HashSet<string> givenNames = alternateExpression.GetVariableNames();
            if (givenNames.Except(parameters).Any() || parameters.Except(givenNames).Any())
            {
                throw new MismatchedParametersException($"Parameters given and parameters named for function {name} do not match");
            }
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
