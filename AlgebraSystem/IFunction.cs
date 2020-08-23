using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra
{
    public interface IFunction
    {
        IFunctionIdentity GetIdentity();
        ReadOnlyCollection<Expression> GetParameterList();
        ReadOnlyDictionary<string, Expression> GetParameters();
        Expression GetAtomicExpression();
    }
}
