using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    public interface IRedirectable<T>
    {
        T RedirectMemory(int[] redirections);
        T RedirectVariables(int[] redirections);
    }
}
