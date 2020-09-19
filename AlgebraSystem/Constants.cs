using Algebra.Atoms;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    internal class Constants
    {
        public static readonly IConstant PI = RealConstant.FromApproximation("PI", Rational.Approximate(Math.PI));
        public static readonly IConstant E = RealConstant.FromApproximation("PI", Rational.Approximate(Math.E));
    }
}
