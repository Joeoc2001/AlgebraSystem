using Algebra.Equivalence;
using Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Compilation
{
    public abstract class Compiler<ReturnType, Compiled>
    {
        private class CompileTraverser : IEvaluator<int>
        {
            private readonly List<Compiled> _instructions;
            private readonly Dictionary<Expression, int> _cache;
            private readonly Compiler<ReturnType, Compiled> _owningCompiler;
            private readonly IVariableInputSet<ReturnType> _variables;

            public CompileTraverser(Compiler<ReturnType, Compiled> owningCompiler, IVariableInputSet<ReturnType> variables)
            {
                _owningCompiler = owningCompiler ?? throw new ArgumentNullException(nameof(owningCompiler));
                _instructions = new List<Compiled>();
                _cache = new Dictionary<Expression, int>();
                _variables = variables;
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
                return EvaluateMonad(argumentExpression, _owningCompiler.EvaluateArcsin);
            }

            public int EvaluateArctan(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _owningCompiler.EvaluateArctan);
            }

            public int EvaluateConstant(IConstant value)
            {
                Compiled compiled = _owningCompiler.EvaluateConstant(value);
                return AddAndReturn(compiled);
            }

            public int EvaluateExponent(Expression baseExpression, Expression powerExpression)
            {
                int baseIndex = EvaluateAndCache(baseExpression);
                int powerIndex = EvaluateAndCache(powerExpression);
                Compiled compiled = _owningCompiler.EvaluateExponent(baseIndex, powerIndex);
                return AddAndReturn(compiled);
            }

            public int EvaluateLn(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _owningCompiler.EvaluateLn);
            }

            public int EvaluateSign(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _owningCompiler.EvaluateSign);
            }

            public int EvaluateSin(Expression argumentExpression)
            {
                return EvaluateMonad(argumentExpression, _owningCompiler.EvaluateSin);
            }

            private int EvaluateSequential(ICollection<Expression> expressions, Func<int, int, Compiled> map)
            {
                List<int> arguments = expressions.Select(e => EvaluateAndCache(e)).ToList();
                int last = arguments[0];
                for (int i = 1; i < arguments.Count; i++)
                {
                    Compiled compiled = map(last, arguments[i]);
                    last = AddAndReturn(compiled);
                }
                return last;
            }

            public int EvaluateProduct(ICollection<Expression> expressions)
            {
                return EvaluateSequential(expressions, _owningCompiler.EvaluateProduct);
            }

            public int EvaluateSum(ICollection<Expression> expressions)
            {
                return EvaluateSequential(expressions, _owningCompiler.EvaluateSum);
            }

            public int EvaluateVariable(IVariable value)
            {
                Compiled compiled = _owningCompiler.EvaluateVariable(value, _variables);
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

        private readonly IDictionary<FunctionIdentity, Func<List<int>, Compiled>> _functionMap;
        private readonly IExpressionMetric _simplificationMetric;
        private readonly List<EquivalencePath> _paths;

        public Compiler(IDictionary<FunctionIdentity, Func<List<int>, Compiled>> functionMap)
        {
            _functionMap = functionMap;
            _simplificationMetric = new DefaultSimplificationMetric(functionMap.Keys);
            _paths = new List<EquivalencePath>(EquivalencePaths.DefaultAtomicPaths.Concat(EquivalencePaths.GenerateFunctionReplacementPaths(functionMap.Keys)));
        }

        protected abstract ICompiledFunction<ReturnType> CreateCompiled(Expression expression, IVariableInputSet<ReturnType> variables, Compiled[] instructions);

        public ICompiledFunction<ReturnType> Compile(Expression expression, IVariableInputSet<ReturnType> variables)
        {
            expression = expression.Simplify(metric:_simplificationMetric, depth:3, equivalencies:_paths);
            CompileTraverser traverser = new CompileTraverser(this, variables);
            expression.Evaluate(traverser);
            Compiled[] instructions = traverser.GetCompiled();
            return CreateCompiled(expression, variables, instructions);
        }

        protected abstract Compiled EvaluateArcsin(int arg);
        protected abstract Compiled EvaluateArctan(int arg);
        protected abstract Compiled EvaluateConstant(IConstant value);
        protected abstract Compiled EvaluateExponent(int baseIndex, int powerIndex);
        protected abstract Compiled EvaluateLn(int arg);
        protected abstract Compiled EvaluateSign(int arg);
        protected abstract Compiled EvaluateSin(int arg);
        protected abstract Compiled EvaluateProduct(int argument1, int argument2);
        protected abstract Compiled EvaluateSum(int argument1, int argument2);
        protected abstract Compiled EvaluateVariable(IVariable value, IVariableInputSet<ReturnType> variables);
    }
}
