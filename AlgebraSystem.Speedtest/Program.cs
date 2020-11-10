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
            int lengths = 100;

            Expression expression = "tanh(max(cos(x) + y, x * sin(y))) / arctan(min(x + cos(y), sin(x) * y))";
            double time, sum;

            (time, sum) = Time(lengths, (x, y, z) => expression.EvaluateOnce(x, y, z));
            Console.WriteLine($"Execute Once Avg Time: {time} ns");
            Console.WriteLine($"Value: {sum}");
            
            var stackCompiled = expression.Compile(new List<string>(){ "x", "y" }, Expression.CompilationMethod.Stack, 3);
            var heapCompiled = expression.Compile(new List<string>() { "x", "y" }, Expression.CompilationMethod.Heap, 3);
            var lambdaHeapCompiled = expression.Compile(new List<string>() { "x", "y" }, Expression.CompilationMethod.LambdaHeap, 3);

            (time, sum) = Time(lengths, (x, y, z) => stackCompiled.Evaluate(x, y));
            Console.WriteLine($"Compiled stack Avg Time: {time} ns");
            Console.WriteLine($"Value: {sum}");

            (time, sum) = Time(lengths, (x, y, z) => heapCompiled.Evaluate(x, y));
            Console.WriteLine($"Compiled heap Avg Time: {time} ns");
            Console.WriteLine($"Value: {sum}");

            (time, sum) = Time(lengths, (x, y, z) => lambdaHeapCompiled.Evaluate(x, y) );
            Console.WriteLine($"Compiled lambda heap Avg Time: {time} ns");
            Console.WriteLine($"Value: {sum}");

            //(time, sum) = Time(lengths, (x, y, z) => Math.Tanh(Math.Max(x + y, x * y)) + Math.Atan(Math.Min(x + y, x * y)));
            //Console.WriteLine($"Native Time: {time} ns");
            //Console.WriteLine($"Value: {sum}");
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
