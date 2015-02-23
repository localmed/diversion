using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    class NvParameterInfo : IParameterInfo
    {
        private readonly IReadOnlyList<IAttributeInfo> _attributes;
        private readonly ITypeReference _type;

        public NvParameterInfo(IReflectionInfoFactory reflectionInfoFactory, ParameterInfo parameter)
        {
            Name = parameter.Name;
            _attributes = parameter.GetCustomAttributesData()
                .Select(reflectionInfoFactory.GetInfo).ToArray();
            _type = reflectionInfoFactory.GetReference(parameter.ParameterType);
        }

        public IReadOnlyList<IAttributeInfo> Attributes
        {
            get { return _attributes; }
        }

        public ITypeReference Type
        {
            get { return _type; }
        }

        public string Name
        {
            get; private set;
        }

        public string Identity
        {
            get { return Name; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as NvParameterInfo;
            return other != null && GetType() == other.GetType() && Identity == other.Identity;
        }

        public override int GetHashCode()
        {
            return (GetType() + Identity).GetHashCode();
        }
    }
}
