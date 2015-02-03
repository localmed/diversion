using System;
using System.Reflection;

namespace Diversion.Reflection
{
    internal class NvGenericParameterInfo : NvTypeInfo, IGenericParameterInfo
    {
        public NvGenericParameterInfo(Type member) : base(member)
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