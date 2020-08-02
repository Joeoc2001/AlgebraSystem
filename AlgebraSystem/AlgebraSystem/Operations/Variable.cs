using System;
using System.Collections.Generic;


namespace Algebra.Operations
{
    public class Variable : Equation, IEquatable<Variable>
    {
        public class NotPresentException : ArgumentException
        {
            public NotPresentException(string message) : base(message)
            {
            }
        }

        public static readonly Variable X = new Variable("X");
        public static readonly Variable Y = new Variable("Y");
        public static readonly Variable Z = new Variable("Z");
        public static readonly Variable W = new Variable("W");

        public readonly string Name;

        public Variable(string name)
        {
            this.Name = name.ToLower();
        }

        public override ExpressionDelegate GetExpression(VariableInputSet set)
        {
            if (!set.Contains(Name))
            {
                throw new NotPresentException($"The variable {Name} is not present in the variable set");
            }

            VariableInput input = set[Name];
            return () => input.Value;
        }

        public override Equation GetDerivative(Variable wrt)
        {
            if (wrt == this)
            {
                return 1;
            }
            return 0;
        }

        public bool Equals(Variable obj)
        {
            if (obj is null)
            {
                return false;
            }

            return this.Name.Equals(obj.Name);
        }

        public override bool Equals(Equation obj)
        {
            return this.Equals(obj as Variable);
        }

        public override int GenHashCode()
        {
            return Name.GetHashCode() * 1513357220;
        }

        public override string ToString()
        {
            return Name;
        }

        public override string ToRunnableString()
        {
            return $"new Variable(\"{Name}\")";
        }

        public override int GetOrderIndex()
        {
            return 0;
        }

        public override Equation Map(EquationMapping map)
        {
            if (!map.ShouldMapThis(this))
            {
                return this;
            }

            return map.PostMap(this);
        }
    }
}