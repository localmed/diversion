using Diversion.Reflection;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diversion.Cecil
{
    public abstract class MemberInfo : IMemberInfo
    {
        protected MemberInfo(IReflectionInfoFactory reflectionInfoFactory, IMemberDefinition member)
        {
            var baseDeclaringType = member.GetBaseDeclaringType();
            BaseDeclaringType = baseDeclaringType == null ? null : reflectionInfoFactory.GetReference(baseDeclaringType);
            DeclaringType = member.DeclaringType == null ? null : reflectionInfoFactory.GetReference(member.DeclaringType);
            Attributes = member.CustomAttributes.Select(reflectionInfoFactory.GetInfo).ToArray();
            Name = member.Name;
        }

        public ITypeReference BaseDeclaringType { get; }

        public ITypeReference DeclaringType { get; }

        public IReadOnlyList<IAttributeInfo> Attributes { get; }

        public string Name { get; protected set; }

        public abstract bool IsPublic { get; }

        public abstract bool IsStatic { get; }

        public abstract bool IsOnApiSurface { get; }

        public override bool Equals(object obj)
        {
            var other = obj as MemberInfo;
            return other != null && GetType() == other.GetType() && Identity == other.Identity;
        }

        public override int GetHashCode()
        {
            return (GetType() + Identity).GetHashCode();
        }

        public virtual string Identity
        {
            get { return string.Join(".", BaseDeclaringType, Name); }
        }

        public sealed override string ToString()
        {
            return Identity;
        }
    }
}