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
            private readonly string[] _variableNames;

            public DefaultStackCompiledFunction(IDefaultStackInstruction[] instructions, string[] variables)
            {
                _instructions = instructions;
                _variableNames = variables;

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
                double arg0;
                double arg1;
                double arg2;
                switch (opcode)
                {
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
                    case DefaultOpcode.LN:
                    case DefaultOpcode.SQRT:
                    case DefaultOpcode.SIGN:
                    case DefaultOpcode.ABS:
                        arg0 = stack.Pop();
                        return EvaluateMonad(opcode, arg0);
                    case DefaultOpcode.EXPONENT:
                    case DefaultOpcode.LOG:
                    case DefaultOpcode.ADD:
                    case DefaultOpcode.SUBTRACT:
                    case DefaultOpcode.MULTIPLY:
                    case DefaultOpcode.DIVIDE:
                    case DefaultOpcode.MIN:
                    case DefaultOpcode.MAX:
                        arg1 = stack.Pop();
                        arg0 = stack.Pop();
                        return EvaluateDyad(opcode, arg0, arg1);
                    case DefaultOpcode.SELECT:
                        arg2 = stack.Pop();
                        arg1 = stack.Pop();
                        arg0 = stack.Pop();
                        return EvaluateTryad(opcode, arg0, arg1, arg2);
                    default:
                        throw new NotImplementedException($"Invalid opcode {opcode}");
                }
            }

            private static double EvaluateMonad(DefaultOpcode opcode, double arg0)
            {
                switch (opcode)
                {
                    case DefaultOpcode.SIN:
                        return Math.Sin(arg0);
                    case DefaultOpcode.COS:
                        return Math.Cos(arg0);
                    case DefaultOpcode.TAN:
                        return Math.Tan(arg0);
                    case DefaultOpcode.ARCSIN:
                        return Math.Asin(arg0);
                    case DefaultOpcode.ARCCOS:
                        return Math.Acos(arg0);
                    case DefaultOpcode.ARCTAN:
                        return Math.Atan(arg0);
                    case DefaultOpcode.SINH:
                        return Math.Sinh(arg0);
                    case DefaultOpcode.COSH:
                        return Math.Cosh(arg0);
                    case DefaultOpcode.TANH:
                        return Math.Tanh(arg0);
                    case DefaultOpcode.ARSINH:
                        return Arsinh(arg0);
                    case DefaultOpcode.ARCOSH:
                        return Arcosh(arg0);
                    case DefaultOpcode.ARTANH:
                        return Artanh(arg0);
                    case DefaultOpcode.LN:
                        return Math.Log(arg0);
                    case DefaultOpcode.SQRT:
                        return Math.Sqrt(arg0);
                    case DefaultOpcode.SIGN:
                        return Math.Sign(arg0);
                    case DefaultOpcode.ABS:
                        return Math.Abs(arg0);
                    default:
                        throw new NotImplementedException($"Invalid opcode {opcode}");
                }
            }

            private static double EvaluateDyad(DefaultOpcode opcode, double arg0, double arg1)
            {
                switch (opcode)
                {
                    case DefaultOpcode.EXPONENT:
                        return Math.Pow(arg0, arg1);
                    case DefaultOpcode.LOG:
                        return Math.Log(arg0, arg1);
                    case DefaultOpcode.ADD:
                        return arg0 + arg1;
                    case DefaultOpcode.SUBTRACT:
                        return arg0 - arg1;
                    case DefaultOpcode.MULTIPLY:
                        return arg0 * arg1;
                    case DefaultOpcode.DIVIDE:
                        return arg0 / arg1;
                    case DefaultOpcode.MIN:
                        return Math.Min(arg0, arg1);
                    case DefaultOpcode.MAX:
                        return Math.Max(arg0, arg1);
                    default:
                        throw new NotImplementedException($"Invalid opcode {opcode}");
                }
            }

            private static double EvaluateTryad(DefaultOpcode opcode, double arg0, double arg1, double arg2)
            {
                switch (opcode)
                {
                    case DefaultOpcode.SELECT:
                        return UtilityMethods.Select(arg0, arg1, arg2);
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

            public string[] GetParameterOrdering()
            {
                return _variableNames;
            }

            public double Evaluate(params double[] variables)
            {
                VariableInputSet<double> input = new VariableInputSet<double>();
                for (int i = 0; i < variables.Length; i++)
                {
                    input.Add(_variableNames[i], variables[i]);
                }
                return Evaluate(input);
            }
        }
    }
}
