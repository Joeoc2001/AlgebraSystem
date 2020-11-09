using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    public interface ICompiledFunction<T>
    {
        string[] GetParameterOrdering();
        T Evaluate(IVariableInputSet<T> variables);
        T Evaluate(params T[] variables);
    }
}
