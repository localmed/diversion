using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Diversion.Reflection;
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
            return new NextVersion(triggers ?? new IVersionTrigger[0], _minorTriggers);
        }

        public NextVersion WithMinorTriggers(params IVersionTrigger[] triggers)
        {
            return new NextVersion(_majorTriggers, triggers ?? new IVersionTrigger[0]);
        }

        public Version Determine(IAssemblyDiversion diversion)
        {
            Contract.Requires<ArgumentNullException>(diversion != null, "diversion must not be null.");
            return
                !Identical(diversion) ?
                    !ShouldIncrementMajor(diversion) ?
                        !ShouldIncrementMinor(diversion) ?
                            diversion.Old.Version.IncrementBuild() :
                        diversion.Old.Version.IncrementMinor() :
                    diversion.Old.Version.IncrementMajor() :
                diversion.Old.Version;
        }

        public Version Determine(IDiversionDiviner diviner, IAssemblyInfo old, IAssemblyInfo @new)
        {
            Contract.Requires<ArgumentNullException>(old != null, "old must not be null.");
            Contract.Requires<ArgumentNullException>(@new != null, "@new must not be null.");
            return Determine(new AssemblyDiversion(diviner, old, @new));
        }

        public Version Determine(IReflectionInfoFactory factory, IDiversionDiviner diviner, string oldAssemblyPath, string newAssemblyPath)
        {
            Contract.Requires<ArgumentNullException>(factory != null, "factory must not be null.");
            Contract.Requires<ArgumentNullException>(oldAssemblyPath != null, "oldAssemblyPath must not be null.");
            Contract.Requires<ArgumentNullException>(newAssemblyPath != null, "newAssemblyPath must not be null.");
            Contract.Requires<ArgumentException>(File.Exists(oldAssemblyPath), "oldAssemblyPath must exist.");
            Contract.Requires<ArgumentException>(File.Exists(newAssemblyPath), "newAssemblyPath must exist.");
            return Determine(diviner, factory.FromFile(oldAssemblyPath), factory.FromFile(newAssemblyPath));
        }

        public Version Determine(string oldAssemblyPath, string newAssemblyPath)
        {
            return Determine(new NvReflectionInfoFactory(), new DiversionDiviner(), oldAssemblyPath, newAssemblyPath);
        }

        private static bool Identical(IAssemblyDiversion diversion)
        {
            return diversion.Old.MD5.SequenceEqual(diversion.New.MD5);
        }

        private bool ShouldIncrementMajor(IAssemblyDiversion diversion)
        {
            return _majorTriggers.Any(trigger => trigger.IsTriggered(diversion));
        }

        private bool ShouldIncrementMinor(IAssemblyDiversion diversion)
        {
            return _minorTriggers.Any(trigger => trigger.IsTriggered(diversion));
        }

    }
}
