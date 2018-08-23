using Diversion.Reflection;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diversion.Cecil
{
    public class GenericParameterInfo : TypeReference, IGenericParameterInfo
    {
        public GenericParameterInfo(IReflectionInfoFactory reflectionInfoFactory, GenericParameter member) : base(reflectionInfoFactory, member)
        {
            RequiresDefaultConstructor =
                member.Attributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint);
            TypeRequirement =
                member.Attributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint)
                    ? GenericTypeRequirement.Struct
                    : member.Attributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint)
                        ? GenericTypeRequirement.Class
                        : GenericTypeRequirement.None;
            TypeVariance =
                member.Attributes.HasFlag(GenericParameterAttributes.Covariant)
                    ? GenericTypeVariance.Covariant
                    : member.Attributes.HasFlag(GenericParameterAttributes.Contravariant)
                        ? GenericTypeVariance.Contravariant
                        : GenericTypeVariance.None;
            Base = member.Constraints.Where(t => !t.Resolve().IsInterface).Select(reflectionInfoFactory.GetReference).FirstOrDefault();
            Interfaces = member.Constraints.Where(t => t.Resolve().IsInterface).Select(reflectionInfoFactory.GetReference).ToArray();
        }

        public ITypeReference Base { get; private set; }

        public IReadOnlyList<ITypeReference> Interfaces { get; private set; }

        public bool RequiresDefaultConstructor { get; private set; }

        public GenericTypeRequirement TypeRequirement { get; private set; }

        public GenericTypeVariance TypeVariance { get; private set; }
    }
}