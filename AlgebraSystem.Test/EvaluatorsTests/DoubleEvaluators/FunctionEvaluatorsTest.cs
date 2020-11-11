using Algebra;
using Algebra.Functions;
using Algebra.Mappings;
using Libs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.EvaluatorsTests.DoubleMappings
{
    class FunctionEvaluatorsTest
    {
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

        [Test]
        public void TestThat_FunctionEvaluator_ProducesSameResult_For([Values] MonadBuilder.Monad monad, [ValueSource(nameof(inputs))] double input)
        {
            // ARRANGE
            Expression expression = MonadBuilder.Build(Expression.VarA, monad);
            VariableInputSet<double> inputs = new VariableInputSet<double>() { { "a", input } };
            DoubleMapping evaluator1 = new DoubleMapping(inputs, new Dictionary<FunctionIdentity, DoubleMapping.FunctionMapping>());
            DoubleMapping evaluator2 = new DoubleMapping(inputs, DoubleMapping.DefaultFunctionMappings);

            // ACT
            double output1 = expression.Map(evaluator1);
            double output2 = expression.Map(evaluator2);

            // ASSERT
            Assert.AreEqual(output1, output2, CalculateTolerance(input));
        }

        [Test]
        public void TestThat_FunctionEvaluator_ProducesSameResult_For([Values] DyadBuilder.Dyad dyad, [ValueSource(nameof(inputs))] double input1, [ValueSource(nameof(inputs))] double input2)
        {
            // ARRANGE
            Expression expression = DyadBuilder.Build(Expression.VarA, Expression.VarB, dyad);
            VariableInputSet<double> inputs = new VariableInputSet<double>() { { "a", input1 }, { "b", input2 } };
            DoubleMapping evaluator1 = new DoubleMapping(inputs, new Dictionary<FunctionIdentity, DoubleMapping.FunctionMapping>());
            DoubleMapping evaluator2 = new DoubleMapping(inputs, DoubleMapping.DefaultFunctionMappings);

            // ACT
            double output1 = expression.Map(evaluator1);
            double output2 = expression.Map(evaluator2);

            // ASSERT
            Assert.AreEqual(output1, output2, CalculateTolerance(input1, input2));
        }

        [Test]
        public void TestThat_FunctionEvaluator_ProducesSameResult_For([Values] TryadBuilder.Tryad tryad, [ValueSource(nameof(inputs))] double input1, [ValueSource(nameof(inputs))] double input2, [ValueSource(nameof(inputs))] double input3)
        {
            // ARRANGE
            Expression expression = TryadBuilder.Build(Expression.VarA, Expression.VarB, Expression.VarC, tryad);
            VariableInputSet<double> inputs = new VariableInputSet<double>() { { "a", input1 }, { "b", input2 }, { "c", input3 } };
            DoubleMapping evaluator1 = new DoubleMapping(inputs, new Dictionary<FunctionIdentity, DoubleMapping.FunctionMapping>());
            DoubleMapping evaluator2 = new DoubleMapping(inputs, DoubleMapping.DefaultFunctionMappings);

            // ACT
            double output1 = expression.Map(evaluator1);
            double output2 = expression.Map(evaluator2);

            // ASSERT
            Assert.AreEqual(output1, output2, CalculateTolerance(input1, input2, input3));
        }
    }
}
