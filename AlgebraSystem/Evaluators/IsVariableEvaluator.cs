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

            private readonly string name;

            public Result(string name)
            {
                this.name = name ?? throw new ArgumentNullException(nameof(name));
            }

            private Result()
            {
                name = null;
            }

            public bool IsVariable()
            {
                return name is null;
            }

            public string GetName()
            {
                return name;
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
