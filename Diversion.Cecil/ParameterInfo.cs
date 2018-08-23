using Diversion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Cecil
{
    public class ParameterInfo : IParameterInfo
    {
        public ParameterInfo(IReflectionInfoFactory reflectionInfoFactory, Mono.Cecil.ParameterDefinition parameter)
        {
            Name = parameter.Name;
            Attributes = parameter.CustomAttributes
                .Select(reflectionInfoFactory.GetInfo).ToArray();
            Type = reflectionInfoFactory.GetReference(parameter.ParameterType);
        }

        public IReadOnlyList<IAttributeInfo> Attributes { get; }

        public ITypeReference Type { get; }

        public string Name { get; }

        public string Identity
        {
            get { return Name; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as ParameterInfo;
            return other != null && GetType() == other.GetType() && Identity == other.Identity;
        }

        public override int GetHashCode()
        {
            return (GetType() + Identity).GetHashCode();
        }

        public sealed override string ToString() => Identity;
    }
}
