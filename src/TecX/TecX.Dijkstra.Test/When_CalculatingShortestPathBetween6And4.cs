using System.Collections.Generic;
using System.Linq;

using Dijkstra;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Dijkstra.Test
{
    [TestClass]
    public class When_CalculatingShortestPathBetween6And4 : Given_DijkstraAndInputData
    {
        private List<Node> path;

        protected override void When()
        {
            this.path = this.algorithm.GetShortestPath(this.dictNodes[6], this.dictNodes[4]).ToList();
        }

        [TestMethod]
        public void Then_RouteMustBe6To3To4()
        {
            Assert.AreEqual(this.path[0], new Node(6, 0.0));
            Assert.AreEqual(this.path[1], new Node(3, 0.0));
            Assert.AreEqual(this.path[2], new Node(4, 0.0));
        }
    }
}