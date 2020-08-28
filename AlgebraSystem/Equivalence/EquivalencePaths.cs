using Algebra.Atoms;
using System.Collections.Generic;

namespace Algebra.Equivalence
{
    public static class EquivalencePaths
    {
        public static readonly ReplacementPath ExpandBraces = new ReplacementPath(
                Expression.VarX * (Expression.VarY + Expression.VarZ),
                Expression.VarX * Expression.VarY + Expression.VarX * Expression.VarZ
            );
        public static readonly ReplacementPath FactorBraces = new ReplacementPath(
                Expression.VarX * Expression.VarY + Expression.VarX * Expression.VarZ,
                Expression.VarX * (Expression.VarY + Expression.VarZ)
            );
    }
}