using Algebra.Evaluators;
using Rationals;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Algebra
{
    public interface IExpression : IComparable<IExpression>, IEquatable<IExpression>
    {
        IExpression GetDerivative(string wrt);
        T Evaluate<T>(IEvaluator<T> evaluator);
        T Evaluate<T>(IExpandedEvaluator<T> evaluator);
        T Evaluate<T>(IExpression secondary, IDualEvaluator<T> evaluator);

        public HashSet<string> GetVariables()
        {
            return Evaluate(GetVariablesEvaluator.Instance);
        }

        /* Used for displaying braces when printing a human-readable string
         * Should be:
         * 0 -> Node (eg. x, 12, function)
         * 10 -> Indeces
         * 20 -> Multiplication
         * 30 -> Addition
         * Used to determine order of operations
         * Less => Higher priority
         */
        int GetOrderIndex();
        int GetHashCode();

        /// <summary>
        /// Replaces all non-atomic operations with their atomic counterparts.
        /// The resulting expressions should not be executed as it will be far slower.
        /// Instead, this form is used as an intermediate form for performing simplifications.
        /// </summary>
        /// <returns>An expression in atomic form</returns>
        IExpression GetAtomicExpression();

        /// <summary>
        /// Checks if this expression is in atomic form, i.e. has no function calls. 
        /// </summary>
        /// <returns>True if this is atomic</returns>
        bool IsAtomic();

        Rational EvaluateOnce(VariableInputSet<Rational> variables)
        {
            return Evaluate(new RationalEvaluator(variables));
        }

        public float EvaluateOnce(float variable) => EvaluateOnce(new VariableInputSet<float>() { { "x", variable } });
        public float EvaluateOnce(Vector2 variable) => EvaluateOnce(new VariableInputSet<float>() { { "x", variable.X }, { "y", variable.Y } });
        public float EvaluateOnce(Vector3 variable) => EvaluateOnce(new VariableInputSet<float>() { { "x", variable.X }, { "y", variable.Y }, { "z", variable.Z } });
        public float EvaluateOnce(VariableInputSet<float> variables)
        {
            return Evaluate(new FloatEvaluator(variables));
        }

        /// <summary>
        /// Creates a new Equivalence Class used for proving equivalence and for finding alternate forms of an equation.
        /// </summary>
        /// <returns>A queriable equivalence class for this expression</returns>
        IEquivalenceClass GetEquivalenceClass();

        bool Equals(object obj);
        // new bool Equals(IExpression obj);

        /// <summary>
        /// Checks if an expression is equal to this on a variable level.
        /// Each <see cref="EqualityLevel"/> gives a different level of effort to put in to calculate equality.
        /// Note that the problem of expression equality is undecidable, so with the deepest setting it is not guaranteed that this method will terminate.
        /// This method will however terminate on all other levels.
        /// Also note that a return of false does not ever guarantee that two expressions are not equal, however a return of true guarantees that they are equal.
        /// </summary>
        /// <param name="e">The expression to check against this</param>
        /// <param name="level">The level of effort to put in to calculate equality</param>
        /// <returns>True if the equations are equal, false if equality could not be proven, or if e is null.</returns>
        bool Equals(IExpression e, EqualityLevel level);

        public static IExpression operator +(IExpression left, IExpression right)
        {
            return Expression.Add(new List<IExpression>() { left, right });
        }
        public static IExpression operator +(double left, IExpression right) => Expression.ConstantFrom(left) + right;
        public static IExpression operator +(IExpression left, double right) => left + Expression.ConstantFrom(right);

        public static IExpression operator -(IExpression left, IExpression right)
        {
            return Expression.Add(new List<IExpression>() { left, -right });
        }
        public static IExpression operator -(double left, IExpression right) => Expression.ConstantFrom(left) - right;
        public static IExpression operator -(IExpression left, double right) => left - Expression.ConstantFrom(right);

        public static IExpression operator -(IExpression a)
        {
            return Expression.MinusOne * a;
        }

        public static IExpression operator *(IExpression left, IExpression right)
        {
            return Expression.Multiply(new List<IExpression>() { left, right });
        }
        public static IExpression operator *(double left, IExpression right) => Expression.ConstantFrom(left) * right;
        public static IExpression operator *(IExpression left, double right) => left * Expression.ConstantFrom(right);

        public static IExpression operator /(IExpression left, IExpression right)
        {
            return Expression.Divide(left, right);
        }
        public static IExpression operator /(double left, IExpression right) => Expression.ConstantFrom(left) / right;
        public static IExpression operator /(IExpression left, double right) => left / Expression.ConstantFrom(right);
    }
}