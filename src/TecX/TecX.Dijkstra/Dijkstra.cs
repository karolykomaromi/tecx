using System;
using System.Collections.Generic;
using System.Linq;

using TecX.Common;

namespace TecX.Dijkstra
{
    public delegate double EdgeCostFunction(Edge edge);

    public class Dijkstra
    {
        private readonly HashSet<Node> nodes;
        private readonly HashSet<Edge> edges;
        private readonly HashSet<Node> searchSpace;
        private readonly Dictionary<long, double> distances;
        private readonly Dictionary<long, Node> previous;

        private readonly Dictionary<long, IEnumerable<Node>> neighbors;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="edges">Liste aller Kanten</param>
        /// <param name="nodes">Liste aller Knoten</param>
        public Dijkstra(IEnumerable<Edge> edges, IEnumerable<Node> nodes)
        {
            Guard.AssertNotNull(edges, "edges");
            Guard.AssertNotNull(nodes, "nodes");

            this.edges = new HashSet<Edge>(edges, new EdgeEqualityComparer());
            this.nodes = new HashSet<Node>(nodes, new NodeEqualityComparer());

            searchSpace = new HashSet<Node>(new NodeEqualityComparer());
            distances = new Dictionary<long, double>();
            previous = new Dictionary<long, Node>();

            neighbors = this.edges
                                .GroupBy(e => e.StartNode.Id)
                                .ToDictionary(g => g.Key, g => g.Select(e => e.EndNode));

            InitializeAlgorithm();
        }

        private void InitializeAlgorithm()
        {
            searchSpace.Clear();
            distances.Clear();
            previous.Clear();

            foreach (Node node in this.nodes)
            {
                searchSpace.Add(node);
                distances.Add(node.Id, double.MaxValue);
                previous.Add(node.Id, Node.Empty);
            }
        }

        /// <summary>
        /// Berechnet die k�rzesten Wege vom startNode
        /// Knoten zu allen anderen Knoten
        /// </summary>
        /// <param name="startNode">Startknoten</param>
        /// <param name="endNode"></param>
        public IEnumerable<Node> GetShortestPath(Node startNode, Node endNode)
        {
            distances[startNode.Id] = 0;

            while (searchSpace.Count > 0)
            {
                Node u = GetNearestNode();

                if (u.Equals(Node.Empty))
                {
                    searchSpace.Clear();
                }
                else
                {
                    foreach (Node neighbor in GetNeighbors(u))
                    {
                        double alt = distances[u.Id] + GetDistanceBetween(u, neighbor);

                        if (alt < distances[neighbor.Id])
                        {
                            distances[neighbor.Id] = alt;
                            previous[neighbor.Id] = u;
                        }
                    }

                    searchSpace.Remove(u);

                    //premature end of algorith as we already reached the target
                    if(u.Equals(endNode))
                    {
                        return GetPathTo(endNode);
                    }
                }
            }

            return GetPathTo(endNode);
        }

        /// <summary>
        /// Liefert den Pfad zum Knoten destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        private IEnumerable<Node> GetPathTo(Node destination)
        {
            List<Node> path = new List<Node>();

            path.Insert(0, destination);

            while (!previous[destination.Id].Equals(Node.Empty))
            {
                destination = previous[destination.Id];

                path.Insert(0, destination);
            }

            return path;
        }

        /// <summary>
        /// Liefert den Knoten mit der k�rzesten Distanz
        /// </summary>
        /// <returns></returns>
        private Node GetNearestNode()
        {
            //TODO priority queue?
            double distance = double.MaxValue;

            Node smallest = Node.Empty;

            foreach (Node node in searchSpace)
            {
                if (distances[node.Id] < distance)
                {
                    distance = distances[node.Id];

                    smallest = node;
                }
            }

            return smallest;
        }

        /// <summary>
        /// Liefert alle Nachbarn von node die noch in der Basis sind
        /// </summary>
        /// <param name="node">Knoten</param>
        /// <returns></returns>
        private IEnumerable<Node> GetNeighbors(Node node)
        {
            return neighbors[node.Id].Where(n => searchSpace.Contains(n));
        }

        /// <summary>
        /// Liefert die Distanz zwischen zwei Knoten
        /// </summary>
        /// <param name="startNode">Startknoten</param>
        /// <param name="endNode">Endknoten</param>
        /// <returns></returns>
        private double GetDistanceBetween(Node startNode, Node endNode)
        {
            foreach (Edge edge in edges)
            {
                if (edge.StartNode.Equals(startNode) &&
                    edge.EndNode.Equals(endNode))
                {
                    return edge.Cost;
                }
            }

            return 0.0;
        }
    }
}