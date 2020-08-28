using System.Collections.Generic;

namespace Algebra.Equivalence
{
    public abstract class EquivalencePath
    {
        public abstract IEnumerable<IExpression> GetAllFrom(IExpression expression);
        public IEnumerable<IExpression> GetAllFrom(IEnumerable<IExpression> expressions)
        {
            foreach (IExpression expression in expressions)
            {
                foreach (IExpression result in GetAllFrom(expression))
                {
                    yield return result;
                }
            }
        }
    }
}