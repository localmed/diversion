using System;
using System.Linq;
using Diversion.Reflection;
using Diversion.Triggers;
using NuGet.Frameworks;
using NuGet.Versioning;

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
            return new NextVersion(triggers ?? new IVersionTrigger[0], _minorTriggers);
        }

        public NextVersion WithMinorTriggers(params IVersionTrigger[] triggers)
        {
            return new NextVersion(_majorTriggers, triggers ?? new IVersionTrigger[0]);
        }

        public NuGetVersion Determine(IAssemblyDiversion diversion)
        {
            return
                diversion.HasDiverged() ?
                    !ShouldIncrementMajor(diversion) ?
                        !ShouldIncrementMinor(diversion) ?
                            diversion.Old.Version.IncrementPatch() :
                        diversion.Old.Version.IncrementMinor() :
                    diversion.Old.Version.IncrementMajor() :
                diversion.Old.Version;
        }

        public NextVersionAnalysis Analyze(IAssemblyDiversion diversion)
        {
            return new NextVersionAnalysis(diversion.Identity, diversion.New.TargetFramework,
                diversion.Old.Version, diversion.New.Version, Determine(diversion));
        }

        private bool ShouldIncrementMajor(IAssemblyDiversion diversion)
        {
            return diversion.Old.Version.Major == 0 && diversion.New.Version.Major == 1 ||
                diversion.Old.Version.Major > 0 && _majorTriggers.Any(trigger => trigger.IsTriggered(diversion));
        }

        private bool ShouldIncrementMinor(IAssemblyDiversion diversion)
        {
            return diversion.Old.Version.Major == 0 && _majorTriggers.Any(trigger => trigger.IsTriggered(diversion)) ||
                _minorTriggers.Any(trigger => trigger.IsTriggered(diversion));
        }
    }

    public class NextVersionAnalysis
    {
        public NextVersionAnalysis(string identity, NuGetFramework targetFramework,
            NuGetVersion oldVersion, NuGetVersion newVersion, NuGetVersion calculatedVersion)
        {
            Identity = identity;
            TargetFramework = targetFramework;
            OldVersion = oldVersion;
            NewVersion = newVersion;
            CalculatedVersion = calculatedVersion;
        }

        public string Identity { get; }
        public NuGetFramework TargetFramework { get; }
        public NuGetVersion OldVersion { get; }
        public NuGetVersion NewVersion { get; }
        public NuGetVersion CalculatedVersion { get; }
        public bool HasDiverged => !Equals(OldVersion, CalculatedVersion);
        public bool IsNewVersionCorrect => NewVersion >= CalculatedVersion;
    }
}
