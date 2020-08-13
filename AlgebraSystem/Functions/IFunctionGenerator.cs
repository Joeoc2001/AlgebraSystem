using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra.Functions
{
    public interface IFunctionGenerator
    {
        string GetName();
        Expression CreateExpression(Dictionary<string, Expression> parameters);
        ReadOnlyCollection<string> GetRequiredParameters();
        Expression CreateExpression(List<Expression> nodes);
    }
}
