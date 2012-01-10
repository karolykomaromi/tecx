using System.Diagnostics;

namespace TecX.Dijkstra
{
    [DebuggerDisplay("Edge Id={Id}, Start={StartNode.Id}, End={EndNode.Id}, Cost={Cost}")]
    public struct Edge
    {
        private readonly long id;

        private readonly double cost;

        private readonly Node startNode;

        private readonly Node endNode;

        public Edge(long id, Node startNode, Node endNode, double cost)
            : this()
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.id = id;
            this.cost = cost;
        }

        public bool Selected { get; set; }

        public long Id
        {
            get
            {
                return this.id;
            }
        }

        public double Cost
        {
            get
            {
                return this.cost;
            }
        }

        public Node EndNode
        {
            get
            {
                return this.endNode;
            }
        }

        public Node StartNode
        {
            get
            {
                return this.startNode;
            }
        }
    }
}