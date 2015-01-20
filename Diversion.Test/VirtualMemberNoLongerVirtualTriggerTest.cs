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
                            tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo, IMemberChange>>(
                                mcs => mcs.Changes == new[] {
                                    Mock.Of<IMemberChange>(
                                        mc => mc.New == Mock.Of<IMemberInfo>(mi => mi.IsVirtual == true && mi.IsNonVirtual == false) &&
                                            mc.Old == Mock.Of<IMemberInfo>(mi => mi.IsNonVirtual == true && mi.IsVirtual == false))
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
                            tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo, IMemberChange>>(
                                mcs => mcs.Changes == new[] {
                                    Mock.Of<IMemberChange>(
                                        mc => mc.Old == Mock.Of<IMemberInfo>(mi => mi.IsVirtual == true) &&
                                            mc.New == Mock.Of<IMemberInfo>(mi => mi.IsVirtual == true)),
                                    Mock.Of<IMemberChange>(
                                        mc => mc.Old == Mock.Of<IMemberInfo>(mi => mi.IsNonVirtual == true) &&
                                            mc.New == Mock.Of<IMemberInfo>(mi => mi.IsNonVirtual == true))
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
                            tc => tc.MemberChanges == Mock.Of<IChanges<IMemberInfo, IMemberChange>>(
                                mcs => mcs.Changes == new[] {
                                    Mock.Of<IMemberChange>(
                                        mc => mc.Old == Mock.Of<IMemberInfo>(mi => mi.IsVirtual == true && mi.IsNonVirtual == false) &&
                                            mc.New == Mock.Of<IMemberInfo>(mi => mi.IsNonVirtual == true && mi.IsVirtual == false)),
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeTrue();
        }
    }
}
