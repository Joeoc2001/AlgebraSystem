using Algebra.Operations;
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
        public Expression Map(EquationMapping.EquationMap map) => Map((EquationMapping)map);
        public abstract Expression Map(EquationMapping map);
        public abstract int GenHashCode();
        public abstract bool Equals(Expression obj);
        [Obsolete]
        public abstract string ToRunnableString();

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

        // Cache hash
        int? hash = null;
        public override sealed int GetHashCode()
        {
            if (!hash.HasValue)
            {
                hash = GenHashCode();
            }

            return hash.Value;
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as Expression);
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
            // a/b = a * (b^-1)
            return Multiply(new List<Expression>() { left, Pow(right, -1) });
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
            return SinOf(eq + (Constant.PI / 2));
        }

        public static Expression TanOf(Expression eq)
        {
            return SinOf(eq) / CosOf(eq);
        }

        public static Expression Abs(Expression eq)
        {
            return eq * SignOf(eq);
        }

        public static Expression Min(Expression a, Expression b)
        {
            return 0.5 * (a + b - Abs(a - b));
        }

        public static Expression Max(Expression a, Expression b)
        {
            return 0.5 * (a + b + Abs(a - b));
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
    }
}