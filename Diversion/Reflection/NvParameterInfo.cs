using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvParameterInfo : IParameterInfo
    {
        private readonly IReadOnlyList<IAttributeInfo> _attributes;
        private readonly ITypeReference _type;

        public NvParameterInfo(IReflectionInfoFactory reflectionInfoFactory, ParameterInfo parameter)
        {
            Contract.Requires(reflectionInfoFactory != null);
            Contract.Requires(parameter != null);
            Name = parameter.Name;
            _attributes = parameter.GetCustomAttributesData()
                .Select(reflectionInfoFactory.FromReflection).ToArray();
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
            get;private set;
        }

        public string Identity
        {
            get { return Name; }
        }
    }
}
