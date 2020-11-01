using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    public class DefaultCompiler
    {
        private enum Opcode
        {
            CONSTANT, VARIABLE,
            SIN, COS, TAN,
            ARCSIN, ARCCOS, ARCTAN,
            SINH, COSH, TANH,
            ARSINH, ARCOSH, ARTANH,
            EXPONENT, LN, LOG, SQRT,
            ADD, SUBTRACT, MULTIPLY, DIVIDE,
            SIGN, ABS, MIN, MAX,
        }

        private struct Instruction
        {
            Opcode opcode;
            int operand1;
            int operand2;
        }
    }
}
