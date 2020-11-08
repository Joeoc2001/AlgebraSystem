using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Stack
    {
        internal class DefaultStackCompiledFunction : ICompiledFunction<double>
        {
            private readonly DefaultOpcode[] _codes; // Extract for speed
            private readonly IDefaultStackInstruction[] _instructions;
            private readonly int _maxStackDepth;

            public DefaultStackCompiledFunction(IDefaultStackInstruction[] instructions)
            {
                _instructions = instructions;

                _codes = _instructions.Select(i => i.Opcode).ToArray();

                _maxStackDepth = GetMaxStackDepth(instructions);
            }

            private static int GetMaxStackDepth(IDefaultStackInstruction[] _instructions)
            {
                int depth = 0;
                int maxDepth = 0;

                foreach (IDefaultStackInstruction instruction in _instructions)
                {
                    switch (instruction.Opcode)
                    {
                        case DefaultOpcode.VARIABLE:
                        case DefaultOpcode.CONSTANT:
                            depth += 1;
                            maxDepth = Math.Max(depth, maxDepth);
                            break;
                        case DefaultOpcode.SIN:
                        case DefaultOpcode.COS:
                        case DefaultOpcode.TAN:
                        case DefaultOpcode.ARCSIN:
                        case DefaultOpcode.ARCCOS:
                        case DefaultOpcode.ARCTAN:
                        case DefaultOpcode.SINH:
                        case DefaultOpcode.COSH:
                        case DefaultOpcode.TANH:
                        case DefaultOpcode.ARSINH:
                        case DefaultOpcode.ARCOSH:
                        case DefaultOpcode.ARTANH:
                        case DefaultOpcode.SQRT:
                        case DefaultOpcode.SIGN:
                        case DefaultOpcode.ABS:
                        case DefaultOpcode.LN:
                            continue;
                        case DefaultOpcode.EXPONENT:
                        case DefaultOpcode.LOG:
                        case DefaultOpcode.ADD:
                        case DefaultOpcode.SUBTRACT:
                        case DefaultOpcode.MULTIPLY:
                        case DefaultOpcode.DIVIDE:
                        case DefaultOpcode.MIN:
                        case DefaultOpcode.MAX:
                            depth -= 1;
                            break;
                        case DefaultOpcode.SELECT:
                            depth -= 2;
                            break;
                        default:
                            throw new NotImplementedException($"Invalid opcode {instruction.Opcode}");
                    }
                }

                return maxDepth;
            }


            public double Evaluate(IVariableInputSet<double> variables)
            {
                FastStack<double> stack = new FastStack<double>(_maxStackDepth);

                for (int i = 0; i < _codes.Length; i++)
                {
                    DefaultOpcode code = _codes[i];
                    switch (code)
                    {
                        case DefaultOpcode.CONSTANT:
                            DefaultStackLoadConst loadConst = (DefaultStackLoadConst)_instructions[i];
                            stack.Push(loadConst.Value);
                            break;
                        case DefaultOpcode.VARIABLE:
                            DefaultStackLoadVar loadVar = (DefaultStackLoadVar)_instructions[i];
                            stack.Push(variables.Get(loadVar.Name).Value);
                            break;
                        default:
                            stack.Push(Evaluate(code, ref stack));
                            break;
                    }
                }

                return stack.Pop();
            }

            private static double Evaluate(DefaultOpcode opcode, ref FastStack<double> stack)
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
        }
    }
}
