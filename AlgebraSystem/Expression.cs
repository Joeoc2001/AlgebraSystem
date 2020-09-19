using Algebra.Atoms;
using Algebra.Equivalence;
using Algebra.Evaluators;
using Algebra.Functions.HardcodedFunctionIdentities;
using Algebra.Metrics;
using Algebra.Parsing;
using Algebra.PatternMatching;
using Rationals;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Algebra
{
    public abstract class Expression
    {
        public static implicit operator Expression(int r) => (RationalConstant)r;
        public static implicit operator Expression(long r) => (RationalConstant)r;
        public static implicit operator Expression(float r) => (RationalConstant)r;
        public static implicit operator Expression(double r) => (RationalConstant)r;
        public static implicit operator Expression(decimal r) => (RationalConstant)r;
        public static implicit operator Expression(Rational r) => (RationalConstant)r;

        public static implicit operator Expression(string s) => From(s);

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
            return RationalConstant.FromValue(value);
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

        public static Expression From(string s)
        {
            return Parser.Parse(s);
        }

        public abstract Expression GetDerivative(string wrt);
        protected abstract int GenHashCode();
        protected abstract Expression GenAtomicExpression();
        public abstract T Evaluate<T>(IEvaluator<T> evaluator);
        public abstract T Evaluate<T>(IExpandedEvaluator<T> evaluator);
        public abstract T Evaluate<T>(Expression other, IDualEvaluator<T> evaluator);

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

        // Generated on demand
        private int? _hashCode;
        private bool? _isAtomic;
        private Expression _atomicExpression; 

        public Expression()
        {

        }

        public override int GetHashCode()
        {
            if (!_hashCode.HasValue)
            {
                _hashCode = GenHashCode();
            }
            return _hashCode.Value;
        }

        /// <summary>
        /// Checks if this expression is in atomic form, i.e. has no function calls. 
        /// </summary>
        /// <returns>True if this is atomic</returns>
        public bool IsAtomic()
        {
            if (!_isAtomic.HasValue)
            {
                _isAtomic = Evaluate(IsAtomicEvaluator.Instance);
            }
            return _isAtomic.Value;
        }

        /// <summary>
        /// Replaces all non-atomic operations with their atomic counterparts.
        /// The resulting expressions should not be executed as it will be far slower.
        /// Instead, this form is used as an intermediate form for performing simplifications.
        /// </summary>
        /// <returns>An expression in atomic form</returns>
        public Expression GetAtomicExpression()
        {
            if (IsAtomic())
            {
                return this;
            }

            if (_atomicExpression is null)
            {
                _atomicExpression = GenAtomicExpression();
            }

            return _atomicExpression;
        }

        public double EvaluateOnce() => EvaluateOnce(new VariableInputSet<double>() { });
        public double EvaluateOnce(double x) => EvaluateOnce(new VariableInputSet<double>() { { "x", x } });
        public double EvaluateOnce(double x, double y) => EvaluateOnce(new VariableInputSet<double>() { { "x", x }, { "y", y } });
        public double EvaluateOnce(double x, double y, double z) => EvaluateOnce(new VariableInputSet<double>() { { "x", x }, { "y", y }, { "z", z } });
        public double EvaluateOnce(Vector2 values) => EvaluateOnce(new VariableInputSet<double>() { { "x", values.X }, { "y", values.Y } });
        public double EvaluateOnce(Vector3 values) => EvaluateOnce(new VariableInputSet<double>() { { "x", values.X }, { "y", values.Y }, { "z", values.Z } });
        public double EvaluateOnce(VariableInputSet<double> variables)
        {
            return Evaluate(new DoubleEvaluator(variables));
        }

        public static bool ShouldParenthesise(Expression parent, Expression child)
        {
            return parent.GetOrderIndex() <= child.GetOrderIndex();
        }

        public static string ToParenthesisedString(Expression parent, Expression child)
        {
            if (ShouldParenthesise(parent, child))
            {
                return $"({child})";
            }

            return child.ToString();
        }

        /// <summary>
        /// Creates a new Equivalence Class used for proving equivalence and for finding alternate forms of an equation.
        /// </summary>
        /// <returns>A queriable equivalence class for this expression</returns>
        public EquivalenceClass GetEquivalenceClass()
        {
            return new EquivalenceClass(this);
        }

        public int CompareTo(Expression other)
        {
            return Evaluate(other, OrderingDualEvaluator.Instance);
        }

        public override sealed bool Equals(object obj)
        {
            return Equals(obj as Expression);
        }

        public bool Equals(Expression obj)
        {
            return Equals(obj, EqualityLevel.Exactly);
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

            if (ReferenceEquals(this, e))
            {
                return true;
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
                    return GetEquivalenceClass().IsInClass(e, 3);
                case EqualityLevel.Deepest:
                    return GetEquivalenceClass().IsInClass(e);
                default:
                    throw new NotImplementedException($"Unknown equality level: {level}");
            }
        }

        protected bool ExactlyEquals(Expression expression)
        {
            return Evaluate(expression, ExactlyEqualsDualEvaluator.Instance);
        }

        public HashSet<string> GetVariables()
        {
            return Evaluate(GetVariablesEvaluator.Instance);
        }

        public PatternMatchingResultSet Match(Expression pattern)
        {
            return Evaluate(pattern, PatternMatchingDualEvaluator.Instance);
        }

        /// <summary>
        /// Returns a set of expressions where all instance of a pattern have been replaced with a replacement expression.
        /// All of the variables in the replacement expression must be contained in the pattern expression.
        /// For example, if 3 * (x + y) + 2 is evaluated with an instance of this with pattern a + b and replacement a * b,
        /// the resulting expression set will be {6 * (x + y), 3 * x * y + 2}.
        /// This is useful for equality axioms, e.g. x * (y + z) == x * y + x * z
        /// </summary>
        /// <param name="pattern">The pattern to search this expression for</param>
        /// <param name="replacement">The expression to replace the found pattern with</param>
        /// <returns>A set of expressions where all instance of the pattern have been replaced with the replacement expression</returns>
        public IEnumerable<Expression> Replace(Expression pattern, Expression replacement)
        {
            return Evaluate(new ReplaceEvaluator(pattern, replacement));
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

        public static Expression operator +(Expression left, Expression right)
        {
            return Expression.Add(new List<Expression>() { left, right });
        }
        public static Expression operator +(double left, Expression right) => Expression.ConstantFrom(left) + right;
        public static Expression operator +(Expression left, double right) => left + Expression.ConstantFrom(right);

        public static Expression operator -(Expression left, Expression right)
        {
            return Expression.Add(new List<Expression>() { left, -right });
        }
        public static Expression operator -(double left, Expression right) => Expression.ConstantFrom(left) - right;
        public static Expression operator -(Expression left, double right) => left - Expression.ConstantFrom(right);

        public static Expression operator -(Expression a)
        {
            return Expression.MinusOne * a;
        }

        public static Expression operator *(Expression left, Expression right)
        {
            return Expression.Multiply(new List<Expression>() { left, right });
        }
        public static Expression operator *(double left, Expression right) => Expression.ConstantFrom(left) * right;
        public static Expression operator *(Expression left, double right) => left * Expression.ConstantFrom(right);

        public static Expression operator /(Expression left, Expression right)
        {
            return Expression.Divide(left, right);
        }
        public static Expression operator /(double left, Expression right) => Expression.ConstantFrom(left) / right;
        public static Expression operator /(Expression left, double right) => left / Expression.ConstantFrom(right);

        public static Expression Add<T>(params T[] eqs) where T : Expression => Add(new List<T>(eqs));
        public static Expression Add<T>(IEnumerable<T> eqs) where T : Expression => Sum.Add(eqs);
        public static Expression Multiply<T>(params T[] eqs) where T : Expression => Multiply(new List<T>(eqs));
        public static Expression Multiply<T>(IEnumerable<T> eqs) where T : Expression => Product.Multiply(eqs);

        public static Expression Divide(Expression left, Expression right) => DivIdentity.Instance.CreateExpression(left, right);

        public static Expression Pow(Expression left, Expression right) => Exponent.Pow(left, right);

        public static Expression Sqrt(Expression expression) => SqrtIdentity.Instance.CreateExpression(expression);

        public static Expression LnOf(Expression eq) => Ln.LnOf(eq);

        public static Expression LogOf(Expression a, Expression b) => LogIdentity.Instance.CreateExpression(a, b);

        public static Expression SignOf(Expression eq) => Sign.SignOf(eq);

        public static Expression SinOf(Expression eq) => Sin.SinOf(eq);

        public static Expression CosOf(Expression eq) => CosIdentity.Instance.CreateExpression(eq);

        public static Expression TanOf(Expression eq) => TanIdentity.Instance.CreateExpression(eq);

        public static Expression ArcsinOf(Expression eq) => Arcsin.ArcsinOf(eq);

        public static Expression ArccosOf(Expression eq) => ArccosIdentity.Instance.CreateExpression(eq);

        public static Expression ArctanOf(Expression eq) => Arctan.ArctanOf(eq);

        public static Expression AbsOf(Expression eq) => AbsIdentity.Instance.CreateExpression(eq);

        public static Expression Min(Expression a, Expression b) => MinIdentity.Instance.CreateExpression(a, b);

        public static Expression Max(Expression a, Expression b) => MaxIdentity.Instance.CreateExpression(a, b);

        public static Expression SelectOn(Expression lt, Expression gt, Expression condition) => SelectIdentity.Instance.CreateExpression(lt, gt, condition);

        public static Expression SinhOf(Expression a) => SinhIdentity.Instance.CreateExpression(a);

        public static Expression CoshOf(Expression a) => CoshIdentity.Instance.CreateExpression(a);

        public static Expression TanhOf(Expression a) => TanhIdentity.Instance.CreateExpression(a);

        public static Expression ArsinhOf(Expression a) => ArsinhIdentity.Instance.CreateExpression(a);

        public static Expression ArcoshOf(Expression a) => ArcoshIdentity.Instance.CreateExpression(a);

        public static Expression ArtanhOf(Expression a) => ArtanhIdentity.Instance.CreateExpression(a);
    }
}
