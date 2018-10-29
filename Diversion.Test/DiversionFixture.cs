using System;
using Xunit;
using Shouldly;

namespace Diversion.Test
{

    public class DiversionFixture
    {
        [Fact]
        public void OldShouldBeSetCorrectly()
        {
            var old = Tuple.Create(1, 0, 0);
            var @new = Tuple.Create(1, 0, 1);
            new DiversionBase<Tuple<int, int, int>>(old, @new).Old.ShouldBeSameAs(old);
        }

        [Fact]
        public void NewShouldBeSetCorrectly()
        {
            var old = Tuple.Create(1, 0, 0);
            var @new = Tuple.Create(1, 0, 1);
            new DiversionBase<Tuple<int, int, int>>(old, @new).New.ShouldBeSameAs(@new);
        }

        [Fact]
        public void HasDivergedReturnsTrueIfAnyPropertyDiffersBetweenTwoObjects()
        {
            new DiversionBase<Tuple<int, int, int>>(Tuple.Create(1, 0, 0), Tuple.Create(2, 0, 0)).HasDiverged().ShouldBeTrue();
            new DiversionBase<Tuple<int, int, int[]>>(Tuple.Create(1, 0, new []{0}), Tuple.Create(1, 0, new [] {1})).HasDiverged().ShouldBeTrue();
        }

        [Fact]
        public void HasDivergedReturnsFalseIfNoPropertiesDifferBetweenTwoObjects()
        {
            new DiversionBase<Tuple<int, int, int>>(Tuple.Create(1, 0, 0), Tuple.Create(1, 0, 0)).HasDiverged().ShouldBeFalse();
            new DiversionBase<Tuple<int, int, int[]>>(Tuple.Create(1, 0, new [] {0}), Tuple.Create(1, 0, new [] {0})).HasDiverged().ShouldBeFalse();
        }

    }
}
