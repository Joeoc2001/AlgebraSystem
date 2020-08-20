using Algebra;


namespace Algebra
{
    public class ExpressionMapping
    {
        public static implicit operator ExpressionMapping(ExpressionMap map) => new ExpressionMapping() { Map = map };

        public delegate Expression ExpressionMap(Expression a);
        public delegate bool ExpressionFilter(Expression a);

        public ExpressionMap Map = (a => a);
        public ExpressionFilter ShouldMapChildren = (a => true);
        public ExpressionFilter ShouldMapThis = (a => true);
    }
}
