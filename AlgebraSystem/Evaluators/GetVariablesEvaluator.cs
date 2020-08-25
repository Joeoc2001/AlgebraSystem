using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class GetVariablesEvaluator : TraversalEvaluator<HashSet<string>>
    {
        public static readonly GetVariablesEvaluator Instance = new GetVariablesEvaluator();

        protected GetVariablesEvaluator()
        {

        }

        private static HashSet<string> Combine(ICollection<HashSet<string>> variables)
        {
            HashSet<string> newSet = new HashSet<string>();
            foreach (var variableSet in variables)
            {
                newSet.UnionWith(variableSet);
            }
            return newSet;
        }

        private static readonly HashSet<string> EmptySet = new HashSet<string>();
        public override HashSet<string> EvaluateConstant(Rational value)
        {
            return EmptySet;
        }

        public override HashSet<string> EvaluateVariable(string name)
        {
            return new HashSet<string>() { name };
        }

        protected override HashSet<string> Arcsin(HashSet<string> expression)
        {
            return expression;
        }

        protected override HashSet<string> Arctan(HashSet<string> expression)
        {
            return expression;
        }

        protected override HashSet<string> EvaluateFunction(IFunction function, IList<HashSet<string>> parameters)
        {
            return Combine(parameters);
        }

        protected override HashSet<string> Ln(HashSet<string> expression)
        {
            return expression;
        }

        protected override HashSet<string> Pow(HashSet<string> b, HashSet<string> e)
        {
            return Combine(new List<HashSet<string>>() { b, e });
        }

        protected override HashSet<string> Product(ICollection<HashSet<string>> expressions)
        {
            return Combine(expressions);
        }

        protected override HashSet<string> Sign(HashSet<string> expression)
        {
            return expression;
        }

        protected override HashSet<string> Sin(HashSet<string> expression)
        {
            return expression;
        }

        protected override HashSet<string> Sum(ICollection<HashSet<string>> expressions)
        {
            return Combine(expressions);
        }

        public override HashSet<string> EvaluateOther(IExpression other)
        {
            throw new NotImplementedException($"Cannot get variables for unknown expression {other}. Override {typeof(GetVariablesEvaluator).Name} to add functionality for your new class.");
        }
    }
}
