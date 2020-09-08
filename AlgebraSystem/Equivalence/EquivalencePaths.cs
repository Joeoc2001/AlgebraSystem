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
        public static readonly ReplacementPath ExpandQuadratic = new ReplacementPath(
                (Expression.VarA + Expression.VarB) * (Expression.VarY + Expression.VarZ),
                Expression.VarA * Expression.VarY + Expression.VarA * Expression.VarZ + Expression.VarB * Expression.VarY + Expression.VarB * Expression.VarZ
            );

        public static readonly List<EquivalencePath> DefaultPaths = new List<EquivalencePath> {
            ExpandBraces,
            FactorBraces,
            ExpandQuadratic
        };
    }
}