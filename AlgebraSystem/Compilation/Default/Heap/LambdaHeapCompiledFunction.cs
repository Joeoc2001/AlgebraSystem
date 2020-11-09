using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Heap
    {
        internal class LambdaHeapCompiledFunction : LambdaCompiledFunction<double>
        {
            public LambdaHeapCompiledFunction(DefaultHeapInstruction[] instructions, int heapSize, string[] variableNames)
                : base(GenerateFunction(instructions, heapSize, variableNames), variableNames)
            {
            }

            private static Func<double[], double> GenerateFunction(DefaultHeapInstruction[] instructions, int heapSize, string[] variables)
            {
                // Create LINQ expression
                System.Linq.Expressions.ParameterExpression parameters = System.Linq.Expressions.Expression.Parameter(typeof(double[]), "vars");
                System.Linq.Expressions.Expression linqExpression = GenerateLinqExpression(instructions, heapSize, parameters);

                // Try to reduce
                while (linqExpression.CanReduce)
                {
                    linqExpression = linqExpression.ReduceAndCheck();
                }

                // Compile lambda
                System.Linq.Expressions.Expression<Func<double[], double>> lambda = System.Linq.Expressions.Expression.Lambda<Func<double[], double>>(linqExpression, true, parameters);  // TODO: Evaluate if tail call adds any performance due to lack of recusion
                Func<double[], double> func = lambda.Compile(); 

                return func;
            }

            private static System.Linq.Expressions.Expression GenerateLinqExpression(DefaultHeapInstruction[] instructions, int heapSize, System.Linq.Expressions.ParameterExpression parameters)
            {
                // Create heap
                System.Linq.Expressions.Expression[] heap = new System.Linq.Expressions.Expression[heapSize];

                // Process instructions
                System.Linq.Expressions.Expression result = null;
                foreach (DefaultHeapInstruction instruction in instructions)
                {
                    switch (instruction.Opcode)
                    {
                        case DefaultOpcode.VARIABLE:
                            System.Linq.Expressions.Expression index = System.Linq.Expressions.Expression.Constant(instruction.Data.Arg_1, typeof(int));
                            result = System.Linq.Expressions.Expression.ArrayIndex(parameters, index);
                            break;
                        case DefaultOpcode.CONSTANT:
                            result = System.Linq.Expressions.Expression.Constant(instruction.Data.Value, typeof(double));
                            break;
                        default:
                            System.Linq.Expressions.Expression arg1 = heap[instruction.Data.Arg_1];
                            System.Linq.Expressions.Expression arg2 = heap[instruction.Data.Arg_2];
                            result = GenerateLinqExpression(instruction.Opcode, arg1, arg2);
                            break;
                    }
                    heap[instruction.Dest] = result;
                }

                // Return last executed instruction
                if (result == null)
                {
                    throw new ArgumentNullException(nameof(result));
                }

                return result;
            }

            // TODO: Clean this up into a data structure (dict?)
            private static readonly MethodInfo sinMethod = typeof(Math).GetMethod("Sin", new[] { typeof(double) });
            private static readonly MethodInfo cosMethod = typeof(Math).GetMethod("Cos", new[] { typeof(double) });
            private static readonly MethodInfo tanMethod = typeof(Math).GetMethod("Tan", new[] { typeof(double) });
            private static readonly MethodInfo asinMethod = typeof(Math).GetMethod("Asin", new[] { typeof(double) });
            private static readonly MethodInfo acosMethod = typeof(Math).GetMethod("Acos", new[] { typeof(double) });
            private static readonly MethodInfo atanMethod = typeof(Math).GetMethod("Atan", new[] { typeof(double) });
            private static readonly MethodInfo sinhMethod = typeof(Math).GetMethod("Sinh", new[] { typeof(double) });
            private static readonly MethodInfo coshMethod = typeof(Math).GetMethod("Cosh", new[] { typeof(double) });
            private static readonly MethodInfo tanhMethod = typeof(Math).GetMethod("Tanh", new[] { typeof(double) });
            private static readonly MethodInfo asinhMethod = typeof(UtilityMethods).GetMethod("Arsinh", new[] { typeof(double) });
            private static readonly MethodInfo acoshMethod = typeof(UtilityMethods).GetMethod("Arcosh", new[] { typeof(double) });
            private static readonly MethodInfo atanhMethod = typeof(UtilityMethods).GetMethod("Artanh", new[] { typeof(double) });
            private static readonly MethodInfo sqrtMethod = typeof(Math).GetMethod("Sqrt", new[] { typeof(double) });
            private static readonly MethodInfo lnMethod = typeof(Math).GetMethod("Log", new[] { typeof(double) });
            private static readonly MethodInfo signMethod = typeof(Math).GetMethod("Sign", new[] { typeof(double) });
            private static readonly MethodInfo absMethod = typeof(Math).GetMethod("Abs", new[] { typeof(double) });

            private static readonly MethodInfo logMethod = typeof(Math).GetMethod("Log", new[] { typeof(double), typeof(double) });
            private static readonly MethodInfo minMethod = typeof(Math).GetMethod("Min", new[] { typeof(double), typeof(double) });
            private static readonly MethodInfo maxMethod = typeof(Math).GetMethod("Max", new[] { typeof(double), typeof(double) });

            private static System.Linq.Expressions.Expression GenerateLinqExpression(DefaultOpcode opcode, System.Linq.Expressions.Expression arg1, System.Linq.Expressions.Expression arg2)
            {
                switch (opcode)
                {
                    case DefaultOpcode.SIN:
                        return System.Linq.Expressions.Expression.Call(sinMethod, arg1);
                    case DefaultOpcode.COS:
                        return System.Linq.Expressions.Expression.Call(cosMethod, arg1);
                    case DefaultOpcode.TAN:
                        return System.Linq.Expressions.Expression.Call(tanMethod, arg1);
                    case DefaultOpcode.ARCSIN:
                        return System.Linq.Expressions.Expression.Call(asinMethod, arg1);
                    case DefaultOpcode.ARCCOS:
                        return System.Linq.Expressions.Expression.Call(acosMethod, arg1);
                    case DefaultOpcode.ARCTAN:
                        return System.Linq.Expressions.Expression.Call(atanMethod, arg1);
                    case DefaultOpcode.SINH:
                        return System.Linq.Expressions.Expression.Call(sinhMethod, arg1);
                    case DefaultOpcode.COSH:
                        return System.Linq.Expressions.Expression.Call(coshMethod, arg1);
                    case DefaultOpcode.TANH:
                        return System.Linq.Expressions.Expression.Call(tanhMethod, arg1);
                    case DefaultOpcode.ARSINH:
                        return System.Linq.Expressions.Expression.Call(asinhMethod, arg1);
                    case DefaultOpcode.ARCOSH:
                        return System.Linq.Expressions.Expression.Call(acoshMethod, arg1);
                    case DefaultOpcode.ARTANH:
                        return System.Linq.Expressions.Expression.Call(atanhMethod, arg1);
                    case DefaultOpcode.EXPONENT:
                        return System.Linq.Expressions.Expression.Power(arg1, arg2);
                    case DefaultOpcode.LN:
                        return System.Linq.Expressions.Expression.Call(lnMethod, arg1);
                    case DefaultOpcode.LOG:
                        return System.Linq.Expressions.Expression.Call(logMethod, arg1, arg2);
                    case DefaultOpcode.SQRT:
                        return System.Linq.Expressions.Expression.Call(sqrtMethod, arg1);
                    case DefaultOpcode.ADD:
                        return System.Linq.Expressions.Expression.Add(arg1, arg2);
                    case DefaultOpcode.SUBTRACT:
                        return System.Linq.Expressions.Expression.Subtract(arg1, arg2);
                    case DefaultOpcode.MULTIPLY:
                        return System.Linq.Expressions.Expression.Multiply(arg1, arg2);
                    case DefaultOpcode.DIVIDE:
                        return System.Linq.Expressions.Expression.Divide(arg1, arg2);
                    case DefaultOpcode.SIGN:
                        return System.Linq.Expressions.Expression.Call(signMethod, arg1);
                    case DefaultOpcode.ABS:
                        return System.Linq.Expressions.Expression.Call(absMethod, arg1);
                    case DefaultOpcode.MIN:
                        return System.Linq.Expressions.Expression.Call(minMethod, arg1, arg2);
                    case DefaultOpcode.MAX:
                        return System.Linq.Expressions.Expression.Call(maxMethod, arg1, arg2);
                    default:
                        throw new NotImplementedException($"Cannot execute instruction {opcode}");
                }
            }
        }
    }
}
