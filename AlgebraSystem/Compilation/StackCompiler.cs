﻿using Algebra.Equivalence;
using Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra.Compilation
{
    public abstract class StackCompiler<ReturnType, Compiled>
    {
        private class CompileTraverser : IMapping
        {
            private readonly List<Compiled> _instructions;
            private readonly StackCompiler<ReturnType, Compiled> _owningCompiler;
            private readonly IVariableInputSet<ReturnType> _variables;

            public CompileTraverser(StackCompiler<ReturnType, Compiled> owningCompiler, IVariableInputSet<ReturnType> variables)
            {
                _owningCompiler = owningCompiler ?? throw new ArgumentNullException(nameof(owningCompiler));
                _instructions = new List<Compiled>();
                _variables = variables;
            }

            public Compiled[] GetCompiled()
            {
                return _instructions.ToArray();
            }

            private void Add(Compiled compiled)
            {
                _instructions.Add(compiled);
            }

            private void EvaluateMonad(Expression argumentExpression, Func<Compiled> mapping)
            {
                argumentExpression.Map(this);
                Compiled compiled = mapping();
                Add(compiled);
            }

            public void EvaluateArcsin(Expression argumentExpression)
            {
                EvaluateMonad(argumentExpression, _owningCompiler.EvaluateArcsin);
            }

            public void EvaluateArctan(Expression argumentExpression)
            {
                EvaluateMonad(argumentExpression, _owningCompiler.EvaluateArctan);
            }

            public void EvaluateConstant(IConstant value)
            {
                Compiled compiled = _owningCompiler.EvaluateConstant(value);
                Add(compiled);
            }

            public void EvaluateExponent(Expression baseExpression, Expression powerExpression)
            {
                baseExpression.Map(this);
                powerExpression.Map(this);
                Compiled compiled = _owningCompiler.EvaluateExponent();
                Add(compiled);
            }

            public void EvaluateLn(Expression argumentExpression)
            {
                EvaluateMonad(argumentExpression, _owningCompiler.EvaluateLn);
            }

            public void EvaluateSign(Expression argumentExpression)
            {
                EvaluateMonad(argumentExpression, _owningCompiler.EvaluateSign);
            }

            public void EvaluateSin(Expression argumentExpression)
            {
                EvaluateMonad(argumentExpression, _owningCompiler.EvaluateSin);
            }

            private void EvaluateSequential(ICollection<Expression> expressions, Func<Compiled> gen)
            {
                if (expressions.Count == 0)
                {
                    throw new IndexOutOfRangeException("Empty sequential not supported");
                }

                var enumerator = expressions.GetEnumerator();
                enumerator.MoveNext();
                enumerator.Current.Map(this);
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Map(this);
                    Compiled compiled = gen();
                    Add(compiled);
                }
            }

            public void EvaluateProduct(ICollection<Expression> expressions)
            {
                EvaluateSequential(expressions, _owningCompiler.EvaluateProduct);
            }

            public void EvaluateSum(ICollection<Expression> expressions)
            {
                EvaluateSequential(expressions, _owningCompiler.EvaluateSum);
            }

            public void EvaluateVariable(IVariable value)
            {
                Compiled compiled = _owningCompiler.EvaluateVariable(value, _variables);
                Add(compiled);
            }

            public void EvaluateFunction(Function function)
            {
                if (_owningCompiler._supportedFunctions.Contains(function.GetIdentity()))
                {
                    foreach (Expression param in function.GetParameterList())
                    {
                        param.Map(this);
                    }
                    Compiled compiled = _owningCompiler.EvaluateFunction(function.GetIdentity());
                    Add(compiled);
                }
                else
                {
                    function.GetAtomicBodiedExpression().Map(this);
                }
            }
        }

        private readonly ICollection<FunctionIdentity> _supportedFunctions;
        private readonly IExpressionMetric _simplificationMetric;
        private readonly List<EquivalencePath> _paths;

        public StackCompiler(ICollection<FunctionIdentity> supportedFunctions)
        {
            _supportedFunctions = supportedFunctions;
            _simplificationMetric = new DefaultSimplificationMetric(supportedFunctions);
            _paths = new List<EquivalencePath>(EquivalencePaths.DefaultAtomicPaths.Concat(EquivalencePaths.GenerateFunctionReplacementPaths(supportedFunctions)));
        }

        protected abstract ICompiledFunction<ReturnType> CreateCompiled(Expression expression, IVariableInputSet<ReturnType> variables, Compiled[] instructions);

        public ICompiledFunction<ReturnType> Compile(Expression expression, IVariableInputSet<ReturnType> variables, int simplificationAggressiveness=3)
        {
            expression = expression.Simplify(metric:_simplificationMetric, depth:simplificationAggressiveness, equivalencies:_paths);
            CompileTraverser traverser = new CompileTraverser(this, variables);
            expression.Map(traverser);
            Compiled[] instructions = traverser.GetCompiled();
            return CreateCompiled(expression, variables, instructions);
        }

        protected abstract Compiled EvaluateArcsin();
        protected abstract Compiled EvaluateArctan();
        protected abstract Compiled EvaluateConstant(IConstant value);
        protected abstract Compiled EvaluateExponent();
        protected abstract Compiled EvaluateLn();
        protected abstract Compiled EvaluateSign();
        protected abstract Compiled EvaluateSin();
        protected abstract Compiled EvaluateProduct();
        protected abstract Compiled EvaluateSum();
        protected abstract Compiled EvaluateVariable(IVariable value, IVariableInputSet<ReturnType> variables);
        protected abstract Compiled EvaluateFunction(FunctionIdentity function);
    }
}
