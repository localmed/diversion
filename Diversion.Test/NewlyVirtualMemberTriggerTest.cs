using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test
{
    [TestClass]
    public class NewlyVirtualMemberTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyMembersHaveChangedFromNonVirtualToVirtual()
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
            new NewlyVirtualMemberTrigger().IsTriggered(change).ShouldBeTrue();
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
            new NewlyVirtualMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [TestMethod]
        public void ShouldNotTriggerIfVirtualMembersChangedToNonVirtual()
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
            new NewlyVirtualMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
