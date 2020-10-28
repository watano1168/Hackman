using System.Linq;
using UnityEngine;

namespace Hackman.Game.Entity.Monster.Brain
{
    public readonly struct MapGraph
    {
        public readonly int NodeCount;
        public readonly int EdgeCount;
        public readonly MapGraphNode[] Nodes;
        public readonly MapGraphEdge[] Edges;
        public readonly MapGraphElement[,] Tilemap;
        public readonly int MapHeight;
        public readonly int MapWidth;

        public MapGraph(MapGraphNode[] nodes, MapGraphEdge[] edges)
        {
            Nodes = nodes;
            Edges = edges;
            NodeCount = nodes.Length;
            EdgeCount = edges.Length;

            var positions = nodes.Select(n => n.Position).Concat(edges.SelectMany(e => e.RoutePositions));
            var positionsArray = positions as Vector2Int[] ?? positions.ToArray();
            MapWidth = positionsArray.Select(p => p.x).Max() + 1;
            MapHeight = positionsArray.Select(p => p.y).Max() + 1;

            Tilemap = new MapGraphElement[MapWidth, MapHeight];
            foreach (var node in nodes)
                Tilemap[node.Position.x, node.Position.y] = new MapGraphElement
                    {Type = MapGraphElementType.Node, Id = node.Id};
            foreach (var edge in edges)
            foreach (var pos in edge.RoutePositions)
                Tilemap[pos.x, pos.y] = new MapGraphElement {Type = MapGraphElementType.Edge, Id = edge.Id};
        }
    }

    public enum MapGraphElementType
    {
        None,
        Node,
        Edge
    }

    public struct MapGraphElement
    {
        public MapGraphElementType Type;
        public int Id;
    }

    public readonly struct MapGraphNode
    {
        public readonly int Id;
        public readonly Vector2Int Position;
        public readonly int[] ConnectedEdgesId;

        public MapGraphNode(int id, Vector2Int position, int[] edgesId)
        {
            Id = id;
            Position = position;
            ConnectedEdgesId = edgesId;
        }
    }

    public readonly struct MapGraphEdge
    {
        public readonly int Id;
        public readonly Vector2Int[] RoutePositions;
        public readonly int StartNodeId;
        public readonly int EndNodeId;

        public MapGraphEdge(int id, Vector2Int[] route, int startNodeId, int endNodeId)
        {
            Id = id;
            RoutePositions = route;
            StartNodeId = startNodeId;
            EndNodeId = endNodeId;
        }
    }
}