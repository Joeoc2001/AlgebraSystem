using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default
    {
        internal class DefaultCompiledFunction : ICompiledFunction<double>
        {
            private readonly IVariableInputSet<double> _variables;
            private readonly IDefaultInstruction[] _instructions;

            public DefaultCompiledFunction(IVariableInputSet<double> variables, IDefaultInstruction[] instructions)
            {
                _variables = variables;
                _instructions = instructions;
            }

            public double Evaluate()
            {
                Stack<double> stack = new Stack<double>();

                foreach (IDefaultInstruction instruction in _instructions)
                {
                    switch (instruction)
                    {
                        case DefaultLoadConst loadConst:
                            stack.Push(loadConst.Value);
                            break;
                        case DefaultLoadVar loadVar:
                            stack.Push(loadVar.Variable.Value);
                            break;
                        case DefaultInstruction opInstruction:
                            stack.Push(Evaluate(opInstruction.Opcode, stack));
                            break;
                    }
                }

                return stack.Pop();
            }

            private static double Evaluate(DefaultOpcode opcode, Stack<double> stack)
            {
                switch (opcode)
                {
                    case DefaultOpcode.SIN:
                        return Math.Sin(stack.Pop());
                    case DefaultOpcode.COS:
                        return Math.Cos(stack.Pop());
                    case DefaultOpcode.TAN:
                        return Math.Tan(stack.Pop());
                    case DefaultOpcode.ARCSIN:
                        return Math.Asin(stack.Pop());
                    case DefaultOpcode.ARCCOS:
                        return Math.Acos(stack.Pop());
                    case DefaultOpcode.ARCTAN:
                        return Math.Atan(stack.Pop());
                    case DefaultOpcode.SINH:
                        return Math.Sinh(stack.Pop());
                    case DefaultOpcode.COSH:
                        return Math.Cosh(stack.Pop());
                    case DefaultOpcode.TANH:
                        return Math.Tanh(stack.Pop());
                    case DefaultOpcode.ARSINH:
                        return Arsinh(stack.Pop());
                    case DefaultOpcode.ARCOSH:
                        return Arcosh(stack.Pop());
                    case DefaultOpcode.ARTANH:
                        return Artanh(stack.Pop());
                    case DefaultOpcode.EXPONENT:
                        return Math.Pow(stack.Pop(), stack.Pop());
                    case DefaultOpcode.LN:
                        return Math.Log(stack.Pop());
                    case DefaultOpcode.LOG:
                        return Math.Log(stack.Pop(), stack.Pop());
                    case DefaultOpcode.SQRT:
                        return Math.Sqrt(stack.Pop());
                    case DefaultOpcode.ADD:
                        return stack.Pop() + stack.Pop();
                    case DefaultOpcode.SUBTRACT:
                        return stack.Pop() - stack.Pop();
                    case DefaultOpcode.MULTIPLY:
                        return stack.Pop() * stack.Pop();
                    case DefaultOpcode.DIVIDE:
                        return stack.Pop() / stack.Pop();
                    case DefaultOpcode.SIGN:
                        return Math.Sign(stack.Pop());
                    case DefaultOpcode.ABS:
                        return Math.Abs(stack.Pop());
                    case DefaultOpcode.MIN:
                        return Math.Min(stack.Pop(), stack.Pop());
                    case DefaultOpcode.MAX:
                        return Math.Max(stack.Pop(), stack.Pop());
                    case DefaultOpcode.SELECT:
                        return UtilityMethods.Select(stack.Pop(), stack.Pop(), stack.Pop());
                    default:
                        throw new NotImplementedException($"Invalid opcode {opcode}");
                }
            }

            private static double Arsinh(double a)
            {
                return Math.Log(a + Math.Sqrt(a * a + 1));
            }

            private static double Arcosh(double a)
            {
                return Math.Log(a + Math.Sqrt(a * a - 1));
            }

            private static double Artanh(double a)
            {
                return 0.5 * Math.Log((a + 1) / (a - 1));
            }

            public IVariableInputSet<double> GetVariableInputs()
            {
                return _variables;
            }
        }
    }
}
