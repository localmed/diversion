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
        private readonly Lazy<ReadOnlyCollection<ITypeInfo>> _interfaces;
        private readonly Lazy<ReadOnlyCollection<IMemberInfo>> _members;
        private readonly Lazy<ITypeInfo> _base;

        public NvTypeInfo(Type type) : base(type)
        {
            _type = type;
            _base = new Lazy<ITypeInfo>(() => new NvTypeInfo(_type.BaseType), true);
            _interfaces = new Lazy<ReadOnlyCollection<ITypeInfo>>(() => new ReadOnlyCollection<ITypeInfo>(_type
                .GetInterfaces()
                .Select(t => new NvTypeInfo(t)).Cast<ITypeInfo>().ToArray()), true);
            _members = new Lazy<ReadOnlyCollection<IMemberInfo>>(() => new ReadOnlyCollection<IMemberInfo>(_type
                .GetMembers(BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic)
                .Where(m => m.IsPublicOrProtected())
                .Select(FromMemberInfo).ToArray()), true);
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

        public IEnumerable<ITypeInfo> Interfaces
        {
            get { return _interfaces.Value; }
        }

        public IEnumerable<IMemberInfo> Members
        {
            get { return _members.Value; }
        }

    }
}