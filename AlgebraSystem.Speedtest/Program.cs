using Algebra;
using Algebra.Equivalence;
using System;
using System.Collections.Generic;
using Algebra.Parsing;
using System.Xml.Schema;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace AlgebraSystem.Speedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            int lengths = 100;

            Algebra.Expression expression = "tanh(cos(x) * y + x * sin(y)) * arctan(x * cos(y) + sin(x) * y)";
            var symbolicsExpr = Expr.Parse("tanh(cos(x) * y + x * sin(y)) * atan(x * cos(y) + sin(x) * y)");

            TimeEvaluator(lengths, (x, y, z) => symbolicsExpr.Evaluate(new Dictionary<string, FloatingPoint>() {{"x", x}, {"y", y}}).RealValue, "Symbolics Execute Once");
            TimeEvaluator(lengths, (x, y, z) => expression.EvaluateOnce(x, y, z), "Execute Once");

            var lambdaHeapCompiled = expression.Compile("x", "y");
            Func<double, double, double> symbolicsCompiled = symbolicsExpr.Compile("x", "y");

            TimeEvaluator(lengths, (x, y, z) => symbolicsCompiled(x, y), "Symbolics Compiled");
            //TimeEvaluator(lengths, (x, y, z) => stackCompiled.Evaluate(x, y), "Stack Compiled");
            //TimeEvaluator(lengths, (x, y, z) => heapCompiled.Evaluate(x, y), "Heap Compiled");
            TimeEvaluator(lengths, (x, y, z) => lambdaHeapCompiled.Evaluate(x, y), "Compiled");
            TimeEvaluator(lengths, (x, y, z) => Math.Tanh((Math.Cos(x) * y) + (x * Math.Sin(y))) * Math.Atan((x * Math.Cos(y)) + (Math.Sin(x) * y)), "Native");
        }

        private static void TimeEvaluator(int range, Func<double, double, double, double> func, string name)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (double x = 1; x <= range; x++)
            {
                for (double y = 1; y <= range; y++)
                {
                    for (double z = 1; z <= range; z++)
                    {
                        func(x, y, z);
                    }
                }
            }
            watch.Stop();
            double time = (double)(watch.ElapsedMilliseconds * 1000000) / (range * range * range);

            Console.WriteLine($"{name} Avg Time: {time} ns");
            //Console.WriteLine($"Value: {sum}");
        }
    }
}
