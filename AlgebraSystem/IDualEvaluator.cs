using Algebra.Functions;
using Rationals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra
{
    public interface IDualEvaluator<T>
    {
        T EvaluateConstants(IConstantValue value1, IConstantValue value2);
        T EvaluateVariables(IVariableValue value1, IVariableValue value2);
        T EvaluateSums(ICollection<Expression> arguments1, ICollection<Expression> arguments2);
        T EvaluateProducts(ICollection<Expression> arguments1, ICollection<Expression> arguments2);
        T EvaluateExponents(Expression baseArgument1, Expression powerArgument1, Expression baseArgument2, Expression powerArgument2);
        T EvaluateLns(Expression argument1, Expression argument2);
        T EvaluateSigns(Expression argument1, Expression argument2);
        T EvaluateSins(Expression argument1, Expression argument2);
        T EvaluateArcsins(Expression argument1, Expression argument2);
        T EvaluateArctans(Expression argument1, Expression argument2);
        T EvaluateFunctions(Function function1, Function function2);
        T EvaluateOthers(Expression expression1, Expression expression2);
    }
}
