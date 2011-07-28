using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

namespace Dijkstra
{
    [TestClass]
    public class DijkstraFixture
    {
        [TestMethod]
        public void NodeEqualsEvaluatesProperly()
        {
            Node n1 = new Node(long.MinValue, double.MaxValue);

            Assert.IsTrue(n1.Equals(Node.Empty));
        }

        [TestMethod]
        public void EdgeEqualityComparer()
        {
            Edge e1 = new Edge(1, new Node(1, 0.0), new Node(2, 0.0), 1.0);
            Edge e2 = new Edge(2, new Node(1, 0.0), new Node(2, 0.0), 1.0);

            EdgeEqualityComparer comparer = new EdgeEqualityComparer();

            Dictionary<Edge, double> dictionary = new Dictionary<Edge, double>(comparer);

            dictionary.Add(e1, e1.Cost);
            dictionary.Add(e2, e2.Cost);

            Assert.AreEqual(2, dictionary.Count);
        }
    }
}
