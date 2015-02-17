using System;
using System.Linq;
using System.Linq.Expressions;

namespace Diversion
{
    class DiversionImpl<T> : IDiversion<T>
    {
        readonly static Lazy<Func<IDiversion<T>, bool>> HasDivergedImpl = new Lazy<Func<IDiversion<T>, bool>>(CompileHasDivergedImpl, true);

        public DiversionImpl(T old, T @new)
        {
            Old = old;
            New = @new;
        }

        public T Old { get; private set; }

        public T New { get; private set; }

        public virtual bool HasDiverged()
        {
            return !HasDivergedImpl.Value(this);
        }

        private static Func<IDiversion<T>, bool> CompileHasDivergedImpl()
        {
            var diversion = Expression.Parameter(typeof(IDiversion<T>));
            return Expression.Lambda<Func<IDiversion<T>, bool>>(
                typeof (T).GetProperties()
                    .Where(p => !p.PropertyType.IsConstructedGenericType)
                    .Select(
                        p => (Expression)
                            (typeof(IIdentifiable).IsAssignableFrom(p.PropertyType) ?
                            Expression.Call(Expression.New(typeof(IdentityComparer<>).MakeGenericType(p.PropertyType)),
                                "Equals", null,
                                Expression.Property(Expression.Property(diversion, "Old"), p),
                                Expression.Property(Expression.Property(diversion, "New"), p)) :
                            Expression.Call(typeof (object), "Equals", null,
                                Expression.Convert(Expression.Property(Expression.Property(diversion, "Old"), p), typeof(object)),
                                Expression.Convert(Expression.Property(Expression.Property(diversion, "New"), p), typeof(object)))))
                    .Aggregate((Expression)null, (r, e) => r == null ? e : Expression.AndAlso(r, e)), diversion).Compile();
        }
    }
}