using Diversion.Reflection;
using System;
using System.Linq;

namespace Diversion.Cecil
{
    [Serializable]
    public class TypeReference : ITypeReference
    {
        public TypeReference(IReflectionInfoFactory factory, Mono.Cecil.TypeReference type)
        {
            DeclaringType = type.DeclaringType == null ? null : factory.GetReference(type.DeclaringType);
            Namespace = type.Namespace;
            Name = type.IsGenericInstance && type.Name.Contains('`') ? string.Format("{0}<{1}>", type.Name.Substring(0, type.Name.IndexOf('`')), string.Join(",", type.GenericParameters.Select(t => t.IsGenericParameter ? string.Empty : factory.GetReference(t).Identity))) : type.Name;
            IsArray = type.IsArray;
        }

        public string Identity
        {
            get { return DeclaringType == null ? string.Join(".", Namespace, Name) : string.Join("+", DeclaringType, Name); }
        }

        public ITypeReference DeclaringType { get; private set; }

        public string Name { get; private set; }

        public string Namespace { get; private set; }

        public bool IsArray { get; private set; }

        public sealed override string ToString() => Identity;

        public override bool Equals(object obj)
        {
            var other = obj as TypeReference;
            return other != null && GetType() == other.GetType() && Identity == other.Identity;
        }

        public override int GetHashCode()
        {
            return (GetType() + Identity).GetHashCode();
        }

    }
}