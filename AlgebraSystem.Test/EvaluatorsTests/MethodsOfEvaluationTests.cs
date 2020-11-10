using Algebra;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraSystem.Test.EvaluatorsTests
{
    class MethodsOfEvaluationTests
    {
        private static readonly Dictionary<Expression, Func<int, int, double>> expecteds = new Dictionary<Expression, Func<int, int, double>>()
        {
            { "0", (x, y) => 0 },
            { "1", (x, y) => 1 },
            { "x", (x, y) => x },
            { "y", (x, y) => y },
            { "y + 1", (x, y) => y + 1 },
            { "x + y", (x, y) => x + y },
            { "x * y", (x, y) => x * y },
            { "x - y", (x, y) => x - y },
            { "x / y", (x, y) => x / y },
            { "x * y + y", (x, y) => x * y + y },
            { "x ^ y", (x, y) => Math.Pow(x, y) },
            { "max(x + y, x * y)", (x, y) => Math.Max(x + y, x * y) },
            { "log(x + 2, x * y * -10)", (x, y) => Math.Log(x + 2, x * y * -10) },
            { "tanh(max(x + y, x * y)) + arctan(min(x + y, x * y))", (x, y) => Math.Tanh(Math.Max(x + y, x * y)) + Math.Atan(Math.Min(x + y, x * y)) },
            { "tanh(max(cos(x) + y, x * sin(y))) - arctan(min(x + cos(y), sin(x) * y))", (x, y) => Math.Tanh(Math.Max(Math.Cos(x) + y, x * Math.Sin(y))) + Math.Atan(Math.Min(x + Math.Cos(y), Math.Sin(x) * y)) },
        };

        public enum EvaluationMethod
        {
            DoubleEvaluator,
            CompiledStack,
            CompiledHeap,
            CompiledHeapLambda
        }

        public static IEnumerable<object[]> GenTestCases()
        {
            foreach (EvaluationMethod method in Enum.GetValues(typeof(EvaluationMethod)))
            {
                foreach (var item in expecteds)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            yield return new object[] { method, item.Key, item.Value, x, y };
                        }
                    }
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(GenTestCases))]
        public static void TestThat_Method_Returns_SameAsNative(EvaluationMethod method, Expression expr, Func<int, int, double> func, int x, int y)
        {
            // Arrange
            double expected = func(x, y);

            // Act
            double got = 0;
            switch (method)
            {
                case EvaluationMethod.DoubleEvaluator:
                    got = expr.EvaluateOnce(x, y);
                    break;
                case EvaluationMethod.CompiledStack:
                    got = expr.Compile(new List<string>() { "x", "y" }, Expression.CompilationMethod.Stack).Evaluate(x, y);
                    break;
                case EvaluationMethod.CompiledHeap:
                    got = expr.Compile(new List<string>() { "x", "y" }, Expression.CompilationMethod.Heap).Evaluate(x, y);
                    break;
                case EvaluationMethod.CompiledHeapLambda:
                    got = expr.Compile(new List<string>() { "x", "y" }, Expression.CompilationMethod.LambdaHeap).Evaluate(x, y);
                    break;
            }

            // Assert
            Assert.AreEqual(expected, got);
        }
    }
}
