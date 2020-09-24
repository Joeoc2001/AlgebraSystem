using Algebra;
using Algebra.Evaluators;
using Algebra.Functions;
using Libs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.EvaluatorsTests.DoubleEvaluators
{
    class FunctionEvaluatorsTest
    {
        private static readonly double[] inputs =
        {
            -1,
            0,
            1,
            2,
            double.NaN,
            double.NegativeInfinity,
            double.PositiveInfinity,
            double.MaxValue,
            double.MinValue
        };

        private static double CalculateTolerance(params double[] inputs)
        {
            double max = 0;
            foreach (var input in inputs)
            {
                if (double.IsNaN(input) || double.IsInfinity(input))
                {
                    continue;
                }
                max = Math.Abs(input) > max ? Math.Abs(input) : max;
            }

            return max * 0.00000000001f;
        }

        [Test, Pairwise]
        public void TestThat_FunctionEvaluator_ProducesSameResult_For([Values] MonadBuilder.Monad monad, [ValueSource(nameof(inputs))] double input)
        {
            // ARRANGE
            Expression expression = MonadBuilder.Build(Expression.VarA, monad);
            VariableInputSet<double> inputs = new VariableInputSet<double>() { { "a", input } };
            DoubleEvaluator evaluator1 = new DoubleEvaluator(inputs, DoubleEvaluator.DefaultFunctionEvaluators);
            DoubleEvaluator evaluator2 = new DoubleEvaluator(inputs, new Dictionary<FunctionIdentity, DoubleEvaluator.FunctionEvaluator>());

            // ACT
            double output1 = expression.Evaluate(evaluator1);
            double output2 = expression.Evaluate(evaluator2);

            // ASSERT
            Assert.AreEqual(output1, output2, CalculateTolerance(input));
        }

        [Test, Pairwise]
        public void TestThat_FunctionEvaluator_ProducesSameResult_For([Values] DyadBuilder.Dyad dyad, [ValueSource(nameof(inputs))] double input1, [ValueSource(nameof(inputs))] double input2)
        {
            // ARRANGE
            Expression expression = DyadBuilder.Build(Expression.VarA, Expression.VarB, dyad);
            VariableInputSet<double> inputs = new VariableInputSet<double>() { { "a", input1 }, { "b", input2 } };
            DoubleEvaluator evaluator1 = new DoubleEvaluator(inputs, DoubleEvaluator.DefaultFunctionEvaluators);
            DoubleEvaluator evaluator2 = new DoubleEvaluator(inputs, new Dictionary<FunctionIdentity, DoubleEvaluator.FunctionEvaluator>());

            // ACT
            double output1 = expression.Evaluate(evaluator1);
            double output2 = expression.Evaluate(evaluator2);

            // ASSERT
            Assert.AreEqual(output1, output2, CalculateTolerance(input1, input2));
        }

        [Test, Pairwise]
        public void TestThat_FunctionEvaluator_ProducesSameResult_For([Values] TryadBuilder.Tryad tryad, [ValueSource(nameof(inputs))] double input1, [ValueSource(nameof(inputs))] double input2, [ValueSource(nameof(inputs))] double input3)
        {
            // ARRANGE
            Expression expression = TryadBuilder.Build(Expression.VarA, Expression.VarB, Expression.VarC, tryad);
            VariableInputSet<double> inputs = new VariableInputSet<double>() { { "a", input1 }, { "b", input2 }, { "c", input3 } };
            DoubleEvaluator evaluator1 = new DoubleEvaluator(inputs, DoubleEvaluator.DefaultFunctionEvaluators);
            DoubleEvaluator evaluator2 = new DoubleEvaluator(inputs, new Dictionary<FunctionIdentity, DoubleEvaluator.FunctionEvaluator>());

            // ACT
            double output1 = expression.Evaluate(evaluator1);
            double output2 = expression.Evaluate(evaluator2);

            // ASSERT
            Assert.AreEqual(output1, output2, CalculateTolerance(input1, input2, input3));
        }
    }
}
