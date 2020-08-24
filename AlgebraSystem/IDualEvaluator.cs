using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IDualEvaluator<T>
    {
        T EvaluateConstants(Rational value1, Rational value2);
        T EvaluateVariables(string name1, string name2);
        T EvaluateSums(ICollection<IExpression> arguments1, ICollection<IExpression> arguments2);
        T EvaluateProducts(ICollection<IExpression> arguments1, ICollection<IExpression> arguments2);
        T EvaluateExponents(IExpression baseArgument1, IExpression powerArgument1, IExpression baseArgument2, IExpression powerArgument2);
        T EvaluateLns(IExpression argument1, IExpression argument2);
        T EvaluateSigns(IExpression argument1, IExpression argument2);
        T EvaluateSins(IExpression argument1, IExpression argument2);
        T EvaluateArcsins(IExpression argument1, IExpression argument2);
        T EvaluateArctans(IExpression argument1, IExpression argument2);
        T EvaluateFunctions(IFunction function1, IFunction function2);
        T EvaluateOthers(IExpression expression1, IExpression expression2);
    }
}
