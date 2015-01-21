using System;
using System.Collections.Generic;
using System.Reflection;

namespace Diversion
{
    internal class NvMemberInfo : IMemberInfo
    {
        private readonly MemberInfo _memberInfo;

        public NvMemberInfo(MemberInfo memberInfo)
        {
            _memberInfo = memberInfo;
        }

        public IEnumerable<IAttributeInfo> Attributes { get; private set; } 

        public string Name { get; private set; }

        public Type Type
        {
            get;
            private set;
        }

        public IEnumerable<IParameterInfo> Parameters
        {
            get;
            private set;
        }

        public bool IsVirtual
        {
            get;
            private set;
        }

        public bool IsAbstract
        {
            get;
            private set;
        }

        public bool IsPublic { get; private set; }

        public bool IsStatic { get; private set; }
    }
}