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
        bool AreParametersSatisfied(IDictionary<string, IExpression> parameters);
        IExpression CreateExpression(IList<IExpression> nodes);
        IExpression CreateExpression(IDictionary<string, IExpression> parameters);
    }
}