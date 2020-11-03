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
            int lengths = 1000;

            Expression expression = "tanh(max(x + y, x * y)) + arsinh(min(x + y, x * y))";
            int c = 0;
            double sum = 0;

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (int x = 0; x < lengths; x++)
            {
                for (int y = 0; y < lengths; y++)
                {
                    sum += expression.EvaluateOnce(x, y);
                    c += 1;
                }
            }
            watch.Stop();
            Console.WriteLine($"Value: {sum}");
            Console.WriteLine($"Execute Once Avg Time: {(double)watch.ElapsedMilliseconds / c} ms");

            watch.Restart();
            VariableInputSet<double> variableInputs = new VariableInputSet<double>() { { "x", 0 }, { "y", 0 } };
            VariableInput<double> xInput = variableInputs.Get("x");
            VariableInput<double> yInput = variableInputs.Get("y");
            var compiled = expression.Compile(variableInputs, 3);
            watch.Stop();
            Console.WriteLine($"Compile time: {watch.ElapsedMilliseconds} ms");

            watch.Restart();
            c = 0;
            sum = 0;
            for (int x = 0; x < lengths; x++)
            {
                for (int y = 0; y < lengths; y++)
                {
                    xInput.Value = x;
                    yInput.Value = y;
                    sum += compiled.Evaluate();
                    c += 1;
                }
            }
            watch.Stop();
            Console.WriteLine($"Value: {sum}");
            Console.WriteLine($"Compiled Avg Time: {(double)watch.ElapsedMilliseconds / c} ms");
        }
    }
}
