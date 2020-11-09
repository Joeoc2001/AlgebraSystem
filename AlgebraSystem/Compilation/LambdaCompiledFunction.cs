using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    internal class LambdaCompiledFunction<T> : ICompiledFunction<T>
    {
        private readonly Func<T[], T> _func;
        private readonly string[] _variables;

        public LambdaCompiledFunction(Func<T[], T> func, string[] variables)
        {
            _func = func;
            _variables = variables;
        }

        public T Evaluate(IVariableInputSet<T> variables)
        {
            T[] values = new T[_variables.Length];
            for (int i = 0; i < _variables.Length; i++)
            {
                values[i] = variables.Get(_variables[i]).Value;
            }

            return _func(values);
        }
    }
}
