using Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IFunctionIdentity : IFunctionGenerator
    {
        IAtomicExpression GetBodyAsAtomicExpression();
        int GetHashSeed();
    }
}
