using System.Collections.Generic;

namespace Diversion
{
    public class IdentityComparer<T> : IEqualityComparer<T>
        where T : IIdentifiable
    {
        public bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y) || (x != null && y != null && x.GetType() == y.GetType() && x.Identity == y.Identity);
        }

        public int GetHashCode(T obj)
        {
            return obj == null ? string.Empty.GetHashCode() : (obj.GetType() + obj.Identity).GetHashCode();
        }
    }
}