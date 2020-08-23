using Algebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraSystem.Test.Libs
{
    static class EqualityLibs
    {
        public enum EqualityType
        {
            AEqualsB,
            AEqualsBObj,
            ADoubleEqualsB,
            ANotEqualsB,
            AHashEqualsB,
            AExactlyEqualsB,
            AAtomicEqualB,
            ADeepEqualsB,
            ADeepestEqualsB
        }

        public enum Order
        {
            AB,
            BA
        }

        public static bool AreEqual(IExpression first, IExpression second, EqualityType type, Order order)
        {
            IExpression a;
            IExpression b;
            switch (order)
            {
                case Order.AB:
                    a = first;
                    b = second;
                    break;
                case Order.BA:
                    b = first;
                    a = second;
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (type)
            {
                case EqualityType.AEqualsB:
                    return a.Equals(b);
                case EqualityType.AEqualsBObj:
                    return a.Equals((object)b);
                case EqualityType.ADoubleEqualsB:
                    return a == b;
                case EqualityType.ANotEqualsB:
                    return !(a != b);
                case EqualityType.AHashEqualsB:
                    return a.GetHashCode().Equals(b.GetHashCode());
                case EqualityType.AExactlyEqualsB:
                    return a.Equals(b, EqualityLevel.Exactly);
                case EqualityType.AAtomicEqualB:
                    return a.Equals(b, EqualityLevel.Atomic);
                case EqualityType.ADeepEqualsB:
                    return a.Equals(b, EqualityLevel.Deep);
                case EqualityType.ADeepestEqualsB:
                    return a.Equals(b, EqualityLevel.Deepest);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
