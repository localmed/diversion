using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Diversion
{
    public class DiversionBase<T> : IDiversion<T>
    {
        readonly static Lazy<Func<IDiversion<T>, bool>> HasDivergedImpl = new Lazy<Func<IDiversion<T>, bool>>(CompileHasDivergedImpl, true);

        public DiversionBase(T old, T @new)
        {
            Old = old;
            New = @new;
        }

        public virtual string Identity => Old.ToString();

        public T New { get; private set; }
        public T Old { get; private set; }
        public virtual bool HasDiverged()
        {
            return !HasDivergedImpl.Value(this);
        }

        private static Func<IDiversion<T>, bool> CompileHasDivergedImpl()
        {
            var diversion = Expression.Parameter(typeof(IDiversion<T>));
            return Expression.Lambda<Func<IDiversion<T>, bool>>(
                typeof(T).GetProperties()
                    .Select(
                        p => (Expression)(
                            p.PropertyType != typeof(string) && p.PropertyType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ?
                                Expression.Call(typeof(Enumerable), "SequenceEqual",
                                    p.PropertyType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GenericTypeArguments,
                                    Expression.Property(Expression.Property(diversion, "Old"), p),
                                    Expression.Property(Expression.Property(diversion, "New"), p)) :
                                Expression.Call(typeof(object), "Equals", null,
                                    Expression.Convert(Expression.Property(Expression.Property(diversion, "Old"), p), typeof(object)),
                                    Expression.Convert(Expression.Property(Expression.Property(diversion, "New"), p), typeof(object)))))
                    .Aggregate((Expression)null, (r, e) => r == null ? e : Expression.AndAlso(r, e)), diversion).Compile();
        }
    }
}