using System;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvTypeReference : ITypeReference
    {
        public NvTypeReference(IReflectionInfoFactory factory, Type type)
        {
            DeclaringType = type.DeclaringType == null ? null : factory.GetReference(type.DeclaringType);
            Namespace = type.Namespace;
            Name = type.Name;
        }

        public string Identity
        {
            get { return DeclaringType == null ? string.Join(".", Namespace, Name) : string.Join("+", DeclaringType, Name); }
        }

        public ITypeReference DeclaringType { get; private set; }

        public string Name { get; private set; }

        public string Namespace { get; private set; }
        public sealed override string ToString()
        {
            return Identity;
        }

        public override bool Equals(object obj)
        {
            var other = obj as NvTypeReference;
            return other != null && GetType() == other.GetType() && Identity == other.Identity;
        }

        public override int GetHashCode()
        {
            return (GetType() + Identity).GetHashCode();
        }

    }
}