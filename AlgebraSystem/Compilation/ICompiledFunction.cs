using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    public interface ICompiledFunction<T>
    {
        IVariableInputSet<T> GetVariableInputs();
        T Evaluate();
    }
}
