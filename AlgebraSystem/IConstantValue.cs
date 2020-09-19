using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IConstantValue : IEquatable<IConstantValue>
    {
        bool IsRational();
        Rational GetRationalApproximation();
        double GetDoubleApproximation();
        string ToString();
    }
}
