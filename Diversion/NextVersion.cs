using System;
using System.Linq;

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
            _majorTriggers = majorTriggers;
            _minorTriggers = minorTriggers;
        }

        public NextVersion WithMajorTriggers(params IVersionTrigger[] triggers)
        {
            return new NextVersion(triggers, _minorTriggers);
        }

        public NextVersion WithMinorTriggers(params IVersionTrigger[] triggers)
        {
            return new NextVersion(_majorTriggers, triggers);
        }

        public Version Determine(IAssemblyChange change)
        {
            return
                !Identical(change) ?
                    !ShouldIncrementMajor(change) ?
                        !ShouldIncrementMinor(change) ?
                            change.Old.Version.IncrementBuild() :
                        change.Old.Version.IncrementMinor() :
                    change.Old.Version.IncrementMajor() :
                change.Old.Version;
        }

        private static bool Identical(IAssemblyChange change)
        {
            return change.Old.MD5.SequenceEqual(change.New.MD5);
        }

        private bool ShouldIncrementMajor(IAssemblyChange change)
        {
            return _majorTriggers.Any(trigger => trigger.IsTriggered(change));
        }

        private bool ShouldIncrementMinor(IAssemblyChange change)
        {
            return _minorTriggers.Any(trigger => trigger.IsTriggered(change));
        }
    }
}
