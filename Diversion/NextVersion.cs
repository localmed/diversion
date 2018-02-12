using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Diversion.Triggers;

namespace Diversion
{
    public class NextVersion
    {
        private readonly IVersionTrigger[] _majorTriggers;
        private readonly IVersionTrigger[] _minorTriggers;

        public NextVersion()
            : this(
                new IVersionTrigger[]
                {
                    new NewerFrameworkVersionTrigger(),
                    new TypeRemovalTrigger(),
                    new NewInterfaceImplementationOnInterfaceTrigger(),
                    new InterfaceImplementationRemovalTrigger(),
                    new MemberRemovalTrigger(),
                    new NewAbstractMemberTrigger(),
                    new NewMemberOnInterfaceTrigger(),
                    new VirtualMemberNoLongerVirtualTrigger(),
                },
                new IVersionTrigger[]
                {
                    new NewTypeTrigger(),
                    new NewMemberTrigger(),
                    new NewlyVirtualMemberTrigger(),
                    new NewInterfaceImplementationTrigger()
                })
        {
        }

        public NextVersion(IVersionTrigger[] majorTriggers, IVersionTrigger[] minorTriggers)
        {
            _majorTriggers = majorTriggers ?? new IVersionTrigger[0];
            _minorTriggers = minorTriggers ?? new IVersionTrigger[0];
        }

        public NextVersion WithMajorTriggers(params IVersionTrigger[] triggers)
        {
            Contract.Ensures(Contract.Result<NextVersion>() != null);
            return new NextVersion(triggers ?? new IVersionTrigger[0], _minorTriggers);
        }

        public NextVersion WithMinorTriggers(params IVersionTrigger[] triggers)
        {
            Contract.Ensures(Contract.Result<NextVersion>() != null);
            return new NextVersion(_majorTriggers, triggers ?? new IVersionTrigger[0]);
        }

        public bool IsCorrect(IAssemblyDiversion diversion)
        {
            Contract.Requires(diversion != null);
            return Determine(diversion).Equals(diversion.New.Version);
        }

        public Version Determine(IAssemblyDiversion diversion)
        {
            Contract.Requires(diversion != null);
            Contract.Ensures(Contract.Result<Version>() != null);
            return
                diversion.HasDiverged() ?
                    !ShouldIncrementMajor(diversion) ?
                        !ShouldIncrementMinor(diversion) ?
                            diversion.Old.Version.IncrementBuild() :
                        diversion.Old.Version.IncrementMinor() :
                    diversion.Old.Version.IncrementMajor() :
                diversion.Old.Version;
        }

        private bool ShouldIncrementMajor(IAssemblyDiversion diversion)
        {
            Contract.Requires(diversion != null);
            return diversion.Old.Version.Major == 0 && diversion.New.Version.Major == 1 ||
                diversion.Old.Version.Major > 0 && _majorTriggers.Any(trigger => trigger.IsTriggered(diversion));
        }

        private bool ShouldIncrementMinor(IAssemblyDiversion diversion)
        {
            Contract.Requires(diversion != null);
            return diversion.Old.Version.Major == 0 && _majorTriggers.Any(trigger => trigger.IsTriggered(diversion)) ||
                _minorTriggers.Any(trigger => trigger.IsTriggered(diversion));
        }

    }
}
