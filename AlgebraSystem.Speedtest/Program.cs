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

            Expression expression = "tanh(max(x + y, x * y)) + arctan(min(x + y, x * y))";
            double time;

            time = Time(lengths, (x, y, z) => expression.EvaluateOnce(x, y, z));
            Console.WriteLine($"Execute Once Avg Time: {time} ns");

            VariableInputSet<double> variableInputs = new VariableInputSet<double>() { { "x", 0 }, { "y", 0 } };
            VariableInput<double> xInput = variableInputs.Get("x");
            VariableInput<double> yInput = variableInputs.Get("y");
            var compiled = expression.Compile(variableInputs, 3);

            time = Time(lengths, (x, y, z) => { xInput.Value = x; yInput.Value = y; return compiled.Evaluate(); });
            Console.WriteLine($"Compiled Avg Time: {time} ns");

            time = Time(lengths, (x, y, z) => Math.Tanh(Math.Max(x + y, x * y)) + Math.Atan(Math.Min(x + y, x * y)));
            Console.WriteLine($"Native Time: {time} ns");
        }

        private static double Time(int range, Func<int, int, int, double> func)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            int c = 0;
            for (int x = 0; x < range; x++)
            {
                for (int y = 0; y < range; y++)
                {
                    for (int z = 0; z < range; z++)
                    {
                        func(x, y, z);
                        c += 1;
                    }
                }
            }
            watch.Stop();
            return (double)(watch.ElapsedMilliseconds * 1000000) / c;
        }
    }
}
