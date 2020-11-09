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
            private readonly string[] _variableNames;

            public DefaultHeapCompiledFunction(DefaultHeapInstruction[] instructions, int heapSize, string[] variableNames)
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
                    DefaultHeapInstruction.DataUnion data = instruction.Data;
                    switch (instruction.Opcode)
                    {
                        case DefaultOpcode.VARIABLE:
                            result = variables.Get(_variableNames[data.Arg_1]).Value;
                            break;
                        case DefaultOpcode.CONSTANT:
                            result = data.Value;
                            break;
                        default:
                            double arg1 = heap[data.Arg_1];
                            double arg2 = heap[data.Arg_2];
                            result = EvaluateOperation(instruction.Opcode, arg1, arg2);
                            break;
                    }

                    heap[instruction.Dest] = result;
                }

                return result;
            }

            private static double EvaluateOperation(DefaultOpcode opcode, double arg1, double arg2)
            {
                switch (opcode)
                {
                    case DefaultOpcode.SIN:
                        return Math.Sin(arg1);
                    case DefaultOpcode.COS:
                        return Math.Cos(arg1);
                    case DefaultOpcode.TAN:
                        return Math.Tan(arg1);
                    case DefaultOpcode.ARCSIN:
                        return Math.Asin(arg1);
                    case DefaultOpcode.ARCCOS:
                        return Math.Acos(arg1);
                    case DefaultOpcode.ARCTAN:
                        return Math.Atan(arg1);
                    case DefaultOpcode.SINH:
                        return Math.Sinh(arg1);
                    case DefaultOpcode.COSH:
                        return Math.Cosh(arg1);
                    case DefaultOpcode.TANH:
                        return Math.Tanh(arg1);
                    case DefaultOpcode.ARSINH:
                        return UtilityMethods.Arsinh(arg1);
                    case DefaultOpcode.ARCOSH:
                        return UtilityMethods.Arcosh(arg1);
                    case DefaultOpcode.ARTANH:
                        return UtilityMethods.Artanh(arg1);
                    case DefaultOpcode.EXPONENT:
                        return Math.Pow(arg1, arg2);
                    case DefaultOpcode.LN:
                        return Math.Log(arg1);
                    case DefaultOpcode.LOG:
                        return Math.Log(arg1, arg2);
                    case DefaultOpcode.SQRT:
                        return Math.Sqrt(arg1);
                    case DefaultOpcode.ADD:
                        return arg1 + arg2;
                    case DefaultOpcode.SUBTRACT:
                        return arg1 - arg2;
                    case DefaultOpcode.MULTIPLY:
                        return arg1 * arg2;
                    case DefaultOpcode.DIVIDE:
                        return arg1 / arg2;
                    case DefaultOpcode.SIGN:
                        return Math.Sign(arg1);
                    case DefaultOpcode.ABS:
                        return Math.Abs(arg1);
                    case DefaultOpcode.MIN:
                        return Math.Min(arg1, arg2);
                    case DefaultOpcode.MAX:
                        return Math.Max(arg1, arg2);
                    default:
                        throw new NotImplementedException($"Cannot execute instruction {opcode}");
                }
            }
        }
    }
}
