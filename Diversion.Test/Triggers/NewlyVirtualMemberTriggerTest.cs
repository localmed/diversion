using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class NewlyVirtualMemberTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyMembersHaveChangedFromNonVirtualToVirtual()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new [] {
                        Mock.Of<ITypeDiversion>(
                            tc => tc.New.IsOnApiSurface && tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mcs => mcs.Diverged == new[] {
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == true && mi.IsOnApiSurface) &&
                                            mc.Old == Mock.Of<IMethodInfo>(mi =>  mi.IsVirtual == false && mi.IsOnApiSurface))
                                }))
                    }));
            new NewlyVirtualMemberTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfVirtualMembersRemainedVirtualOrNonVirtual()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(
                            tc => tc.New.IsOnApiSurface && tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mcs => mcs.Diverged == new[] {
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == true && mi.IsOnApiSurface) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == true && mi.IsOnApiSurface)),
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == false && mi.IsOnApiSurface) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == false && mi.IsOnApiSurface))
                                }))
                    }));
            new NewlyVirtualMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [TestMethod]
        public void ShouldNotTriggerIfVirtualMembersChangedToNonVirtual()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(
                            tc => tc.New.IsOnApiSurface && tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mcs => mcs.Diverged == new[] {
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == true && mi.IsOnApiSurface) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual == false && mi.IsOnApiSurface)),
                                }))
                    }));
            new NewlyVirtualMemberTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
