using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diversion.Reflection
{
    [Serializable]
    public class NvGenericParameterInfo : NvTypeReference, IGenericParameterInfo
    {
        public NvGenericParameterInfo(IReflectionInfoFactory reflectionInfoFactory, Type member) : base(reflectionInfoFactory, member)
        {
            RequiresDefaultConstructor =
                member.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint);
            TypeRequirement =
                member.GenericParameterAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint)
                    ? GenericTypeRequirement.Struct
                    : member.GenericParameterAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint)
                        ? GenericTypeRequirement.Class
                        : GenericTypeRequirement.None;
            TypeVariance =
                member.GenericParameterAttributes.HasFlag(GenericParameterAttributes.Covariant)
                    ? GenericTypeVariance.Covariant
                    : member.GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant)
                        ? GenericTypeVariance.Contravariant
                        : GenericTypeVariance.None;
            Base = member.BaseType == null ? null : reflectionInfoFactory.GetReference(member.BaseType);
            Interfaces = member.GetInterfaces().Select(reflectionInfoFactory.GetReference).ToArray();
        }

        public ITypeReference Base { get; private set; }

        public IReadOnlyList<ITypeReference> Interfaces { get; private set; }

        public bool RequiresDefaultConstructor { get; private set; }

        public GenericTypeRequirement TypeRequirement { get; private set; }

        public GenericTypeVariance TypeVariance { get; private set; }
    }
}