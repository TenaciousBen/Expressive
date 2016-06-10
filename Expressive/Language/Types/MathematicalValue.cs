using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apirion.Expressive.Core.Language.Types
{
    public abstract class MathematicalValue<T> : ComparableValue<T>
        where T : class, IComparable<T>, IEquatable<T>
    {
        public abstract T Add(T value);
        public abstract T Subtract(T value);
        public abstract T Multiply(T value);
        public abstract T Divide(T value);

        public static T operator +(MathematicalValue<T> a, T b)
        {
            return a.Add(b);
        }

        public static T operator -(MathematicalValue<T> a, T b)
        {
            return a.Subtract(b);
        }

        public static T operator *(MathematicalValue<T> a, T b)
        {
            return a.Multiply(b);
        }

        public static T operator /(MathematicalValue<T> a, T b)
        {
            return a.Divide(b);
        }
    }
}
