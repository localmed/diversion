using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class NewlyVirtualMemberTriggerTest
    {
        [Fact]
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

        [Fact]
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

        [Fact]
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
