using System;
using Xunit;
using Should.Fluent;

namespace Diversion.Test
{
    
    public class DiversionFacts
    {
        [Fact]
        public void OldShouldBeSetCorrectly()
        {
            var old = Tuple.Create(1, 0, 0);
            var @new = Tuple.Create(1, 0, 1);
            new DiversionBase<Tuple<int, int, int>>(old, @new).Old.Should().Equal(old);
        }

        [Fact]
        public void NewShouldBeSetCorrectly()
        {
            var old = Tuple.Create(1, 0, 0);
            var @new = Tuple.Create(1, 0, 1);
            new DiversionBase<Tuple<int, int, int>>(old, @new).New.Should().Equal(@new);
        }

        [Fact]
        public void HasDivergedReturnsTrueIfAnyPropertyDiffersBetweenTwoObjects()
        {
            new DiversionBase<Tuple<int, int, int>>(Tuple.Create(1, 0, 0), Tuple.Create(2, 0, 0)).HasDiverged().Should().Be.True();
            new DiversionBase<Tuple<int, int, int[]>>(Tuple.Create(1, 0, new []{0}), Tuple.Create(1, 0, new [] {1})).HasDiverged().Should().Be.True();
        }

        [Fact]
        public void HasDivergedReturnsFalseIfNoPropertiesDifferBetweenTwoObjects()
        {
            new DiversionBase<Tuple<int, int, int>>(Tuple.Create(1, 0, 0), Tuple.Create(1, 0, 0)).HasDiverged().Should().Be.False();
            new DiversionBase<Tuple<int, int, int[]>>(Tuple.Create(1, 0, new [] {0}), Tuple.Create(1, 0, new [] {0})).HasDiverged().Should().Be.False();
        }
    }
}
