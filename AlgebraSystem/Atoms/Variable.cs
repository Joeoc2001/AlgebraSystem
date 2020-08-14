using System;
using System.Collections.Generic;


namespace Algebra.Atoms
{
    public class Variable : Expression, IEquatable<Variable>
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

        public override ExpressionDelegate GetDelegate(VariableInputSet set)
        {
            if (!set.Contains(Name))
            {
                throw new NotPresentException($"The variable {Name} is not present in the variable set");
            }

            VariableInput input = set[Name];
            return () => input.Value;
        }

        public override Expression GetDerivative(Variable wrt)
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

        public override bool Equals(Expression obj)
        {
            return this.Equals(obj as Variable);
        }

        protected override int GenHashCode()
        {
            return Name.GetHashCode() * 1513357220;
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetOrderIndex()
        {
            return 0;
        }

        public override Expression Map(ExpressionMapping map)
        {
            if (!map.ShouldMapThis(this))
            {
                return this;
            }

            return map.PostMap(this);
        }
    }
}