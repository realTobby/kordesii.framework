using System;
using System.Collections.Generic;

namespace kordesii.framework.Algorithms.AStar
{
    public class AStarPathfinder
    {
        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static int Distance(Point a, Point b)
            {
                return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
            }
        }

        private class Node
        {
            public Point Position;
            public float GCost;
            public float HCost;
            public float FCost => GCost + HCost;
            public Node Parent;

            public Node(Point position)
            {
                Position = position;
            }
        }

        public static List<Point> FindPath(Point start, Point end)
        {
            var openNodes = new List<Node>();
            var closedNodes = new HashSet<Point>();
            var startNode = new Node(start);
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                var currentNode = openNodes[0];

                for (var i = 1; i < openNodes.Count; i++)
                {
                    if (openNodes[i].FCost < currentNode.FCost ||
                        openNodes[i].FCost == currentNode.FCost && openNodes[i].HCost < currentNode.HCost)
                    {
                        currentNode = openNodes[i];
                    }
                }

                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode.Position);

                if (currentNode.Position.Equals(end))
                {
                    return RetracePath(startNode, currentNode);
                }

                var neighbors = GetNeighbors(currentNode.Position);

                foreach (var neighborPosition in neighbors)
                {
                    if (closedNodes.Contains(neighborPosition))
                    {
                        continue;
                    }

                    var neighbor = new Node(neighborPosition);
                    float tentativeGCost = currentNode.GCost + Point.Distance(currentNode.Position, neighbor.Position);

                    if (tentativeGCost < neighbor.GCost || !openNodes.Contains(neighbor))
                    {
                        neighbor.GCost = tentativeGCost;
                        neighbor.HCost = Point.Distance(neighbor.Position, end);
                        neighbor.Parent = currentNode;

                        if (!openNodes.Contains(neighbor))
                        {
                            openNodes.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        private static List<Point> GetNeighbors(Point position)
        {
            var neighbors = new List<Point>();

            // Only add horizontal and vertical neighbors
            neighbors.Add(new Point(position.X - 1, position.Y));
            neighbors.Add(new Point(position.X + 1, position.Y));
            neighbors.Add(new Point(position.X, position.Y - 1));
            neighbors.Add(new Point(position.X, position.Y + 1));

            return neighbors;
        }

        private static List<Point> RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Point>();
            var currentNode = endNode;

            while (!currentNode.Position.Equals(startNode.Position))
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}
