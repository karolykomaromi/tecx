using System.Collections.Generic;

namespace TecX.Dijkstra
{
    public class NodeEqualityComparer : EqualityComparer<Node>
    {
        public override bool Equals(Node x, Node y)
        {
            return x.Id == y.Id;
        }

        public override int GetHashCode(Node obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}