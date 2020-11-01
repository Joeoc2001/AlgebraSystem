using Algebra.Equivalence;
using Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Compilation
{
    public class Compiler<Compiled>
    {
        private class CompileTraverser : IEvaluator<int>
        {
            private readonly List<Compiled> _instructions;
            private readonly Dictionary<Expression, int> _cache;
            private readonly Compiler<Compiled> _owningCompiler;

            public CompileTraverser(Compiler<Compiled> owningCompiler)
            {
                _owningCompiler = owningCompiler ?? throw new ArgumentNullException(nameof(owningCompiler));
                _instructions = new List<Compiled>();
                _cache = new Dictionary<Expression, int>();
            }

            public Compiled[] GetCompiled()
            {
                return _instructions.ToArray();
            }

            private int EvaluateAndCache(Expression expression)
            {
                if (_cache.TryGetValue(expression, out int index))
                {
                    return index;
                }

                index = expression.Evaluate(this);
                _cache.Add(expression, index);
                return index;
            }

            private int AddAndReturn(Compiled compiled)
            {
                _instructions.Add(compiled);
                return _instructions.Count - 1;
            }

            private int EvaluateMonad(Expression argumentExpression, Func<int, Compiled> mapping)
            {
                int argIndex = EvaluateAndCache(argumentExpression);
                Compiled compiled = mapping(argIndex);
                return AddAndReturn(compiled);
            }

            public int EvaluateArcsin(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _owningCompiler._baseMappings.Arcsin);
            }

            public int EvaluateArctan(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _owningCompiler._baseMappings.Arctan);
            }

            public int EvaluateConstant(IConstant value)
            {
                Compiled compiled = _owningCompiler._baseMappings.Constant(value);
                return AddAndReturn(compiled);
            }

            public int EvaluateExponent(Expression baseExpression, Expression powerExpression)
            {
                int baseIndex = EvaluateAndCache(baseExpression);
                int powerIndex = EvaluateAndCache(powerExpression);
                Compiled compiled = _owningCompiler._baseMappings.Exponent(baseIndex, powerIndex);
                return AddAndReturn(compiled);
            }

            public int EvaluateLn(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _owningCompiler._baseMappings.Ln);
            }

            public int EvaluateSign(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _owningCompiler._baseMappings.Sign);
            }

            public int EvaluateSin(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _owningCompiler._baseMappings.Sin);
            }

            public int EvaluateProduct(ICollection<Expression> expressions)
            {
                List<int> arguments = expressions.Select(e => EvaluateAndCache(e)).ToList();
                Compiled compiled = _owningCompiler._baseMappings.Product(arguments);
                return AddAndReturn(compiled);
            }

            public int EvaluateSum(ICollection<Expression> expressions)
            {
                List<int> arguments = expressions.Select(e => EvaluateAndCache(e)).ToList();
                Compiled compiled = _owningCompiler._baseMappings.Sum(arguments);
                return AddAndReturn(compiled);
            }

            public int EvaluateVariable(IVariable value)
            {
                Compiled compiled = _owningCompiler._baseMappings.Variable(value);
                return AddAndReturn(compiled);
            }

            public int EvaluateFunction(Function function)
            {
                if (_owningCompiler._functionMap.TryGetValue(function.GetIdentity(), out Func<List<int>, Compiled> mapping))
                {
                    List<int> arguments = function.GetParameterList().Select(e => EvaluateAndCache(e)).ToList();
                    Compiled compiled = mapping(arguments);
                    return AddAndReturn(compiled);
                }

                return function.GetAtomicBodiedExpression().Evaluate(this);
            }

            public int EvaluateOther(Expression other)
            {
                throw new NotImplementedException();
            }
        }

        public class BaseMappings
        {
            public readonly Func<int, Compiled> Arcsin;
            public readonly Func<int, Compiled> Arctan;
            public readonly Func<IConstant, Compiled> Constant;
            public readonly Func<int, int, Compiled> Exponent;
            public readonly Func<int, Compiled> Ln;
            public readonly Func<ICollection<int>, Compiled> Product;
            public readonly Func<int, Compiled> Sign;
            public readonly Func<int, Compiled> Sin;
            public readonly Func<ICollection<int>, Compiled> Sum;
            public readonly Func<IVariable, Compiled> Variable;

            public BaseMappings(Func<int, Compiled> arcsin, Func<int, Compiled> arctan, Func<IConstant, Compiled> constant,
                Func<int, int, Compiled> exponent, Func<int, Compiled> ln, Func<ICollection<int>, Compiled> product,
                Func<int, Compiled> sign, Func<int, Compiled> sin, Func<ICollection<int>, Compiled> sum, Func<IVariable, Compiled> variable)
            {
                Arcsin = arcsin ?? throw new ArgumentNullException(nameof(arcsin));
                Arctan = arctan ?? throw new ArgumentNullException(nameof(arctan));
                Constant = constant ?? throw new ArgumentNullException(nameof(constant));
                Exponent = exponent ?? throw new ArgumentNullException(nameof(exponent));
                Ln = ln ?? throw new ArgumentNullException(nameof(ln));
                Product = product ?? throw new ArgumentNullException(nameof(product));
                Sign = sign ?? throw new ArgumentNullException(nameof(sign));
                Sin = sin ?? throw new ArgumentNullException(nameof(sin));
                Sum = sum ?? throw new ArgumentNullException(nameof(sum));
                Variable = variable ?? throw new ArgumentNullException(nameof(variable));
            }
        }

        private readonly IDictionary<FunctionIdentity, Func<List<int>, Compiled>> _functionMap;
        private readonly IExpressionMetric _simplificationMetric;
        private readonly List<EquivalencePath> _paths;
        private readonly BaseMappings _baseMappings;

        public Compiler(IDictionary<FunctionIdentity, Func<List<int>, Compiled>> functionMap, BaseMappings baseMappings)
        {
            _functionMap = functionMap;
            _baseMappings = baseMappings;
            _simplificationMetric = new DefaultSimplificationMetric(functionMap.Keys);
            _paths = new List<EquivalencePath>(EquivalencePaths.DefaultAtomicPaths.Concat(EquivalencePaths.GenerateFunctionReplacementPaths(functionMap.Keys)));
        }

        public Compiled[] Compile(Expression expression)
        {
            expression = expression.Simplify(metric:_simplificationMetric, depth:3, equivalencies:_paths);
            CompileTraverser traverser = new CompileTraverser(this);
            expression.Evaluate(traverser);
            return traverser.GetCompiled();
        }
    }
}
