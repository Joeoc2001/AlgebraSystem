using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default
    {
        internal enum DefaultOpcode
        {
            SIN, COS, TAN,
            ARCSIN, ARCCOS, ARCTAN,
            SINH, COSH, TANH,
            ARSINH, ARCOSH, ARTANH,
            EXPONENT, LN, LOG, SQRT,
            ADD, SUBTRACT, MULTIPLY, DIVIDE,
            SIGN, ABS, MIN, MAX, SELECT
        }
    }
}
