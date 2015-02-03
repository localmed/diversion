using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    class NvAttributeInfo : IAttributeInfo
    {
        private readonly ITypeInfo _type;
        private readonly IReadOnlyList<IAttributeArgumentInfo> _arguments;

        public NvAttributeInfo(IReflectionInfoFactory reflectionInfoFactory, CustomAttributeData attribute)
        {
            Contract.Requires(reflectionInfoFactory != null);
            Contract.Requires(attribute != null);
            _type = reflectionInfoFactory.FromReflection(attribute.AttributeType);
            _arguments =
                attribute.Constructor.GetParameters().Zip(attribute.ConstructorArguments,
                    (p, a) => new NvAttributeArgumentInfo(p.Name, a.Value)).ToArray();
            if (attribute.NamedArguments != null)
                _arguments = _arguments.Concat(attribute.NamedArguments.Select(
                    a => new NvAttributeArgumentInfo(a.MemberName, a.TypedValue.Value))).ToArray();
        }

        public ITypeInfo Type
        {
            get { return _type; }
        }

        public IReadOnlyList<IAttributeArgumentInfo> Arguments
        {
            get { return _arguments; }
        }
    }
}