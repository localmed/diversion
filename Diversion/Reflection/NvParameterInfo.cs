using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    class NvParameterInfo : IParameterInfo
    {
        private readonly ParameterInfo _parameter;
        private readonly Lazy<IReadOnlyList<IAttributeInfo>> _attributes;
        private readonly ITypeInfo _type;

        public NvParameterInfo(IReflectionInfoFactory reflectionInfoFactory, ParameterInfo parameter)
        {
            Contract.Requires(reflectionInfoFactory != null);
            Contract.Requires(parameter != null);
            _parameter = parameter;
            _attributes = new Lazy<IReadOnlyList<IAttributeInfo>>(_parameter.GetCustomAttributesData()
                .Select(reflectionInfoFactory.FromReflection).ToArray, true);
            _type = reflectionInfoFactory.FromReflection(_parameter.ParameterType);

        }

        public IReadOnlyList<IAttributeInfo> Attributes
        {
            get { return _attributes.Value; }
        }

        public ITypeInfo Type
        {
            get { return _type; }
        }

        public string Name
        {
            get { return _parameter.Name; }
        }

        public string Identity
        {
            get { return Name; }
        }
    }
}
