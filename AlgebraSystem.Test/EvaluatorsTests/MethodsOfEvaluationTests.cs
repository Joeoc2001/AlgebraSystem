using Algebra;
using Libs;
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
            { "(x + y) / 2", (x, y) => (x + y) / 2 },
            { "x * y + y", (x, y) => x * y + y },
            { "x ^ y", (x, y) => Math.Pow(x, y) },
            { "max(x + y, x * y)", (x, y) => Math.Max(x + y, x * y) },
            { "log(x + 2, x * y * -10)", (x, y) => Math.Log(x + 2, x * y * -10) },
            { "tanh(max(x + y, x * y)) + arctan(min(x + y, x * y))", (x, y) => Math.Tanh(Math.Max(x + y, x * y)) + Math.Atan(Math.Min(x + y, x * y)) },
            { "tanh(max(cos(x) + y, x * sin(y))) - arctan(min(x + cos(y), sin(x) * y))", (x, y) => Math.Tanh(Math.Max(Math.Cos(x) + y, x * Math.Sin(y))) + Math.Atan(Math.Min(x + Math.Cos(y), Math.Sin(x) * y)) },
        };

        private static readonly double[] inputs =
        {
            -1,
            0,
            1,
            2,
            Math.PI,
            double.MaxValue / 2,
            double.MinValue / 2
        };

        public static IEnumerable<object[]> GenTestCases()
        {
            foreach (Expression.CompilationMethod method in Enum.GetValues(typeof(Expression.CompilationMethod)))
            {
                foreach (var item in expecteds)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            yield return new object[] { method, item.Key, x, y, item.Value };
                        }
                    }
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(GenTestCases))]
        public static void TestThat_Method_Returns_SameAsNative_For(Expression.CompilationMethod method, Expression expr, int x, int y, Func<int, int, double> func)
        {
            // Arrange
            double expected = func(x, y);

            // Act
            double got = expr.Compile(new List<string>() { "x", "y" }, method).Evaluate(x, y);

            // Assert
            Assert.AreEqual(expected, got);
        }

        [Test]
        public void TestThat_Method_ProducesSameResultAsEvaluateOnce_For([Values] Expression.CompilationMethod method, [Values] MonadBuilder.Monad monad, [ValueSource(nameof(inputs))] double input)
        {
            // ARRANGE
            Expression expression = MonadBuilder.Build(Expression.VarX, monad);

            // ACT
            double expected = expression.EvaluateOnce(input);
            double got = expression.Compile(new List<string>() { "x" }, method).Evaluate(input);

            // ASSERT
            Assert.AreEqual(expected, got);
        }

        [Test]
        public void TestThat_Method_ProducesSameResultAsEvaluateOnce_For([Values] Expression.CompilationMethod method, [Values] DyadBuilder.Dyad dyad, [ValueSource(nameof(inputs))] double input1, [ValueSource(nameof(inputs))] double input2)
        {
            // ARRANGE
            Expression expression = DyadBuilder.Build(Expression.VarX, Expression.VarY, dyad);

            // ACT
            double expected = expression.EvaluateOnce(input1, input2);
            double got = expression.Compile(new List<string>() { "x", "y" }, method).Evaluate(input1, input2);

            // ASSERT
            Assert.AreEqual(expected, got);
        }

        [Test]
        public void TestThat_Method_ProducesSameResultAsEvaluateOnce_For([Values] Expression.CompilationMethod method, [Values] TryadBuilder.Tryad tryad, [ValueSource(nameof(inputs))] double input1, [ValueSource(nameof(inputs))] double input2, [ValueSource(nameof(inputs))] double input3)
        {
            // ARRANGE
            Expression expression = TryadBuilder.Build(Expression.VarX, Expression.VarY, Expression.VarZ, tryad);

            // ACT
            double expected = expression.EvaluateOnce(input1, input2, input3);
            double got = expression.Compile(new List<string>() { "x", "y", "z" }, method).Evaluate(input1, input2, input3);

            // ASSERT
            Assert.AreEqual(expected, got);
        }
    }
}
