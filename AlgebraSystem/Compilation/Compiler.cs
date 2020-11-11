using Algebra.Equivalence;
using Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Compilation
{
    public abstract class Compiler<ReturnType>
    {
        protected readonly ICollection<FunctionIdentity> _supportedFunctions;
        protected readonly IExpressionMetric _simplificationMetric;
        protected readonly List<EquivalencePath> _paths;

        public Compiler(ICollection<FunctionIdentity> supportedFunctions)
        {
            _supportedFunctions = supportedFunctions;
            _simplificationMetric = new DefaultSimplificationMetric(supportedFunctions);
            _paths = new List<EquivalencePath>(EquivalencePaths.DefaultAtomicPaths.Concat(EquivalencePaths.GenerateFunctionReplacementPaths(supportedFunctions)));
        }

        protected static IEnumerable<string> CompareVariables(IEnumerable<string> parameterOrdering, IEnumerable<string> foundVariables)
        {
            if (parameterOrdering == null)
            {
                return foundVariables.OrderBy(s => s);
            }

            if (foundVariables.Count() > parameterOrdering.Count()
                || foundVariables.Except(parameterOrdering).Any())
            {
                throw new ArgumentException($"Provided parameter order {string.Join(", ", parameterOrdering)} does not match found parameters {string.Join(", ", foundVariables)}");
            }
            return parameterOrdering;
        }

        public abstract ICompiledFunction<ReturnType> Compile(Expression expression, IEnumerable<string> parameterOrdering = null, int simplificationAggressiveness = 3);
    }
}
