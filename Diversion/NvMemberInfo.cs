using System;
using System.Collections.Generic;
using System.Reflection;

namespace Diversion
{
    internal class NvMemberInfo : IMemberInfo
    {
        public NvMemberInfo(string name, MemberTypes memberType)
        {
            Name = name;
            MemberType = memberType;
            Parameters = new IParameterInfo[0];
        }

        public MemberTypes MemberType
        {
            get; private set; }

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
    }
}