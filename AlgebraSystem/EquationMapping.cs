using Algebra;


namespace Algebra
{
    public class EquationMapping
    {
        public static implicit operator EquationMapping(EquationMap map) => new EquationMapping() { PostMap = map };

        public delegate Expression EquationMap(Expression a);
        public delegate bool EquationFilter(Expression a);

        public EquationMap PostMap = (a => a);
        public EquationFilter ShouldMapChildren = (a => true);
        public EquationFilter ShouldMapThis = (a => true);
    }
}
