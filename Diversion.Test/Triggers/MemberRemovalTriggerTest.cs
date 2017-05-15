using Diversion.Reflection;
using Diversion.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;

namespace Diversion.Test.Triggers
{
    [TestClass]
    public class MemberRemovalTriggerTest
    {
        [TestMethod]
        public void ShouldTriggerIfAnyPublicMembersOfAnyPublicTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new [] {
                        // API Surface type with removed API Surface member.
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(t => t.IsOnApiSurface == true) &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mc =>
                                mc.Diverged == new IDiversion<IMemberInfo>[0] &&
                                mc.Removed == new[] {Mock.Of<IMemberInfo>( m => m.IsOnApiSurface == true)}))}));
            new MemberRemovalTrigger().IsTriggered(change).ShouldBeTrue();
        }

        [TestMethod]
        public void ShouldNotTriggerIfNoPublicMembersOfAnyTypesHaveBeenRemoved()
        {
            var change = Mock.Of<IAssemblyDiversion>(
                obj => obj.TypeDiversions == Mock.Of<IDiversions<ITypeInfo, ITypeDiversion>>(
                    tcs => tcs.Diverged == new[] {
                        // API Surface type without removed API Surface member.
                        Mock.Of<ITypeDiversion>(tc =>
                            tc.New == Mock.Of<ITypeInfo>(t => t.IsOnApiSurface == true) &&
                            tc.MemberDiversions == Mock.Of<IDiversions<IMemberInfo>>(
                                mc =>
                                mc.Diverged == new IDiversion<IMemberInfo>[0] &&
                                mc.Removed == new IMemberInfo[0]))}));
            new MemberRemovalTrigger().IsTriggered(change).ShouldBeFalse();
        }
    }
}
