using System.Diagnostics;

namespace TecX.Dijkstra
{
    [DebuggerDisplay("Node Id={Id}, Cost={Cost}")]
    public struct Node
    {
        private readonly long id;

        private readonly double cost;

        public static Node Empty
        {
            get { return new Node(long.MinValue, double.MaxValue); }
        }

        public long Id
        {
            get
            {
                return id;
            }
        }

        public double Cost
        {
            get
            {
                return this.cost;
            }
        }

        public Node(long id, double cost)
        {
            this.id = id;
            this.cost = cost;
        }
    }
}