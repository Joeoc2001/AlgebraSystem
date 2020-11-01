using Algebra.Atoms;
using Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Algebra.Equivalence
{
    public static class EquivalencePaths
    {
        // Polynomial
        public static readonly ReplacementPath ExpandBraces = new ReplacementPath("x * (y + z)", "x * y + x * z");
        public static readonly ReplacementPath FactorBraces = new ReplacementPath("x * y + x * z", "x * (y + z)");
        public static readonly ReplacementPath ExpandQuadratic = new ReplacementPath("(a + b) * (c + d)", "(a * c) + (b * c) + (a * d) + (b * d)");
        public static readonly ReplacementPath CompleteTheSquare = new ReplacementPath("a * (x ^ 2) + b * x + c", "a * (x + b / (2 * a)) ^ 2 - (b ^ 2) / 4 + c");
        

        private static readonly Regex _rx = new Regex(@"^([^=>]+?)=>([^=>]+?)$");
        public static List<EquivalencePath> ParseReplacementsString(string equivalencies)
        {
            List<EquivalencePath> paths = new List<EquivalencePath>();

            foreach (var equivalency in equivalencies.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(equivalency))
                {
                    continue;
                }

                MatchCollection matches = _rx.Matches(equivalency);
                List<Match> matchesList = new List<Match>();
                foreach (Match match in matches)
                {
                    matchesList.Add(match);
                }

                if (matchesList.Count != 1 || !matchesList[0].Success)
                {
                    throw new ArgumentException($"Equivalency {equivalency} is not of the form f=>g");
                }

                string expression1 = matchesList[0].Groups[0].Value;
                string expression2 = matchesList[0].Groups[1].Value;

                paths.Add(new ReplacementPath(expression1, expression2));
            }

            return paths;
        }
        public static readonly List<EquivalencePath> DefaultFunctionReplacementPaths = GenerateFunctionReplacementPaths(FunctionIdentitySet.DefaultFunctionIdentities);
        public static readonly List<EquivalencePath> DefaultAtomicPaths = new List<EquivalencePath>() {
            ExpandBraces,
            FactorBraces,
            ExpandQuadratic,
            CompleteTheSquare
        };
        public static readonly List<EquivalencePath> DefaultPaths = new List<EquivalencePath>(DefaultFunctionReplacementPaths.Concat(DefaultAtomicPaths));

        public static List<EquivalencePath> GenerateFunctionReplacementPaths(IEnumerable<FunctionIdentity> functions)
        {
            List<EquivalencePath> paths = new List<EquivalencePath>();

            foreach (FunctionIdentity identity in functions)
            {
                Expression atomicForm = identity.GetBodyAsAtomicExpression();
                Expression functionForm = identity.GetBodyAsFunctionExpression();
                paths.Add(new ReplacementPath(atomicForm, functionForm));
            }

            return paths;
        }
    }
}