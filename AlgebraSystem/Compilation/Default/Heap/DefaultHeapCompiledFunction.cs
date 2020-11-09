using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Heap
    {
        internal class DefaultHeapCompiledFunction : ICompiledFunction<double>
        {
            private readonly DefaultHeapInstruction[] _instructions;
            private readonly int _heapSize;
            private readonly Dictionary<int, string> _variableNames;

            public DefaultHeapCompiledFunction(DefaultHeapInstruction[] instructions, int heapSize, Dictionary<int, string> variableNames)
            {
                _instructions = instructions;
                _heapSize = heapSize;
                _variableNames = variableNames;
            }

            public double Evaluate(IVariableInputSet<double> variables)
            {
                double[] heap = new double[_heapSize];
                double result = 0;

                foreach (DefaultHeapInstruction instruction in _instructions)
                {
                    double arg1 = heap[instruction.Data.Arg_1];
                    double arg2 = heap[instruction.Data.Arg_2];
                    switch (instruction.Opcode)
                    {
                        case DefaultOpcode.VARIABLE:
                            result = variables.Get(_variableNames[instruction.Data.Arg_1]).Value;
                            break;
                        case DefaultOpcode.CONSTANT:
                            result = instruction.Data.Value;
                            break;
                        case DefaultOpcode.SIN:
                            result = Math.Sin(arg1);
                            break;
                        case DefaultOpcode.COS:
                            result = Math.Cos(arg1);
                            break;
                        case DefaultOpcode.TAN:
                            result = Math.Tan(arg1);
                            break;
                        case DefaultOpcode.ARCSIN:
                            result = Math.Asin(arg1);
                            break;
                        case DefaultOpcode.ARCCOS:
                            result = Math.Acos(arg1);
                            break;
                        case DefaultOpcode.ARCTAN:
                            result = Math.Atan(arg1);
                            break;
                        case DefaultOpcode.SINH:
                            result = Math.Sinh(arg1);
                            break;
                        case DefaultOpcode.COSH:
                            result = Math.Cosh(arg1);
                            break;
                        case DefaultOpcode.TANH:
                            result = Math.Tanh(arg1);
                            break;
                        case DefaultOpcode.ARSINH:
                            result = UtilityMethods.Arsinh(arg1);
                            break;
                        case DefaultOpcode.ARCOSH:
                            result = UtilityMethods.Arcosh(arg1);
                            break;
                        case DefaultOpcode.ARTANH:
                            result = UtilityMethods.Artanh(arg1);
                            break;
                        case DefaultOpcode.EXPONENT:
                            result = Math.Pow(arg1, arg2);
                            break;
                        case DefaultOpcode.LN:
                            result = Math.Log(arg1);
                            break;
                        case DefaultOpcode.LOG:
                            result = Math.Log(arg1, arg2);
                            break;
                        case DefaultOpcode.SQRT:
                            result = Math.Sqrt(arg1);
                            break;
                        case DefaultOpcode.ADD:
                            result = arg1 + arg2;
                            break;
                        case DefaultOpcode.SUBTRACT:
                            result = arg1 - arg2;
                            break;
                        case DefaultOpcode.MULTIPLY:
                            result = arg1 * arg2;
                            break;
                        case DefaultOpcode.DIVIDE:
                            result = arg1 / arg2;
                            break;
                        case DefaultOpcode.SIGN:
                            result = Math.Sign(arg1);
                            break;
                        case DefaultOpcode.ABS:
                            result = Math.Abs(arg1);
                            break;
                        case DefaultOpcode.MIN:
                            result = Math.Min(arg1, arg2);
                            break;
                        case DefaultOpcode.MAX:
                            result = Math.Max(arg1, arg2);
                            break;
                        default:
                            throw new NotImplementedException($"Cannot execute instruction {instruction.Opcode}");
                    }

                    heap[instruction.Dest] = result;
                }

                return result;
            }
        }
    }
}
