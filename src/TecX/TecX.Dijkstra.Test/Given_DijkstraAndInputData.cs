using System.Collections.Generic;
using System.Linq;

using TecX.Dijkstra;
using TecX.TestTools;

namespace Dijkstra
{
    public abstract class Given_DijkstraAndInputData : GivenWhenThen
    {
        protected TecX.Dijkstra.Dijkstra algorithm;

        protected Dictionary<long, Node> dictNodes;

        protected List<Node> nodes;

        protected List<Edge> edges;

        protected override void Given()
        {
            this.dictNodes = new Dictionary<long, Node>
                {
                    { 1, new Node(1, 0.0) },
                    { 2, new Node(2, 0.0) },
                    { 3, new Node(3, 0.0) },
                    { 4, new Node(4, 0.0) },
                    { 5, new Node(5, 0.0) },
                    { 6, new Node(6, 0.0) }
                };

            this.nodes = this.dictNodes.Values.ToList();

            this.edges = new List<Edge>
                {
                    new Edge(1, this.dictNodes[1], this.dictNodes[2], 7.0),
                    new Edge(2, this.dictNodes[1], this.dictNodes[3], 9.0),
                    new Edge(3, this.dictNodes[1], this.dictNodes[6], 14.0),

                    new Edge(4, this.dictNodes[2], this.dictNodes[1], 7.0),
                    new Edge(5, this.dictNodes[2], this.dictNodes[3], 10.0),
                    new Edge(6, this.dictNodes[2], this.dictNodes[4], 15.0),

                    new Edge(7, this.dictNodes[3], this.dictNodes[1], 9.0),
                    new Edge(8, this.dictNodes[3], this.dictNodes[2], 10.0),
                    new Edge(8, this.dictNodes[3], this.dictNodes[4], 11.0),
                    new Edge(9, this.dictNodes[3], this.dictNodes[6], 2.0),

                    new Edge(10, this.dictNodes[4], this.dictNodes[2], 15.0),
                    new Edge(11, this.dictNodes[4], this.dictNodes[3], 11.0),
                    new Edge(13, this.dictNodes[4], this.dictNodes[5], 6.0),

                    new Edge(15, this.dictNodes[5], this.dictNodes[4], 6.0),
                    new Edge(16, this.dictNodes[5], this.dictNodes[6], 9.0),

                    new Edge(17, this.dictNodes[6], this.dictNodes[1], 14.0),
                    new Edge(17, this.dictNodes[6], this.dictNodes[3], 2.0),
                    new Edge(18, this.dictNodes[6], this.dictNodes[5], 9.0)
                };

            algorithm = new TecX.Dijkstra.Dijkstra(this.edges, this.nodes);
        }
    }
}