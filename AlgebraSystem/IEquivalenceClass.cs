using Algebra.Equivalence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IEquivalenceClass
    {
        bool IsInClass(IExpression queryExpression, int searchDepth = -1, List<EquivalencePath> paths = null);
    }
}
