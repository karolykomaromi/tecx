using System.Collections.Generic;

namespace TecX.Dijkstra
{
    public class EdgeEqualityComparer : EqualityComparer<Edge>
    {
        public override bool Equals(Edge x, Edge y)
        {
            return x.StartNode.Id == y.StartNode.Id && x.EndNode.Id == y.EndNode.Id;
        }

        public override int GetHashCode(Edge obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}