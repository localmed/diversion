using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should.Fluent;

namespace Diversion.Test
{
    [TestClass]
    public class DiversionFacts
    {
        [TestMethod]
        public void HasDivergedReturnsTrueIfAnyPropertyDiffersBetweenTwoObjects()
        {
            new DiversionImpl<Tuple<int, int, int>>(Tuple.Create(1, 0, 0), Tuple.Create(2, 0, 0)).HasDiverged().Should().Be.True();
            new DiversionImpl<Tuple<int, int, int[]>>(Tuple.Create(1, 0, new []{0}), Tuple.Create(1, 0, new [] {1})).HasDiverged().Should().Be.True();
        }

        [TestMethod]
        public void HasDivergedReturnsFalseIfNoPropertiesDifferBetweenTwoObjects()
        {
            new DiversionImpl<Tuple<int, int, int>>(Tuple.Create(1, 0, 0), Tuple.Create(1, 0, 0)).HasDiverged().Should().Be.False();
            new DiversionImpl<Tuple<int, int, int[]>>(Tuple.Create(1, 0, new [] {0}), Tuple.Create(1, 0, new [] {0})).HasDiverged().Should().Be.False();
        }
    }
}
