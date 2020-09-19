using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IVariable : IEquatable<IVariable>
    {
        string GetName();
        Expression ToExpression();
    }
}
