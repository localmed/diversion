using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    class NvAttributeInfo : IAttributeInfo
    {
        private readonly ITypeReference _type;
        private readonly IReadOnlyList<IAttributeArgumentInfo> _arguments;

        public NvAttributeInfo(IReflectionInfoFactory reflectionInfoFactory, CustomAttributeData attribute)
        {
            _type = reflectionInfoFactory.GetReference(attribute.AttributeType);
            _arguments =
                attribute.Constructor.GetParameters().Zip(attribute.ConstructorArguments,
                    (p, a) => new NvAttributeArgumentInfo(p.Name, GetArgumentValue(reflectionInfoFactory, a))).ToArray();
            if (attribute.NamedArguments != null)
                _arguments = _arguments.Concat(attribute.NamedArguments.Select(
                    a => new NvAttributeArgumentInfo(a.MemberName, GetArgumentValue(reflectionInfoFactory, a.TypedValue)))).ToArray();
        }

        private object GetArgumentValue(IReflectionInfoFactory reflectionInfoFactory, CustomAttributeTypedArgument argument)
        {
            if (Equals(argument.ArgumentType, typeof(Type)))
                return reflectionInfoFactory.GetReference((Type)argument.Value);
            else if (argument.ArgumentType.IsArray)
                return ((IEnumerable)argument.Value).OfType<CustomAttributeTypedArgument>().Select(e => GetArgumentValue(reflectionInfoFactory, e)).ToArray();
            return argument.Value;
        }

        public ITypeReference Type
        {
            get { return _type; }
        }

        public IReadOnlyList<IAttributeArgumentInfo> Arguments
        {
            get { return _arguments; }
        }

        public sealed override string ToString() => Identity;

        public string Identity => $"{Type.Identity}({string.Join(", ", Arguments.Select(a => $"{a.Name}={a.Value}"))})";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((NvAttributeInfo) obj);
        }

        private bool Equals(NvAttributeInfo other)
        {
            return _type.Equals(other._type) && _arguments.SequenceEqual(other._arguments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_type.GetHashCode() * 397) ^ _arguments.Take(16).Aggregate(19, (result, item) => result * 31 + item.GetHashCode());
            }
        }
    }
}