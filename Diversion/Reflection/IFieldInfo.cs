namespace Diversion.Reflection
{
    public interface IFieldInfo : IMemberInfo
    {
        ITypeReference Type { get; }
        bool IsReadOnly { get; }
        bool IsConstant { get; }
    }
}