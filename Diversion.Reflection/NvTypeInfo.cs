using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    class NvTypeInfo : NvMemberInfo, ITypeInfo
    {
        private readonly bool _isPublic;
        private readonly bool _isStatic;
        private readonly bool _isOnApiSurface;

        public NvTypeInfo(IReflectionInfoFactory reflectionInfoFactory, Type type) : base(reflectionInfoFactory, type)
        {
            Base = type.BaseType == null ? null : reflectionInfoFactory.GetReference(type.BaseType);
            Interfaces = type.GetInterfaces().Where(t => t.IsPublicOrProtected()).Select(reflectionInfoFactory.GetReference).OrderBy(i => i.Identity).ToArray();
            Members = type
                .GetMembers(BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.DeclaredOnly)
                .Where(m => m.IsPublicOrProtected())
                .Select(reflectionInfoFactory.GetInfo).OrderBy(i => i.Identity).ToArray();
            GenericArguments = type.GetGenericArguments().Select(reflectionInfoFactory.GetReference).ToArray();
            _isPublic = type.IsPublic;
            _isStatic = type.IsSealed && type.IsAbstract && type.IsClass;
            _isOnApiSurface = type.IsPublicOrProtected();
            IsGenericType = type.IsGenericType;
            IsInterface = type.IsInterface;
            IsAbstract = type.IsAbstract;
            IsArray = type.IsArray;
            Namespace = type.Namespace;
            Name = type.IsGenericType && type.Name.Contains('`') ? string.Format("{0}<{1}>", type.Name.Substring(0, type.Name.IndexOf('`')), string.Join(",", type.GetGenericArguments().Select(t => t.IsGenericParameter ? string.Empty : reflectionInfoFactory.GetReference(t).Identity))) : type.Name;
        }

        // Maybe later, no use for this now
        //private IEnumerable<MemberInfo> GetOverriddenMemberAsWell(MemberInfo member)
        //{
        //    var method = member as MethodInfo;
        //    if (method?.IsOverride() ?? false)
        //        yield return method.GetBaseDefinition();
        //    var property = member as PropertyInfo;
        //    if (property?.IsOverride() ?? false)
        //        yield return property.GetBaseDefinition();
        //    var @event = member as EventInfo;
        //    if (@event?.IsOverride() ?? false)
        //        yield return @event.GetBaseDefinition();
        //    yield return member;
        //}

        public override bool IsPublic => _isPublic;

        public override bool IsStatic => _isStatic;

        public override bool IsOnApiSurface => _isOnApiSurface;

        public bool IsArray { get; }

        public bool IsAbstract { get; }

        public bool IsInterface { get; }

        public string Namespace { get; }

        public ITypeReference Base { get; }

        public IReadOnlyList<ITypeReference> Interfaces { get; }

        public IReadOnlyList<IMemberInfo> Members { get; }

        public bool IsGenericType { get; }

        public IReadOnlyList<ITypeReference> GenericArguments { get; }

        public override string Identity
        {
            get { return BaseDeclaringType == null ? string.Join(".", Namespace, Name) : string.Join("+", BaseDeclaringType, Name); }
        }
    }
}