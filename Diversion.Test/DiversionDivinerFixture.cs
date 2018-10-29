using System;
using Xunit;
using Moq;
using Shouldly;

namespace Diversion.Test
{

    public class DiversionDivinerFixture
    {
        [Fact]
        public void DivineCollectionDiversionsShouldCreateAnICollectionDiversionsInstance()
        {
            new DiversionDiviner().DivineCollectionDiversions(
                new[] {Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2)},
                new[] {Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4)}).ShouldNotBeNull();
        }

        [Fact]
        public void ItemsThatOnlyExistInTheNewListShouldBeInTheAddedCollectionOfAnICollectionDiversionsInstance()
        {
            var diversions =
            new DiversionDiviner().DivineCollectionDiversions(
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4) });
            diversions.Added.ShouldHaveSingleItem();
            diversions.Added.ShouldContain(Tuple.Create(1, 4));
        }

        [Fact]
        public void ItemsThatOnlyExistInTheOldListShouldBeInTheRemovedCollectionOfAnICollectionDiversionsInstance()
        {
            var diversions = new DiversionDiviner().DivineCollectionDiversions(
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4) });
            diversions.Removed.ShouldHaveSingleItem();
            diversions.Removed.ShouldContain(Tuple.Create(1, 1));
        }

        [Fact]
        public void DivineItemDiversionsShouldCreateAnIItemDiversionsInstance()
        {
            var repo = new MockRepository(MockBehavior.Default);
            new DiversionDiviner().DivineItemDiversions(new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4) },
                (o, n) =>
                {
                    var x = repo.OneOf<IDiversion<Tuple<int, int>>>();
                    Mock.Get(x).Setup(obj => obj.HasDiverged()).Returns(false).Verifiable();
                    return x;
                }).ShouldNotBeNull();
            repo.Verify();
        }

        [Fact]
        public void DivineDiversionsWithASubdivinerShouldCreateAnIDiversionsInstanceUsingThatSubdivinerToDetermineWhetherItemsHaveDiverged()
        {
            var repo = new MockRepository(MockBehavior.Default);
            new DiversionDiviner().DivineDiversions(new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4) },
                (o, n) =>
                {
                    var x = repo.OneOf<IDiversion<Tuple<int, int>>>();
                    Mock.Get(x).Setup(obj => obj.HasDiverged()).Returns(false).Verifiable();
                    return x;
                }).ShouldNotBeNull();
            repo.Verify();
        }

        [Fact]
        public void ItemsThatHaveDivergedShouldBeInTheDivergedCollectionOfAnIDiversionsInstance()
        {
            new DiversionDiviner().DivineDiversions(new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] { Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4) },
                (o, n) => Mock.Of<IDiversion<Tuple<int, int>>>(obj => obj.HasDiverged())).Diverged.Count.ShouldBe(2);
        }

        [Fact]
        public void ItemsThatHaveNotDivergedShouldNotBeInTheDivergedCollectionOfAnIDiversionsInstance()
        {
            new DiversionDiviner().DivineDiversions(new[] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2) },
                new[] {Tuple.Create(1, 0), Tuple.Create(1, 2), Tuple.Create(1, 4)},
                (o, n) => Mock.Of<IDiversion<Tuple<int, int>>>(obj => !obj.HasDiverged())).Diverged.ShouldBeEmpty();
        }
    }
}
