using Algebra.Operations;
using Rationals;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Algebra
{
    public abstract class Equation : IEquatable<Equation>
    {
        public static implicit operator Equation(int r) => (Constant)r;
        public static implicit operator Equation(long r) => (Constant)r;
        public static implicit operator Equation(float r) => (Constant)r;
        public static implicit operator Equation(double r) => (Constant)r;
        public static implicit operator Equation(decimal r) => (Constant)r;
        public static implicit operator Equation(Rational r) => (Constant)r;

        public delegate float ExpressionDelegate();
        public delegate Vector2 Vector2ExpressionDelegate();
        public delegate Vector3 Vector3ExpressionDelegate();
        public delegate Vector4 Vector4ExpressionDelegate();

        public abstract ExpressionDelegate GetExpression(VariableInputSet set);
        public abstract Equation GetDerivative(Variable wrt);
        public Equation Map(EquationMapping.EquationMap map) => Map((EquationMapping)map);
        public abstract Equation Map(EquationMapping map);
        public abstract int GenHashCode();
        public abstract bool Equals(Equation obj);
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
            return Equals(obj as Equation);
        }

        public ExpressionDelegate GetDerivitiveExpression(VariableInputSet set, Variable wrt)
        {
            return GetDerivative(wrt).GetExpression(set);
        }

        public Vector2ExpressionDelegate GetDerivitiveExpressionWrtXY(VariableInputSet set)
        {
            ExpressionDelegate dxFunc = GetDerivative(Variable.X).GetExpression(set);
            ExpressionDelegate dyFunc = GetDerivative(Variable.Y).GetExpression(set);
            return () => new Vector2(dxFunc(), dyFunc());
        }

        public Vector3ExpressionDelegate GetDerivitiveExpressionWrtXYZ(VariableInputSet set)
        {
            ExpressionDelegate dxFunc = GetDerivative(Variable.X).GetExpression(set);
            ExpressionDelegate dyFunc = GetDerivative(Variable.Y).GetExpression(set);
            ExpressionDelegate dzFunc = GetDerivative(Variable.Z).GetExpression(set);
            return () => new Vector3(dxFunc(), dyFunc(), dzFunc());
        }

        public Vector4ExpressionDelegate GetDerivitiveExpressionWrtXYZW(VariableInputSet set)
        {
            ExpressionDelegate dxFunc = GetDerivative(Variable.X).GetExpression(set);
            ExpressionDelegate dyFunc = GetDerivative(Variable.Y).GetExpression(set);
            ExpressionDelegate dzFunc = GetDerivative(Variable.Z).GetExpression(set);
            ExpressionDelegate dwFunc = GetDerivative(Variable.W).GetExpression(set);
            return () => new Vector4(dxFunc(), dyFunc(), dzFunc(), dwFunc());
        }

        public static Equation operator +(Equation left, Equation right)
        {
            return Add(new List<Equation>() { left, right });
        }

        public static Equation operator -(Equation left, Equation right)
        {
            return Add(new List<Equation>() { left, -1 * right });
        }

        public static Equation Add(List<Equation> eqs)
        {
            return Sum.Add(eqs);
        }

        public static Equation operator *(Equation left, Equation right)
        {
            return Multiply(new List<Equation>() { left, right });
        }

        public static Equation operator /(Equation left, Equation right)
        {
            // a/b = a * (b^-1)
            return Multiply(new List<Equation>() { left, Pow(right, -1) });
        }

        public static Equation Multiply(List<Equation> eqs)
        {
            return Product.Multiply(eqs);
        }

        public static Equation Pow(Equation left, Equation right)
        {
            return Exponent.Pow(left, right);
        }

        public static Equation LnOf(Equation eq)
        {
            return Ln.LnOf(eq);
        }

        public static Equation SignOf(Equation eq)
        {
            return Sign.SignOf(eq);
        }

        public static Equation SinOf(Equation eq)
        {
            return Sin.SinOf(eq);
        }

        public static Equation CosOf(Equation eq)
        {
            return SinOf(eq + (Constant.PI / 2));
        }

        public static Equation TanOf(Equation eq)
        {
            return SinOf(eq) / CosOf(eq);
        }

        public static Equation Abs(Equation eq)
        {
            return eq * SignOf(eq);
        }

        public static Equation Min(Equation a, Equation b)
        {
            return 0.5 * (a + b - Abs(a - b));
        }

        public static Equation Max(Equation a, Equation b)
        {
            return 0.5 * (a + b + Abs(a - b));
        }

        public bool ShouldParenthesise(Equation other)
        {
            return this.GetOrderIndex() <= other.GetOrderIndex();
        }
        protected string ToParenthesisedString(Equation child)
        {
            if (ShouldParenthesise(child))
            {
                return $"({child})";
            }

            return child.ToString();
        }

        public static bool operator ==(Equation left, Equation right)
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

        public static bool operator !=(Equation left, Equation right)
        {
            return !(left == right);
        }
    }
}