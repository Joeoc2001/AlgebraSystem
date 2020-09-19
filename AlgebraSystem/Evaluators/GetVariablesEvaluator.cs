using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class GetVariablesEvaluator : TraversalEvaluator<HashSet<IVariable>>
    {
        public static readonly GetVariablesEvaluator Instance = new GetVariablesEvaluator();

        protected GetVariablesEvaluator()
        {

        }

        private static HashSet<IVariable> Combine(ICollection<HashSet<IVariable>> variables)
        {
            HashSet<IVariable> newSet = new HashSet<IVariable>();
            foreach (var variableSet in variables)
            {
                newSet.UnionWith(variableSet);
            }
            return newSet;
        }

        public override HashSet<IVariable> EvaluateConstant(IConstant value)
        {
            return new HashSet<IVariable>();
        }

        public override HashSet<IVariable> EvaluateVariable(IVariable value)
        {
            return new HashSet<IVariable>() { value };
        }

        protected override HashSet<IVariable> Arcsin(HashSet<IVariable> expression)
        {
            return expression;
        }

        protected override HashSet<IVariable> Arctan(HashSet<IVariable> expression)
        {
            return expression;
        }

        protected override HashSet<IVariable> EvaluateFunction(Function function, IList<HashSet<IVariable>> parameters)
        {
            return Combine(parameters);
        }

        protected override HashSet<IVariable> Ln(HashSet<IVariable> expression)
        {
            return expression;
        }

        protected override HashSet<IVariable> Pow(HashSet<IVariable> b, HashSet<IVariable> e)
        {
            return Combine(new List<HashSet<IVariable>>() { b, e });
        }

        protected override HashSet<IVariable> Product(ICollection<HashSet<IVariable>> expressions)
        {
            return Combine(expressions);
        }

        protected override HashSet<IVariable> Sign(HashSet<IVariable> expression)
        {
            return expression;
        }

        protected override HashSet<IVariable> Sin(HashSet<IVariable> expression)
        {
            return expression;
        }

        protected override HashSet<IVariable> Sum(ICollection<HashSet<IVariable>> expressions)
        {
            return Combine(expressions);
        }

        public override HashSet<IVariable> EvaluateOther(Expression other)
        {
            throw new NotImplementedException($"Cannot get variables for unknown expression {other}. Override {typeof(GetVariablesEvaluator).Name} to add functionality for your new class.");
        }
    }
}
