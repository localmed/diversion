using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class VirtualMemberNoLongerVirtualTriggerTest
    {
        [TestMethod]
        public void ShouldNotTriggerIfAnyMembersHaveChangedFromNonVirtualToVirtual()
        {
            var change = Mock.Of<IAssemblyChange>(
                obj => obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(
                    tcs => tcs.Changes == new [] {
                        Mock.Of<ITypeChange>(
                            tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo>>(
                                mcs => mcs.Changes == new[] {
                                    Mock.Of<IChange<IMethodInfo>>(
                                        mc => mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == true) &&
                                            mc.Old == Mock.Of<IMethodInfo>(mi =>  mi.IsVirtual == false))
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [TestMethod]
        public void ShouldNotTriggerIfVirtualMembersRemainedVirtualOrNonVirtual()
        {
            var change = Mock.Of<IAssemblyChange>(
                obj => obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(
                    tcs => tcs.Changes == new[] {
                        Mock.Of<ITypeChange>(
                            tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo>>(
                                mcs => mcs.Changes == new[] {
                                    Mock.Of<IChange<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == true) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == true)),
                                    Mock.Of<IChange<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == false) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == false))
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [TestMethod]
        public void ShouldTriggerIfVirtualMembersChangedToNonVirtual()
        {
            var change = Mock.Of<IAssemblyChange>(
                obj => obj.TypeChanges == Mock.Of<IChanges<ITypeInfo, ITypeChange>>(
                    tcs => tcs.Changes == new[] {
                        Mock.Of<ITypeChange>(
                            tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo>>(
                                mcs => mcs.Changes == new[] {
                                    Mock.Of<IChange<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == true) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == false)),
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeTrue();
        }
    }
}
