using Algebra;
using Algebra.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraTests
{
    class DummyExpression : Expression
    {
        public bool DerivativeGot { get => DerivativeGotCount > 0; }
        public int DerivativeGotCount { get; private set; } = 0;

        public bool OrderIndexGot { get => OrderIndexGotCount > 0; }
        public int OrderIndexGotCount { get; private set; } = 0;

        public bool GenHashCodeCalled { get => GenHashCodeCalledCount > 0; }
        public int GenHashCodeCalledCount { get; private set; } = 0;

        public bool GenAtomicExpressionCalled { get => GenAtomicExpressionCalledCount > 0; }
        public int GenAtomicExpressionCalledCount { get; private set; } = 0;

        public bool ToStringCalled { get => ToStringCalledCount > 0; }
        public int ToStringCalledCount { get; private set; } = 0;

        public bool EvaluateCalled { get => EvaluateCalledCount > 0; }
        public int EvaluateCalledCount { get; private set; } = 0;

        /// <summary>
        /// The expression used when <see cref="GetDerivative(Variable)"/> is called.
        /// If kept at default of null, a new dummy expression will be generated if <see cref="GetDerivative(Variable)"/> is called.
        /// </summary>
        public Expression Derivative { get; set; } = null;

        public override Expression GetDerivative(string wrt)
        {
            DerivativeGotCount += 1;
            if (Derivative is null)
            {
                Derivative = new DummyExpression();
            }
            return Derivative;
        }

        public override int GetOrderIndex()
        {
            OrderIndexGotCount += 1;
            return 0;
        }

        protected override int GenHashCode()
        {
            GenHashCodeCalledCount += 1;
            return 0;
        }

        protected override Expression GenAtomicExpression()
        {
            GenAtomicExpressionCalledCount += 1;
            return this;
        }

        public override string ToString()
        {
            ToStringCalledCount += 1;
            return base.ToString();
        }

        public override T Map<T>(IMapping<T> evaluator)
        {
            EvaluateCalledCount += 1;
            return default;
        }

        public override T Map<T>(IExtendedMapping<T> evaluator)
        {
            EvaluateCalledCount += 1;
            return default;
        }

        public override T Map<T>(Expression other, IDualMapping<T> evaluator)
        {
            EvaluateCalledCount += 1;
            return default;
        }
    }
}
