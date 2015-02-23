using System;
using Diversion.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;
using Should.Fluent;

namespace Diversion.Test
{
    [TestClass]
    public class DiversionDivinerFacts
    {
        [TestMethod]
        public void DivineCollectionDiversionsShouldCreateAnICollectionDiversionsInstance()
        {
            new DiversionDiviner().DivineCollectionDiversions(
                new[] {Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2)},
                new[] {Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4)}).Should().Not.Be.Null();
        }

        [TestMethod]
        public void ItemsThatOnlyExistInTheNewListShouldBeInTheAddedCollectionOfAnICollectionDiversionsInstance()
        {
            new DiversionDiviner().DivineCollectionDiversions(
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4) }).Added.Should().Contain.One(Tuple.Create(1, 4));
        }

        [TestMethod]
        public void ItemsThatOnlyExistInTheOldListShouldBeInTheRemovedCollectionOfAnICollectionDiversionsInstance()
        {
            new DiversionDiviner().DivineCollectionDiversions(
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4) }).Removed.Should().Contain.One(Tuple.Create(1, 1));
        }

        [TestMethod]
        public void DivineItemDiversionsShouldCreateAnIItemDiversionsInstance()
        {
            var repo = new MockRepository(MockBehavior.Default);
            new DiversionDiviner().DivineItemDiversions(new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4) },
                (o, n) =>
                {
                    var x = repo.OneOf<IDiversion<Tuple<int, int>>>();
                    Mock.Get(x).Setup(obj => obj.HasDiverged()).Returns(true).Verifiable();
                    return x;
                }).Should().Not.Be.Null();
            repo.VerifyAll();
        }


        [TestMethod]
        public void DivineDiversionsWithASubdivinerShouldCreateAnIDiversionsInstanceUsingThatSubdivinerToDetermineWhetherItemsHaveDiverged()
        {
            var repo = new MockRepository(MockBehavior.Default);
            new DiversionDiviner().DivineDiversions(new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4) },
                (o, n) =>
                {
                    var x = repo.OneOf<IDiversion<Tuple<int, int>>>();
                    Mock.Get(x).Setup(obj => obj.HasDiverged()).Returns(true).Verifiable();
                    return x;
                }).Should().Not.Be.Null();
            repo.VerifyAll();
        }

        [TestMethod]
        public void ItemsThatHaveDivergedShouldBeInTheDivergedCollectionOfAnIDiversionsInstance()
        {
            new DiversionDiviner().DivineDiversions(new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] {Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4)},
                (o, n) => Mock.Of<IDiversion<Tuple<int, int>>>(obj => obj.HasDiverged())).Diverged.Should().Count.Exactly(2);
        }

        [TestMethod]
        public void ItemsThatHaveNotDivergedShouldNotBeInTheDivergedCollectionOfAnIDiversionsInstance()
        {
            new DiversionDiviner().DivineDiversions(new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] {Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4)},
                (o, n) => Mock.Of<IDiversion<Tuple<int, int>>>(obj => !obj.HasDiverged())).Diverged.Should().Be.Empty();
        }
    }
}
