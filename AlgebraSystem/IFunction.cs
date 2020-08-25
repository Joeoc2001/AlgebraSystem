using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Algebra
{
    public interface IFunction : IExpression, IEquatable<IFunction>
    {
        IFunctionIdentity GetIdentity();
        ReadOnlyCollection<IExpression> GetParameterList();
        ReadOnlyDictionary<string, IExpression> GetParameters();
    }
}
