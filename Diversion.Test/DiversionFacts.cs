using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test
{
    [TestClass]
    public class DiversionFacts
    {
        [TestMethod]
        public void OldShouldBeSetCorrectly()
        {
            var old = Tuple.Create(1, 0, 0);
            var @new = Tuple.Create(1, 0, 1);
            new DiversionBase<Tuple<int, int, int>>(old, @new).Old.Should().Equal(old);
        }

        [TestMethod]
        public void NewShouldBeSetCorrectly()
        {
            var old = Tuple.Create(1, 0, 0);
            var @new = Tuple.Create(1, 0, 1);
            new DiversionBase<Tuple<int, int, int>>(old, @new).New.Should().Equal(@new);
        }

        [TestMethod]
        public void HasDivergedReturnsTrueIfAnyPropertyDiffersBetweenTwoObjects()
        {
            new DiversionBase<Tuple<int, int, int>>(Tuple.Create(1, 0, 0), Tuple.Create(2, 0, 0)).HasDiverged().Should().Be.True();
            new DiversionBase<Tuple<int, int, int[]>>(Tuple.Create(1, 0, new []{0}), Tuple.Create(1, 0, new [] {1})).HasDiverged().Should().Be.True();
        }

        [TestMethod]
        public void HasDivergedReturnsFalseIfNoPropertiesDifferBetweenTwoObjects()
        {
            new DiversionBase<Tuple<int, int, int>>(Tuple.Create(1, 0, 0), Tuple.Create(1, 0, 0)).HasDiverged().Should().Be.False();
            new DiversionBase<Tuple<int, int, int[]>>(Tuple.Create(1, 0, new [] {0}), Tuple.Create(1, 0, new [] {0})).HasDiverged().Should().Be.False();
        }
    }
}
