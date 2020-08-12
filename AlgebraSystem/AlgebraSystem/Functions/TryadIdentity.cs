using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Functions
{
    public class TryadIdentity : FunctionIdentity
    {
        public readonly Variable Parameter1;
        public readonly Variable Parameter2;
        public readonly Variable Parameter3;

        public delegate Expression.ExpressionDelegate GetTryadDelegateDelegate(Expression.ExpressionDelegate delegate1, Expression.ExpressionDelegate delegate2, Expression.ExpressionDelegate delegate3);
        public delegate Expression GetTryadDerivativeDelegate(Expression parameter1, Expression parameter2, Expression parameter3, Variable wrt);

        // Oh how I wish I was working in a functional language :(
        public TryadIdentity(Variable parameter1, Variable parameter2, Variable parameter3, int hashSeed, Expression atomicExpression, GetTryadDelegateDelegate getDelegate, GetTryadDerivativeDelegate getDerivative)
            : base(new List<string>() { parameter1.Name, parameter2.Name, parameter3.Name }, hashSeed, atomicExpression,
                  dels => getDelegate(dels[parameter1.Name], dels[parameter2.Name], dels[parameter3.Name]),
                  (exprs, wrt) => getDerivative(exprs[parameter1.Name], exprs[parameter2.Name], exprs[parameter3.Name], wrt))
        {
            Parameter1 = parameter1;
            Parameter2 = parameter2;
            Parameter3 = parameter3;
        }

        public Expression CreateExpression(Expression p1, Expression p2, Expression p3)
        {
            return CreateExpression(new Dictionary<string, Expression>()
            {
                { Parameter1.Name, p1 },
                { Parameter2.Name, p2 },
                { Parameter3.Name, p3 },
            });
        }
    }
}
