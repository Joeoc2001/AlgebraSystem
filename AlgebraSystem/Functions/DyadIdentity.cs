using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions
    {
        internal class DyadIdentity : FunctionIdentity
        {
            public readonly Variable Parameter1;
            public readonly Variable Parameter2;

            public DyadIdentity(string name, Variable parameter1, Variable parameter2, int hashSeed, Expression atomicExpression)
                : base(name, new List<string>() { parameter1.Name, parameter2.Name }, hashSeed, atomicExpression)
            {
                Parameter1 = parameter1;
                Parameter2 = parameter2;
            }

            public Expression CreateExpression(Expression p1, Expression p2)
            {
                return CreateExpression(new Dictionary<string, Expression>()
                {
                    { Parameter1.Name, p1 },
                    { Parameter2.Name, p2 },
                });
            }
        }
    }
}
