using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Diversion.Reflection
{
    class NvGenericParameterInfo : NvTypeInfo, IGenericParameterInfo
    {
        public NvGenericParameterInfo(IReflectionInfoFactory reflectionInfoFactory, Type member) : base(reflectionInfoFactory, member)
        {
            Contract.Requires(reflectionInfoFactory != null);
            Contract.Requires(member != null);
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
        }

        public bool RequiresDefaultConstructor
        {
            get;private set;
        }

        public GenericTypeRequirement TypeRequirement
        {
            get;private set;
        }

        public GenericTypeVariance TypeVariance
        {
            get;private set;
        }
    }
}