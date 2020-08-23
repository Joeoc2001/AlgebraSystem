using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra
{
    public interface IFunctionGenerator
    {
        string GetName();
        ReadOnlyCollection<string> GetRequiredParameters();
        bool AreParametersSatisfied(IDictionary<string, Expression> parameters);
        Expression CreateExpression(IList<Expression> nodes);
        Expression CreateExpression(IDictionary<string, Expression> parameters);
    }
}