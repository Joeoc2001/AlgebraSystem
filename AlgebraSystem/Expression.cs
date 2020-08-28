using Algebra.Atoms;
using Algebra.Equivalence;
using Algebra.Evaluators;
using Algebra.Functions.HardcodedFunctionIdentities;
using Rationals;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Algebra
{
    public abstract class Expression : IExpression
    {
        public static implicit operator Expression(int r) => (Constant)r;
        public static implicit operator Expression(long r) => (Constant)r;
        public static implicit operator Expression(float r) => (Constant)r;
        public static implicit operator Expression(double r) => (Constant)r;
        public static implicit operator Expression(decimal r) => (Constant)r;
        public static implicit operator Expression(Rational r) => (Constant)r;

        public static readonly Expression VarA = VariableFrom("a");
        public static readonly Expression VarB = VariableFrom("b");
        public static readonly Expression VarC = VariableFrom("c");
        public static readonly Expression VarX = VariableFrom("x");
        public static readonly Expression VarY = VariableFrom("y");
        public static readonly Expression VarZ = VariableFrom("z");

        public static readonly Expression Zero = 0;
        public static readonly Expression One = 1;
        public static readonly Expression MinusOne = -1;
        public static readonly Expression PI = Math.PI;
        public static readonly Expression E = Math.E;

        public static Expression ConstantFrom(Rational value)
        {
            return Constant.FromValue(value);
        }

        public static Expression ConstantFrom(float value) => ConstantFrom((Rational)value);
        public static Expression ConstantFrom(int value) => ConstantFrom((Rational)value);
        public static Expression ConstantFrom(double value) => ConstantFrom((Rational)value);
        public static Expression ConstantFrom(decimal value) => ConstantFrom((Rational)value);
        public static Expression ConstantFrom(long value) => ConstantFrom((Rational)value);
        public static Expression ConstantFrom(BigInteger value) => ConstantFrom((Rational)value);

        public static Expression VariableFrom(string name)
        {
            return new Variable(name);
        }

        public abstract IExpression GetDerivative(string wrt);
        protected abstract int GenHashCode();
        protected abstract bool ExactlyEquals(IExpression expression);
        public abstract T Evaluate<T>(IEvaluator<T> evaluator);
        public abstract T Evaluate<T>(IExpandedEvaluator<T> evaluator);
        public abstract T Evaluate<T>(IExpression other, IDualEvaluator<T> evaluator);
        public abstract int GetOrderIndex();

        private int? _hashCode = null;
        public override int GetHashCode()
        {
            if (!_hashCode.HasValue)
            {
                _hashCode = GenHashCode();
            }
            return _hashCode.Value;
        }

        public bool Equals(IExpression e, EqualityLevel level)
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
                    IExpression atomicA = this.GetAtomicExpression();
                    IExpression atomicB = e.GetAtomicExpression();
                    return atomicA.Equals(atomicB, EqualityLevel.Exactly);
                case EqualityLevel.Deep:
                    return GetEquivalenceClass().IsInClass(e, 3);
                case EqualityLevel.Deepest:
                    return GetEquivalenceClass().IsInClass(e);
                default:
                    throw new NotImplementedException($"Unknown equality level: {level}");
            }
        }

        protected abstract IAtomicExpression GenAtomicExpression();
        private IAtomicExpression _atomicExpression = null;
        public IAtomicExpression GetAtomicExpression()
        {
            if (_atomicExpression is null)
            {
                _atomicExpression = GenAtomicExpression();
            }
            return _atomicExpression;
        }

        public static bool ShouldParenthesise(IExpression parent, IExpression child)
        {
            return parent.GetOrderIndex() <= child.GetOrderIndex();
        }

        public static string ToParenthesisedString(IExpression parent, IExpression child)
        {
            if (ShouldParenthesise(parent, child))
            {
                return $"({child})";
            }

            return child.ToString();
        }

        public IEquivalenceClass GetEquivalenceClass()
        {
            return new EquivalenceClass(this);
        }

        public bool IsAtomic()
        {
            return Evaluate(IsAtomicEvaluator.Instance);
        }

        public int CompareTo(IExpression other)
        {
            return Evaluate(other, GetOrderingDualEvaluator.Instance);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IExpression);
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

        public static IExpression operator +(Expression left, IExpression right) => (IExpression)left + right;
        public static IExpression operator +(IExpression left, Expression right) => left + (IExpression)right;
        public static IExpression operator +(Expression left, Expression right) => (IExpression)left + (IExpression)right;

        public static IExpression operator -(Expression left, IExpression right) => (IExpression)left - right;
        public static IExpression operator -(IExpression left, Expression right) => left - (IExpression)right;
        public static IExpression operator -(Expression left, Expression right) => (IExpression)left - (IExpression)right;

        public static IExpression operator -(Expression a) => MinusOne * a;

        public static IExpression operator *(Expression left, IExpression right) => (IExpression)left * right;
        public static IExpression operator *(IExpression left, Expression right) => left * (IExpression)right;
        public static IExpression operator *(Expression left, Expression right) => (IExpression)left * (IExpression)right;

        public static IExpression operator /(Expression left, IExpression right) => (IExpression)left / right;
        public static IExpression operator /(IExpression left, Expression right) => left / (IExpression)right;
        public static IExpression operator /(Expression left, Expression right) => (IExpression)left / (IExpression)right;

        public static IExpression Add<T>(IEnumerable<T> eqs) where T : IExpression
        {
            return Sum.Add(eqs);
        }

        public static IExpression Multiply<T>(IEnumerable<T> eqs) where T : IExpression
        {
            return Product.Multiply(eqs);
        }

        public static IExpression Pow(Expression left, Expression right) => Pow((IExpression)left, (IExpression)right);
        public static IExpression Pow(Expression left, IExpression right) => Pow((IExpression)left, right);
        public static IExpression Pow(IExpression left, Expression right) => Pow(left, (IExpression)right);
        public static IExpression Pow(IExpression left, IExpression right)
        {
            return Exponent.Pow(left, right);
        }

        public static IExpression Sqrt(Expression eq) => Sqrt((IExpression)eq);
        public static IExpression Sqrt(IExpression expression)
        {
            return SqrtIdentity.Instance.CreateExpression(expression);
        }

        public static IExpression LnOf(Expression eq) => LnOf((IExpression)eq);
        public static IExpression LnOf(IExpression eq)
        {
            return Ln.LnOf(eq);
        }

        public static IExpression LogOf(Expression left, Expression right) => LogOf((IExpression)left, (IExpression)right);
        public static IExpression LogOf(Expression left, IExpression right) => LogOf((IExpression)left, right);
        public static IExpression LogOf(IExpression left, Expression right) => LogOf(left, (IExpression)right);
        public static IExpression LogOf(IExpression a, IExpression b)
        {
            return LogIdentity.Instance.CreateExpression(a, b);
        }

        public static IExpression SignOf(Expression eq) => SignOf((IExpression)eq);
        public static IExpression SignOf(IExpression eq)
        {
            return Sign.SignOf(eq);
        }

        public static IExpression SinOf(Expression eq) => SinOf((IExpression)eq);
        public static IExpression SinOf(IExpression eq)
        {
            return Sin.SinOf(eq);
        }

        public static IExpression CosOf(Expression eq) => CosOf((IExpression)eq);
        public static IExpression CosOf(IExpression eq)
        {
            return CosIdentity.Instance.CreateExpression(eq);
        }

        public static IExpression TanOf(Expression eq) => TanOf((IExpression)eq);
        public static IExpression TanOf(IExpression eq)
        {
            return TanIdentity.Instance.CreateExpression(eq);
        }

        public static IExpression ArcsinOf(Expression eq) => ArcsinOf((IExpression)eq);
        public static IExpression ArcsinOf(IExpression eq)
        {
            return Arcsin.ArcsinOf(eq);
        }

        public static IExpression ArccosOf(Expression eq) => ArccosOf((IExpression)eq);
        public static IExpression ArccosOf(IExpression eq)
        {
            return ArccosIdentity.Instance.CreateExpression(eq);
        }

        public static IExpression ArctanOf(Expression eq) => ArctanOf((IExpression)eq);
        public static IExpression ArctanOf(IExpression eq)
        {
            return Arctan.ArctanOf(eq);
        }

        public static IExpression Abs(Expression eq) => Abs((IExpression)eq);
        public static IExpression Abs(IExpression eq)
        {
            return AbsIdentity.Instance.CreateExpression(eq);
        }

        public static IExpression Min(Expression left, Expression right) => Min((IExpression)left, (IExpression)right);
        public static IExpression Min(Expression left, IExpression right) => Min((IExpression)left, right);
        public static IExpression Min(IExpression left, Expression right) => Min(left, (IExpression)right);
        public static IExpression Min(IExpression a, IExpression b)
        {
            return MinIdentity.Instance.CreateExpression(a, b);
        }

        public static IExpression Max(Expression left, Expression right) => Max((IExpression)left, (IExpression)right);
        public static IExpression Max(Expression left, IExpression right) => Max((IExpression)left, right);
        public static IExpression Max(IExpression left, Expression right) => Max(left, (IExpression)right);
        public static IExpression Max(IExpression a, IExpression b)
        {
            return MaxIdentity.Instance.CreateExpression(a, b);
        }

        public static IExpression SelectOn(Expression lt, Expression gt, Expression condition) => SelectOn((IExpression)lt, (IExpression)gt, (IExpression)condition);
        public static IExpression SelectOn(Expression lt, IExpression gt, Expression condition) => SelectOn((IExpression)lt, gt, (IExpression)condition);
        public static IExpression SelectOn(IExpression lt, Expression gt, Expression condition) => SelectOn(lt, (IExpression)gt, (IExpression)condition);
        public static IExpression SelectOn(IExpression lt, IExpression gt, Expression condition) => SelectOn(lt, gt, (IExpression)condition);
        public static IExpression SelectOn(Expression lt, Expression gt, IExpression condition) => SelectOn((IExpression)lt, (IExpression)gt, condition);
        public static IExpression SelectOn(Expression lt, IExpression gt, IExpression condition) => SelectOn((IExpression)lt, gt, condition);
        public static IExpression SelectOn(IExpression lt, Expression gt, IExpression condition) => SelectOn(lt, (IExpression)gt, condition);
        public static IExpression SelectOn(IExpression lt, IExpression gt, IExpression condition)
        {
            return SelectIdentity.Instance.CreateExpression(lt, gt, condition);
        }

        public static IExpression SinhOf(Expression eq) => SinhOf((IExpression)eq);
        public static IExpression SinhOf(IExpression a)
        {
            return SinhIdentity.Instance.CreateExpression(a);
        }

        public static IExpression CoshOf(Expression eq) => CoshOf((IExpression)eq);
        public static IExpression CoshOf(IExpression a)
        {
            return CoshIdentity.Instance.CreateExpression(a);
        }

        public static IExpression TanhOf(Expression eq) => TanhOf((IExpression)eq);
        public static IExpression TanhOf(IExpression a)
        {
            return TanhIdentity.Instance.CreateExpression(a);
        }

        public static IExpression ArsinhOf(Expression eq) => ArsinhOf((IExpression)eq);
        public static IExpression ArsinhOf(IExpression a)
        {
            return ArsinhIdentity.Instance.CreateExpression(a);
        }

        public static IExpression ArcoshOf(Expression eq) => ArcoshOf((IExpression)eq);
        public static IExpression ArcoshOf(IExpression a)
        {
            return ArcoshIdentity.Instance.CreateExpression(a);
        }

        public static IExpression ArtanhOf(Expression eq) => ArtanhOf((IExpression)eq);
        public static IExpression ArtanhOf(IExpression a)
        {
            return ArtanhIdentity.Instance.CreateExpression(a);
        }
    }
}
