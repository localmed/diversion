using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    internal class NvTypeInfo : NvMemberInfo, ITypeInfo
    {
        private readonly Type _type;
        private readonly Lazy<IReadOnlyList<ITypeInfo>> _interfaces;
        private readonly Lazy<IReadOnlyList<IMemberInfo>> _members;
        private readonly Lazy<IReadOnlyList<ITypeInfo>> _genericArguments;
        private readonly Lazy<ITypeInfo> _base;

        public NvTypeInfo(Type type) : base(type)
        {
            _type = type;
            _base = new Lazy<ITypeInfo>(() => (ITypeInfo)FromMemberInfo(_type.BaseType), true);
            _interfaces = new Lazy<IReadOnlyList<ITypeInfo>>(_type.GetInterfaces()
                .Select(FromMemberInfo).Cast<ITypeInfo>().ToArray, true);
            _members = new Lazy<IReadOnlyList<IMemberInfo>>(_type
                .GetMembers(BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic)
                .Where(m => m.IsPublicOrProtected()).Select(FromMemberInfo).ToArray, true);
            _genericArguments = new Lazy<IReadOnlyList<ITypeInfo>>(_type.GetGenericArguments()
                .Select(a => (ITypeInfo)FromMemberInfo(a)).ToArray, true);
        }

        public override bool IsPublic
        {
            get { return _type.IsPublic; }
        }

        public override bool IsStatic
        {
            get { return _type.IsSealed && _type.IsAbstract && _type.IsClass; }
        }

        public bool IsAbstract
        {
            get { return _type.IsAbstract; }
        }

        public bool IsInterface
        {
            get { return _type.IsInterface; }
        }

        public string Namespace
        {
            get { return _type.Namespace; }
        }

        public ITypeInfo Base
        {
            get { return _base.Value; }
        }

        public IReadOnlyList<ITypeInfo> Interfaces
        {
            get { return _interfaces.Value; }
        }

        public IReadOnlyList<IMemberInfo> Members
        {
            get { return _members.Value; }
        }

        public bool IsGenericType
        {
            get { return _type.IsGenericType; }
        }

        public IReadOnlyList<ITypeInfo> GenericArguments
        {
            get { return _genericArguments.Value; }
        }

        public override string Identity
        {
            get { return DeclaringType == null ? string.Join(".", Namespace, Name) : string.Join("+", DeclaringType, Name); }
        }
    }
}