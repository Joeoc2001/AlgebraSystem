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

        public abstract ICompiledFunction<ReturnType> Compile(Expression expression, IVariableInputSet<ReturnType> variables, int simplificationAggressiveness = 3);
    }
}
