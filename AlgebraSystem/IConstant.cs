using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IConstant : IEquatable<IConstant>, IComparable<IConstant>
    {
        bool IsRational();
        Rational GetRationalApproximation();
        double GetDoubleApproximation();
        string ToString();
        Expression ToExpression();
    }
}
