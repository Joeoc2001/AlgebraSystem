using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    internal class CompileEvaluator<Intermediate, Compiled> : IExpandedEvaluator<ICompiledFunctionBuilder<Intermediate, Compiled>>
    {
    }
}
