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

            private readonly IVariable _value;

            public Result(IVariable value)
            {
                this._value = value ?? throw new ArgumentNullException(nameof(value));
            }

            private Result()
            {
                _value = null;
            }

            public bool IsVariable()
            {
                return !(_value is null);
            }

            public IVariable Get()
            {
                return _value;
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

        public override Result EvaluateVariable(IVariable name)
        {
            return new Result(name);
        }
    }
}
