using System.Collections.Generic;
using System.Linq;

using Dijkstra;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Dijkstra.Test
{
    [TestClass]
    public class When_CalculatingShortestPathBetween1And5 : Given_DijkstraAndInputData
    {
        private List<Node> path;

        protected override void When()
        {
            this.path = this.algorithm.CalculateDistance(this.dictNodes[1], this.dictNodes[5]).ToList();
        }

        [TestMethod]
        public void Then_RouteMustBe1To3To6To5()
        {
            Assert.AreEqual(this.path[0], new Node(1, 0.0));
            Assert.AreEqual(this.path[1], new Node(3, 0.0));
            Assert.AreEqual(this.path[2], new Node(6, 0.0));
            Assert.AreEqual(this.path[3], new Node(5, 0.0));
        }
    }
}