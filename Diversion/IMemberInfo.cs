using System.Collections.Generic;

namespace Diversion
{
    public interface IAttributable
    {
        IEnumerable<IAttributeInfo> Attributes { get; }
    }

    public interface IVirtualizable
    {
        bool IsVirtual { get; }
        bool IsAbstract { get; }
    }

    public interface IMemberInfo : IAttributable
    {
        string Name { get; }
        bool IsPublic { get; }
        bool IsStatic { get; }
    }

    public interface IMethodInfo : IMemberInfo, IVirtualizable
    {
        IEnumerable<IParameterInfo> Parameters { get; }
        IEnumerable<IGenericParameter> GenericParameters { get; }
        IParameterInfo ReturnType { get; }
    }

    public interface IGenericParameter
    {
        string Name { get; }
        IEnumerable<ITypeInfo> Constraints { get; }
        bool RequiresDefaultConstructor { get; }
        GenericTypeRequirement TypeRequirement { get; }
    }

    public enum GenericTypeRequirement
    {
        None,
        Class,
        Struct
    }

    public interface IConstructorInfo : IMemberInfo
    {
        IEnumerable<IParameterInfo> Parameters { get; }
    }

    public interface IEventInfo : IMemberInfo, IVirtualizable
    {
        ITypeInfo EventHandlerType { get; }
    }

    public interface IFieldInfo : IMemberInfo
    {
        ITypeInfo Type { get; }
        bool IsReadOnly { get; }
        bool IsConstant { get; }
    }

    public interface IPropertyInfo : IMemberInfo, IVirtualizable
    {
        IEnumerable<IParameterInfo> Parameters { get; }
        ITypeInfo Type { get; }
    }
}