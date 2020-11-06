namespace Algebra.Compilation
{
    namespace Default
    {
        internal class DefaultLoadVar : IDefaultInstruction
        {
            public DefaultOpcode Opcode { get => DefaultOpcode.VARIABLE; }

            public VariableInput<double> Variable { get; }

            public DefaultLoadVar(VariableInput<double> variable)
            {
                Variable = variable;
            }

            public override bool Equals(object obj)
            {
                return obj is DefaultLoadVar var &&
                       Variable == var.Variable;
            }

            public override int GetHashCode()
            {
                return -2134847229 + Variable.GetHashCode();
            }

            public override string ToString()
            {
                return $"Load Var {Variable}";
            }
        }
    }
}
