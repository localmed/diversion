using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvAttributeInfo : IAttributeInfo
    {
        private readonly ITypeReference _type;
        private readonly IReadOnlyList<IAttributeArgumentInfo> _arguments;

        public NvAttributeInfo(IReflectionInfoFactory reflectionInfoFactory, CustomAttributeData attribute)
        {
            _type = reflectionInfoFactory.GetReference(attribute.AttributeType);
            _arguments =
                attribute.Constructor.GetParameters().Zip(attribute.ConstructorArguments,
                    (p, a) => new NvAttributeArgumentInfo(p.Name, ToSafeArgumentValue(reflectionInfoFactory, a.Value))).ToArray();
            if (attribute.NamedArguments != null)
                _arguments = _arguments.Concat(attribute.NamedArguments.Select(
                    a => new NvAttributeArgumentInfo(a.MemberName, ToSafeArgumentValue(reflectionInfoFactory, a.TypedValue.Value)))).ToArray();
        }

        private object ToSafeArgumentValue(IReflectionInfoFactory reflectionInfoFactory, object value)
        {
            return AsType(reflectionInfoFactory, value as Type) ?? AsString(value);
        }

        private object AsString(object value)
        {
            return value == null ? null : ((value.GetType().Namespace ?? "").StartsWith("System") ? value : value.ToString());
        }

        private object AsType(IReflectionInfoFactory reflectionInfoFactory, Type type)
        {
            return type == null ? null : reflectionInfoFactory.GetReference(type);
        }

        public ITypeReference Type
        {
            get { return _type; }
        }

        public IReadOnlyList<IAttributeArgumentInfo> Arguments
        {
            get { return _arguments; }
        }
    }
}