using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions
    {
        internal class MonadIdentity : FunctionIdentity
        {
            public readonly Variable Parameter;

            public MonadIdentity(string name, Variable parameter, int hashSeed, Expression atomicExpression)
                : base(name, new List<string>() { parameter.Name }, hashSeed, atomicExpression)
            {
                Parameter = parameter;
            }

            public Expression CreateExpression(Expression expression)
            {
                return CreateExpression(new Dictionary<string, Expression>()
                {
                    { Parameter.Name, expression },
                });
            }
        }
    }
}