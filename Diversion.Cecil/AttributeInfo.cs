using Diversion.Reflection;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Diversion.Cecil
{
    [Serializable]
    public class AttributeInfo : IAttributeInfo
    {
        private readonly ITypeReference _type;
        private readonly IReadOnlyList<IAttributeArgumentInfo> _arguments;

        public AttributeInfo(IReflectionInfoFactory reflectionInfoFactory, CustomAttribute attribute)
        {
            _type = reflectionInfoFactory.GetReference(attribute.AttributeType);
            _arguments =
                attribute.Constructor.Parameters.Zip(attribute.ConstructorArguments,
                    (p, a) => new AttributeArgumentInfo(p.Name, GetArgumentValue(reflectionInfoFactory, a))).ToArray();
            if (attribute.HasFields)
                _arguments = _arguments.Concat(attribute.Fields.Select(
                    a => new AttributeArgumentInfo(a.Name, GetArgumentValue(reflectionInfoFactory, a.Argument)))).ToArray();
            if (attribute.HasProperties)
                _arguments = _arguments.Concat(attribute.Properties.Select(
                    a => new AttributeArgumentInfo(a.Name, GetArgumentValue(reflectionInfoFactory, a.Argument)))).ToArray();
        }

        private object GetArgumentValue(IReflectionInfoFactory reflectionInfoFactory, CustomAttributeArgument argument)
        {
            if (Equals(argument.Type.FullName, typeof(Type).FullName))
                return reflectionInfoFactory.GetReference((Mono.Cecil.TypeReference)argument.Value);
            else if (argument.Type.IsArray)
                return ((IEnumerable)argument.Value).OfType<CustomAttributeArgument>().Select(e => GetArgumentValue(reflectionInfoFactory, e)).ToArray();
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
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AttributeInfo) obj);
        }

        private bool Equals(AttributeInfo other)
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