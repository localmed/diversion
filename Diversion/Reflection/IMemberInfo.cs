using System.Reflection;

namespace Diversion.Reflection
{
    public interface IMemberInfo : IAttributable, IIdentifiable
    {
        MemberInfo Member { get; }
        ITypeInfo DeclaringType { get; }
        string Name { get; }
        bool IsPublic { get; }
        bool IsStatic { get; }
    }
}