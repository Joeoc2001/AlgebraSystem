using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    namespace Functions
    {
        internal class TryadIdentity : FunctionIdentity
        {
            public readonly Variable Parameter1;
            public readonly Variable Parameter2;
            public readonly Variable Parameter3;

            public TryadIdentity(string name, Variable parameter1, Variable parameter2, Variable parameter3, int hashSeed, IExpression atomicExpression)
                : base(name, new List<string>() { parameter1.Name, parameter2.Name, parameter3.Name }, hashSeed, atomicExpression)
            {
                Parameter1 = parameter1;
                Parameter2 = parameter2;
                Parameter3 = parameter3;
            }

            public IExpression CreateExpression(IExpression p1, IExpression p2, IExpression p3)
            {
                return CreateExpression(new Dictionary<string, IExpression>()
                {
                    { Parameter1.Name, p1 },
                    { Parameter2.Name, p2 },
                    { Parameter3.Name, p3 },
                });
            }
        }
    }
}
