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

            public MonadIdentity(string name, Variable parameter, int hashSeed, IExpression atomicExpression)
                : base(name, new List<string>() { parameter.Name }, hashSeed, atomicExpression)
            {
                Parameter = parameter;
            }

            public IExpression CreateExpression(IExpression expression)
            {
                return CreateExpression(new Dictionary<string, IExpression>()
                {
                    { Parameter.Name, expression },
                });
            }
        }
    }
}