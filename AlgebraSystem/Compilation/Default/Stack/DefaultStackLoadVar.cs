namespace Algebra.Compilation
{
    namespace Default.Stack
    {
        internal class DefaultStackLoadVar : IDefaultStackInstruction
        {
            public DefaultOpcode Opcode { get => DefaultOpcode.VARIABLE; }

            public VariableInput<double> Variable { get; }
            private readonly string _name;

            public DefaultStackLoadVar(VariableInput<double> variable, string name)
            {
                Variable = variable;
                _name = name;
            }

            public override bool Equals(object obj)
            {
                return obj is DefaultStackLoadVar var &&
                       Variable == var.Variable;
            }

            public override int GetHashCode()
            {
                return -2134847229 + Variable.GetHashCode();
            }

            public override string ToString()
            {
                return $"Load Var {_name}";
            }
        }
    }
}
