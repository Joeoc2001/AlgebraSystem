﻿using Algebra.Atoms;
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

        public static readonly Expression X = VariableFrom("x");
        public static readonly Expression Y = VariableFrom("y");
        public static readonly Expression Z = VariableFrom("z");

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
        public abstract int GetOrderIndex();

        private int? hashCode = null;
        public override int GetHashCode()
        {
            if (!hashCode.HasValue)
            {
                hashCode = GenHashCode();
            }
            return hashCode.Value;
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
        private IAtomicExpression atomicExpression = null;
        public IAtomicExpression GetAtomicExpression()
        {
            if (atomicExpression is null)
            {
                atomicExpression = GenAtomicExpression();
            }
            return atomicExpression;
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
            return Evaluate(new IsAtomicEvaluator());
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

        public static IExpression operator +(Expression left, Expression right)
        {
            return (IExpression) left + right;
        }

        public static IExpression operator -(Expression left, Expression right)
        {
            return (IExpression) left - right;
        }

        public static IExpression operator -(Expression a)
        {
            return MinusOne * a;
        }

        public static IExpression operator *(Expression left, Expression right)
        {
            return (IExpression) left * right;
        }

        public static IExpression operator /(Expression left, Expression right)
        {
            return (IExpression) left / right;
        }

        public static IExpression Add<T>(IEnumerable<T> eqs) where T : IExpression
        {
            return Sum.Add(eqs);
        }

        public static IExpression Multiply<T>(IEnumerable<T> eqs) where T : IExpression
        {
            return Product.Multiply(eqs);
        }

        public static IExpression Pow(IExpression left, IExpression right)
        {
            return Exponent.Pow(left, right);
        }

        public static IExpression LnOf(IExpression eq)
        {
            return Ln.LnOf(eq);
        }

        public static IExpression LogOf(IExpression a, IExpression b)
        {
            return LogIdentity.Instance.CreateExpression(a, b);
        }

        public static IExpression SignOf(IExpression eq)
        {
            return Sign.SignOf(eq);
        }

        public static IExpression SinOf(IExpression eq)
        {
            return Sin.SinOf(eq);
        }

        public static IExpression CosOf(IExpression eq)
        {
            return CosIdentity.Instance.CreateExpression(eq);
        }

        public static IExpression TanOf(IExpression eq)
        {
            return TanIdentity.Instance.CreateExpression(eq);
        }

        public static IExpression Abs(IExpression eq)
        {
            return AbsIdentity.Instance.CreateExpression(eq);
        }

        public static IExpression Min(IExpression a, IExpression b)
        {
            return MinIdentity.Instance.CreateExpression(a, b);
        }

        public static IExpression Max(IExpression a, IExpression b)
        {
            return MaxIdentity.Instance.CreateExpression(a, b);
        }

        public static IExpression SelectOn(IExpression lt, IExpression gt, IExpression condition)
        {
            return SelectIdentity.Instance.CreateExpression(lt, gt, condition);
        }

        public static IExpression SinhOf(IExpression a)
        {
            return SinhIdentity.Instance.CreateExpression(a);
        }

        public static IExpression CoshOf(IExpression a)
        {
            return CoshIdentity.Instance.CreateExpression(a);
        }

        public static IExpression TanhOf(IExpression a)
        {
            return TanhIdentity.Instance.CreateExpression(a);
        }
    }
}
