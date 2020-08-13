using Algebra.Atoms;
using System.Collections.Generic;
using System.Linq;

namespace Algebra
{
    public class RandomExpressionGenerator
    {
        //public float baseProb;
        //public int maxDepth;

        //public Expression Next()
        //{
        //    return Gen(baseProb, maxDepth);
        //}

        //private static Expression Gen(float baseProb, int maxDepth)
        //{
        //    if (maxDepth <= 0 || Random.value < baseProb) // Return a terminating node
        //    {
        //        float terminatingValue = Random.value;
        //        if (terminatingValue < 0.7)
        //        {
        //            // Return a constant
        //            return Random.value;
        //        }
        //        else
        //        {
        //            // Return a variable
        //            var vars = Variable.VariableDict.Values;
        //            return vars.ToList()[Random.Range(0, vars.Count)];
        //        }
        //    }

        //    if (Random.value < 0.7) // Return a simple function node
        //    {
        //        float functionValue = Random.value;
        //        if (functionValue < 0.1)
        //        {
        //            // Return a log function
        //            return Expression.LnOf(Gen(baseProb, maxDepth - 1));
        //        }
        //        else if (functionValue < 0.2)
        //        {
        //            // Return a sign function
        //            return Expression.SignOf(Gen(baseProb, maxDepth - 1));
        //        }
        //        else if (functionValue < 0.3)
        //        {
        //            // Return a min function
        //            return Expression.Min(Gen(baseProb, maxDepth - 1),
        //                Gen(baseProb, maxDepth - 1));
        //        }
        //        else if (functionValue < 0.4)
        //        {
        //            // Return a max function
        //            return Expression.Max(Gen(baseProb, maxDepth - 1),
        //                Gen(baseProb, maxDepth - 1));
        //        }
        //        else if (functionValue < 0.5)
        //        {
        //            // Return an Abs function
        //            return Expression.Abs(Gen(baseProb, maxDepth - 1));
        //        }
        //        else if (functionValue < 0.6)
        //        {
        //            // Return an Sin function
        //            return Expression.SinOf(Gen(baseProb, maxDepth - 1));
        //        }
        //        else if (functionValue < 0.7)
        //        {
        //            // Return an Sin function
        //            return Expression.CosOf(Gen(baseProb, maxDepth - 1));
        //        }
        //        else if (functionValue < 0.8)
        //        {
        //            // Return an Sin function
        //            return Expression.TanOf(Gen(baseProb, maxDepth - 1));
        //        }
        //        else
        //        {
        //            // Return exponentiation
        //            return Expression.Pow(Gen(baseProb, maxDepth - 1),
        //                Gen(baseProb, maxDepth - 1));
        //        }
        //    }
        //    else // Addition or subtraction
        //    {
        //        int x = Random.Range(1, 5);
        //        List<Expression> eqs = new List<Expression>(x);
        //        for (int i = 0; i < x; i++)
        //        {
        //            eqs.Add(Gen(baseProb, maxDepth - 1));
        //        }

        //        float operationValue = Random.value;
        //        if (operationValue < 0.5)
        //        {
        //            // Return addition
        //            return Expression.Add(eqs);
        //        }
        //        else
        //        {
        //            // Return Multiplication
        //            return Expression.Multiply(eqs);
        //        }
        //    }
        //}
    }
}
