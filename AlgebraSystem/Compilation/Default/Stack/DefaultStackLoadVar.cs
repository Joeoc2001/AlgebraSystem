namespace Algebra.Compilation
{
    namespace Default.Stack
    {
        internal class DefaultStackLoadVar : IDefaultStackInstruction
        {
            public DefaultOpcode Opcode { get => DefaultOpcode.VARIABLE; }

            public string Name { get; }

            public DefaultStackLoadVar(string name)
            {
                Name = name;
            }

            public override bool Equals(object obj)
            {
                return obj is DefaultStackLoadVar var &&
                       Name == var.Name;
            }

            public override int GetHashCode()
            {
                return -2134847229 + Name.GetHashCode();
            }

            public override string ToString()
            {
                return $"Load Var {Name}";
            }
        }
    }
}
