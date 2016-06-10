using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apirion.Expressive.Core.Language.Types
{
    public abstract class ComparableValue<T> : IComparable<T>, IEquatable<T>
        where T : class, IComparable<T>, IEquatable<T>
    {
        public object Value { get; set; }

        public abstract int CompareTo(T other);

        public virtual bool Equals(T other)
        {
            if (CompareTo(other) == 0)
                return true;
            return false;
        }

        public static bool operator >(ComparableValue<T> left, T right)
        {
            if (left == null) return false; // null equals null and is less than all values
            return left.CompareTo(right) > 0;
        }

        public static bool operator <(ComparableValue<T> left, T right)
        {
            if (left == null) return false;
            return left.CompareTo(right) < 0;
        }

        public static bool operator >=(ComparableValue<T> left, T right)
        {
            if (left == null) return right == null;
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(ComparableValue<T> left, T right)
        {
            if (left == null) return right == null;
            return left.CompareTo(right) <= 0;
        }

        public static bool operator ==(ComparableValue<T> left, T right)
        {
            if ((object)left == null) return (object)right == null; // prevent recursive equality check by casting to object
            return left.Equals(right);
        }

        public static bool operator !=(ComparableValue<T> left, T right)
        {
            if ((object)left == null) return (object)right != null;
            return !left.Equals(right);
        }
    }
}
