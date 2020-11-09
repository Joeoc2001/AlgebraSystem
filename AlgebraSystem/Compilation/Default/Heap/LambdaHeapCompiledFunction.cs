using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
                System.Linq.Expressions.ParameterExpression parameters = System.Linq.Expressions.Expression.Parameter(typeof(double[]));
                System.Linq.Expressions.Expression linqExpression = GenerateLinqExpression(instructions, heapSize, parameters);

                // Try to reduce
                while (linqExpression.CanReduce)
                {
                    linqExpression = linqExpression.ReduceAndCheck();
                }

                // Compile lambda
                System.Linq.Expressions.LambdaExpression lambda = System.Linq.Expressions.Expression.Lambda(linqExpression, true, parameters);  // TODO: Evaluate if tail call adds any performance due to lack of recusion
                Delegate compiled = lambda.Compile();
                Func<double[], double> func = (Func<double[], double>)(compiled); // Error is on this line :(

                return func;
            }

            private static System.Linq.Expressions.Expression GenerateLinqExpression(DefaultHeapInstruction[] instructions, int heapSize, System.Linq.Expressions.ParameterExpression parameters)
            {
                // Create heap
                System.Linq.Expressions.ParameterExpression[] heap = new System.Linq.Expressions.ParameterExpression[heapSize];
                for (int i = 0; i < heapSize; i++)
                {
                    heap[i] = System.Linq.Expressions.Expression.Variable(typeof(double), $"Heap_{i}");
                }

                // Process instructions
                List<System.Linq.Expressions.Expression> expressions = new List<System.Linq.Expressions.Expression>(instructions.Length);
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
                            System.Linq.Expressions.ParameterExpression arg1 = heap[instruction.Data.Arg_1];
                            System.Linq.Expressions.ParameterExpression arg2 = heap[instruction.Data.Arg_2];
                            result = GenerateLinqExpression(instruction.Opcode, arg1, arg2);
                            break;
                    }
                    System.Linq.Expressions.Expression assignExpr = System.Linq.Expressions.Expression.Assign(heap[instruction.Dest], result);
                    expressions.Add(assignExpr);
                }

                // Add return value to the end
                if (result == null)
                {
                    throw new ArgumentNullException(nameof(result));
                }
                expressions.Add(result);

                // Build block
                List<System.Linq.Expressions.ParameterExpression> variables = new List<System.Linq.Expressions.ParameterExpression>(heap) { parameters };
                return System.Linq.Expressions.Expression.Block(variables, expressions);
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

            private static readonly MethodInfo expMethod = typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) });
            private static readonly MethodInfo logMethod = typeof(Math).GetMethod("Log", new[] { typeof(double), typeof(double) });
            private static readonly MethodInfo minMethod = typeof(Math).GetMethod("Min", new[] { typeof(double), typeof(double) });
            private static readonly MethodInfo maxMethod = typeof(Math).GetMethod("Max", new[] { typeof(double), typeof(double) });

            private static System.Linq.Expressions.Expression GenerateLinqExpression(DefaultOpcode opcode, System.Linq.Expressions.ParameterExpression arg1, System.Linq.Expressions.ParameterExpression arg2)
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
                        return System.Linq.Expressions.Expression.Call(expMethod, arg1, arg2);
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
