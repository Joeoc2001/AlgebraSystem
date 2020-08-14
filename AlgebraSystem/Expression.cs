using Algebra.Atoms;
using Algebra.Functions;
using Algebra.Functions.HardcodedFunctionIdentities;
using Rationals;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Algebra
{
    public abstract class Expression : IEquatable<Expression>
    {
        public static implicit operator Expression(int r) => (Constant)r;
        public static implicit operator Expression(long r) => (Constant)r;
        public static implicit operator Expression(float r) => (Constant)r;
        public static implicit operator Expression(double r) => (Constant)r;
        public static implicit operator Expression(decimal r) => (Constant)r;
        public static implicit operator Expression(Rational r) => (Constant)r;

        public delegate float ExpressionDelegate();
        public delegate Vector2 Vector2ExpressionDelegate();
        public delegate Vector3 Vector3ExpressionDelegate();
        public delegate Vector4 Vector4ExpressionDelegate();

        public abstract ExpressionDelegate GetDelegate(VariableInputSet set);
        public abstract Expression GetDerivative(Variable wrt);
        public Expression Map(ExpressionMapping.ExpressionMap map) => Map((ExpressionMapping)map);
        public abstract Expression Map(ExpressionMapping map);
        protected abstract int GenHashCode();
        protected abstract bool ExactlyEquals(Expression expression);

        /* Used for displaying braces when printing a human-readable string
         * Should be:
         * 0 -> Node (eg. x, 12, function)
         * 10 -> Indeces
         * 20 -> Multiplication
         * 30 -> Addition
         * Used to determine order of operations
         * Less => Higher priority
         */
        public abstract int GetOrderIndex();

        // Cache commonly queried results
        private int? hash = null;
        private Expression atomicExpression = null;

        public override sealed int GetHashCode()
        {
            if (!hash.HasValue)
            {
                hash = GenHashCode();
            }
            return hash.Value;
        }

        /// <summary>
        /// Replaces all non-atomic operations with their atomic counterparts.
        /// The resulting expressions should not be executed as it will be far slower.
        /// Instead, this form is used as an intermediate form for performing simplifications.
        /// This function is cached, so is O(1) after first invocation
        /// </summary>
        /// <returns>An expression in atomic form</returns>
        public Expression GetAtomicExpression()
        {
            if (atomicExpression is null)
            {
                atomicExpression = GenAtomicExpression();
            }
            return atomicExpression;
        }

        /// <summary>
        /// See <see cref="GetAtomicExpression"/>.
        /// Functions should override this method
        /// </summary>
        /// <returns>An expression in atomic form</returns>
        protected virtual Expression GenAtomicExpression()
        {
            bool primary = true;

            // Replace types with themselves but with their children's atomic expressions
            Expression atomicExpression = Map(new ExpressionMapping()
            {
                ShouldMapChildren = expression => primary,
                ShouldMapThis = expression => !primary,
                PostMap = expression =>
                {
                    if (primary)
                    {
                        return expression;
                    }
                    else
                    {
                        return expression.GetAtomicExpression();
                    }
                }
            });

            return atomicExpression;
        }

        public ExpressionDelegate GetDerivitiveExpression(VariableInputSet set, Variable wrt)
        {
            return GetDerivative(wrt).GetDelegate(set);
        }

        public Vector2ExpressionDelegate GetDerivitiveExpressionWrtXY(VariableInputSet set)
        {
            ExpressionDelegate dxFunc = GetDerivative(Variable.X).GetDelegate(set);
            ExpressionDelegate dyFunc = GetDerivative(Variable.Y).GetDelegate(set);
            return () => new Vector2(dxFunc(), dyFunc());
        }

        public Vector3ExpressionDelegate GetDerivitiveExpressionWrtXYZ(VariableInputSet set)
        {
            ExpressionDelegate dxFunc = GetDerivative(Variable.X).GetDelegate(set);
            ExpressionDelegate dyFunc = GetDerivative(Variable.Y).GetDelegate(set);
            ExpressionDelegate dzFunc = GetDerivative(Variable.Z).GetDelegate(set);
            return () => new Vector3(dxFunc(), dyFunc(), dzFunc());
        }

        public Vector4ExpressionDelegate GetDerivitiveExpressionWrtXYZW(VariableInputSet set)
        {
            ExpressionDelegate dxFunc = GetDerivative(Variable.X).GetDelegate(set);
            ExpressionDelegate dyFunc = GetDerivative(Variable.Y).GetDelegate(set);
            ExpressionDelegate dzFunc = GetDerivative(Variable.Z).GetDelegate(set);
            ExpressionDelegate dwFunc = GetDerivative(Variable.W).GetDelegate(set);
            return () => new Vector4(dxFunc(), dyFunc(), dzFunc(), dwFunc());
        }

        public static Expression operator +(Expression left, Expression right)
        {
            return Add(new List<Expression>() { left, right });
        }

        public static Expression operator -(Expression left, Expression right)
        {
            return Add(new List<Expression>() { left, -1 * right });
        }

        public static Expression operator -(Expression a)
        {
            return -1 * a;
        }

        public static Expression Add(List<Expression> eqs)
        {
            return Sum.Add(eqs);
        }

        public static Expression operator *(Expression left, Expression right)
        {
            return Multiply(new List<Expression>() { left, right });
        }

        public static Expression operator /(Expression left, Expression right)
        {
            return DivIdentity.Instance.CreateExpression(left, right);
        }

        public static Expression Multiply(List<Expression> eqs)
        {
            return Product.Multiply(eqs);
        }

        public static Expression Pow(Expression left, Expression right)
        {
            return Exponent.Pow(left, right);
        }

        public static Expression LnOf(Expression eq)
        {
            return Ln.LnOf(eq);
        }

        public static Expression LogOf(Expression a, Expression b)
        {
            return LogIdentity.Instance.CreateExpression(a, b);
        }

        public static Expression SignOf(Expression eq)
        {
            return Sign.SignOf(eq);
        }

        public static Expression SinOf(Expression eq)
        {
            return Sin.SinOf(eq);
        }

        public static Expression CosOf(Expression eq)
        {
            return CosIdentity.Instance.CreateExpression(eq);
        }

        public static Expression TanOf(Expression eq)
        {
            return TanIdentity.Instance.CreateExpression(eq);
        }

        public static Expression Abs(Expression eq)
        {
            return AbsIdentity.Instance.CreateExpression(eq);
        }

        public static Expression Min(Expression a, Expression b)
        {
            return MinIdentity.Instance.CreateExpression(a, b);
        }

        public static Expression Max(Expression a, Expression b)
        {
            return MaxIdentity.Instance.CreateExpression(a, b);
        }

        public static Expression SelectOn(Expression lt, Expression gt, Expression condition)
        {
            return SelectIdentity.Instance.CreateExpression(lt, gt, condition);
        }

        public static Expression SinhOf(Expression a)
        {
            return SinhIdentity.Instance.CreateExpression(a);
        }

        public static Expression CoshOf(Expression a)
        {
            return CoshIdentity.Instance.CreateExpression(a);
        }

        public static Expression TanhOf(Expression a)
        {
            return TanhIdentity.Instance.CreateExpression(a);
        }

        public bool ShouldParenthesise(Expression other)
        {
            return this.GetOrderIndex() <= other.GetOrderIndex();
        }
        protected string ToParenthesisedString(Expression child)
        {
            if (ShouldParenthesise(child))
            {
                return $"({child})";
            }

            return child.ToString();
        }

        public static bool operator ==(Expression left, Expression right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Expression left, Expression right)
        {
            return !(left == right);
        }

        // Equality methods
        /// <summary>
        /// Default object equality method.
        /// If obj is an Expression, checks equality on the atomic level
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>True if obj is an Expression and has the same atomic representation as this</returns>
        public sealed override bool Equals(object obj)
        {
            return Equals(obj as Expression);
        }

        /// <summary>
        /// Default Expression equality method.
        /// Checks equality on the atomic level
        /// </summary>
        /// <param name="e">The expression to check</param>
        /// <returns>True if the expression has the same atomic representation as this</returns>
        public bool Equals(Expression e)
        {
            return Equals(e, EqualityLevel.Atomic);
        }

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
        public bool Equals(Expression e, EqualityLevel level)
        {
            if (e is null)
            {
                return false;
            }

            switch (level)
            {
                case EqualityLevel.Exactly:
                    // Check hash first
                    if (GetHashCode() != e.GetHashCode())
                    {
                        return false;
                    }
                    return ExactlyEquals(e);
                case EqualityLevel.Atomic:
                    // Get atomic expressions and check if they are equal on the mimimum level
                    Expression atomicA = this.GetAtomicExpression();
                    Expression atomicB = e.GetAtomicExpression();
                    return atomicA.Equals(atomicB, EqualityLevel.Exactly);
                case EqualityLevel.Deep:
                    throw new NotImplementedException();
                case EqualityLevel.Deepest:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException($"Unknown equality level: {level}");
            }
        }
    }
}