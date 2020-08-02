using Algebra.Operations;
using System;
using System.Numerics;


namespace Algebra
{
    public unsafe struct VariableSet : IEquatable<VariableSet>
    {
        // Better idea: this is a dict taken only at function creation that points to all of the
        // variable cells and their names, allowing for faster allocation and execution after building the equation

        private fixed float values[Variable.VariablesCount];

        public static implicit operator VariableSet(float vector) => new VariableSet(vector);
        public static implicit operator VariableSet(Vector2 vector) => new VariableSet(vector);
        public static implicit operator VariableSet(Vector3 vector) => new VariableSet(vector);
        public static implicit operator VariableSet(float[] vector) => new VariableSet(vector);

        public VariableSet(float vector)
        {
            values[(int)Variable.Variables.X] = vector;
        }

        public VariableSet(Vector2 vector)
        {
            values[(int)Variable.Variables.X] = vector.X;
            values[(int)Variable.Variables.Y] = vector.Y;
        }

        public VariableSet(Vector3 vector)
        {
            values[(int)Variable.Variables.X] = vector.X;
            values[(int)Variable.Variables.Y] = vector.Y;
            values[(int)Variable.Variables.Z] = vector.Z;
        }

        public VariableSet(float[] vector)
        {
            int i = Math.Min(Variable.VariablesCount, vector.Length);
            for (int j = 0; j < i; j++)
            {
                values[j] = vector[j];
            }
        }

        public float this[Variable v]
        {
            get => values[v.Index];
        }

        private static bool NearlyEqual(float a, float b, float epsilon)
        {
            const double MinNormal = 2.2250738585072014E-308d;

            float absA = Math.Abs(a);
            float absB = Math.Abs(b);
            float diff = Math.Abs(a - b);

            if (a == b) // shortcut, handles infinities
            {
                return true;
            }
            else if (a == 0 || b == 0 || (absA + absB < MinNormal))
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < (epsilon * MinNormal);
            }
            else // use relative error
            {
                return diff / (absA + absB) < epsilon;
            }
        }

        public bool Equals(VariableSet obj)
        {
            return Equals(obj, 1E-19f);
        }

        public bool Equals(VariableSet o, float epsilon)
        {
            for (int i = 0; i < Variable.VariablesCount; i++)
            {
                if (NearlyEqual(values[i], o.values[i], epsilon))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj, 1E-19f);
        }

        public bool Equals(object obj, float epsilon)
        {
            if (obj is null || !(obj is VariableSet))
            {
                return false;
            }

            return this.Equals((VariableSet)obj, epsilon);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException("Variable sets cannot be hashed"); // No hash function can exist for this due to floating point accuracy
        }

        public static bool operator ==(VariableSet left, VariableSet right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            return left.Equals(right);
        }

        public static bool operator !=(VariableSet left, VariableSet right)
        {
            return !(left == right);
        }
    }
}