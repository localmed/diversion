using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class VirtualMemberNoLongerVirtualTriggerTest
    {
        [TestMethod]
        public void ShouldNotTriggerIfAnyMembersHaveChangedFromNonVirtualToVirtual()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new [] {
                        Mock.Of<ITypeDiversion>(
                            tc => tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mcs => mcs.Diverged == new[] {
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual) &&
                                            mc.Old == Mock.Of<IMethodInfo>(mi => !mi.IsVirtual))
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [TestMethod]
        public void ShouldNotTriggerIfVirtualMembersRemainedVirtualOrNonVirtual()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(
                            tc => tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mcs => mcs.Diverged == new[] {
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual)),
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => !mi.IsVirtual) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => !mi.IsVirtual))
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [TestMethod]
        public void ShouldTriggerIfVirtualMembersChangedToNonVirtual()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(
                            tc => tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mcs => mcs.Diverged == new[] {
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => !mi.IsVirtual)),
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeTrue();
        }
    }
}
