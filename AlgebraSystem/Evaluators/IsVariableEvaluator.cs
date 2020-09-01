using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Evaluators
{
    public class IsVariableEvaluator : DefaultEvaluator<IsVariableEvaluator.Result>
    {
        public class Result
        {
            public static readonly Result False = new Result();

            private readonly string _name;

            public Result(string name)
            {
                this._name = name ?? throw new ArgumentNullException(nameof(name));
            }

            private Result()
            {
                _name = null;
            }

            public bool IsVariable()
            {
                return !(_name is null);
            }

            public string GetName()
            {
                return _name;
            }
        }

        public static readonly IsVariableEvaluator Instance = new IsVariableEvaluator();

        private IsVariableEvaluator()
        {

        }

        public override Result Default()
        {
            return Result.False;
        }

        public override Result EvaluateVariable(string name)
        {
            return new Result(name);
        }
    }
}
