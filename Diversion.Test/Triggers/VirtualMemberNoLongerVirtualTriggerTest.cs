﻿using Diversion.Reflection;
using Diversion.Triggers;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test.Triggers
{

    public class VirtualMemberNoLongerVirtualTriggerTest
    {
        [Fact]
        public void ShouldNotTriggerIfAnyMembersHaveChangedFromNonVirtualToVirtual()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new [] {
                        Mock.Of<ITypeDiversion>(
                            tc => tc.New.IsOnApiSurface && tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mcs => mcs.Diverged == new[] {
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual && mi.IsOnApiSurface) &&
                                            mc.Old == Mock.Of<IMethodInfo>(mi => !mi.IsVirtual && mi.IsOnApiSurface))
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeFalse();
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
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual && mi.IsOnApiSurface) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => mi.IsVirtual && mi.IsOnApiSurface)),
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => !mi.IsVirtual && mi.IsOnApiSurface) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => !mi.IsVirtual && mi.IsOnApiSurface))
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeFalse();
        }

        [Fact]
        public void ShouldTriggerIfVirtualMembersChangedToNonVirtual()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {
                        Mock.Of<ITypeDiversion>(
                            tc => tc.New.IsOnApiSurface && tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mcs => mcs.Diverged == new[] {
                                    Mock.Of<IDiversion<IMethodInfo>>(
                                        mc => mc.Old == Mock.Of<IMethodInfo>(mi => mi.IsVirtual && mi.IsOnApiSurface) &&
                                            mc.New == Mock.Of<IMethodInfo>(mi => !mi.IsVirtual && mi.IsOnApiSurface)),
                                }))
                    }));
            new VirtualMemberNoLongerVirtualTrigger().IsTriggered(change).ShouldBeTrue();
        }
    }
}
