using Algebra;
using Algebra.Equivalence;
using System;
using System.Collections.Generic;
using Algebra.Parsing;
using System.Xml.Schema;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            int lengths = 400;

            Expression expression = "tanh(max(x + y, x * y)) + arctan(min(x + y, x * y))";
            double time, sum;

            /*(time, sum) = Time(lengths, (x, y, z) => expression.EvaluateOnce(x, y, z));
            Console.WriteLine($"Execute Once Avg Time: {time} ns");
            Console.WriteLine($"Value: {sum}");*/

            VariableInputSet<double> variableInputs = new VariableInputSet<double>() { { "x", 0 }, { "y", 0 } };
            VariableInput<double> xInput = variableInputs.Get("x");
            VariableInput<double> yInput = variableInputs.Get("y");
            var stackCompiled = expression.Compile(Expression.CompilationMethod.Stack, 3);
            var heapCompiled = expression.Compile(Expression.CompilationMethod.Heap, 3);
            var lambdaHeapCompiled = expression.Compile(Expression.CompilationMethod.LambdaHeap, 3);

            /*(time, sum) = Time(lengths, (x, y, z) => { xInput.Value = x; yInput.Value = y; return stackCompiled.Evaluate(variableInputs); });
            Console.WriteLine($"Compiled stack Avg Time: {time} ns");
            Console.WriteLine($"Value: {sum}");*/

            /*(time, sum) = Time(lengths, (x, y, z) => { xInput.Value = x; yInput.Value = y; return heapCompiled.Evaluate(variableInputs); });
            Console.WriteLine($"Compiled heap Avg Time: {time} ns");
            Console.WriteLine($"Value: {sum}");*/

            (time, sum) = Time(lengths, (x, y, z) => { xInput.Value = x; yInput.Value = y; return lambdaHeapCompiled.Evaluate(variableInputs); });
            Console.WriteLine($"Compiled lambda heap Avg Time: {time} ns");
            Console.WriteLine($"Value: {sum}");

            /*(time, sum) = Time(lengths, (x, y, z) => Math.Tanh(Math.Max(x + y, x * y)) + Math.Atan(Math.Min(x + y, x * y)));
            Console.WriteLine($"Native Time: {time} ns");
            Console.WriteLine($"Value: {sum}");*/
        }

        private static (double, double) Time(int range, Func<int, int, int, double> func)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            int c = 0;
            double sum = 0;
            for (int x = 0; x < range; x++)
            {
                for (int y = 0; y < range; y++)
                {
                    for (int z = 0; z < range; z++)
                    {
                        sum += func(x, y, z);
                        c += 1;
                    }
                }
            }
            watch.Stop();
            double time = (double)(watch.ElapsedMilliseconds * 1000000) / c;
            return (time, sum);
        }
    }
}
