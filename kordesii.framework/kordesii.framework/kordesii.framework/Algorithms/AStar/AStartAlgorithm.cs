using kordesii.framework.Algorithms.AStar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace kordesii.framework.Algorithms.AStar
{
    internal class AStartAlgorithm
    {
    }

    public class Pathfinder
    {
        private List<AStarTile> currentGameBoard = new List<AStarTile>();

        private const int MOVE_STRAIGHT_COST = 10;

        private List<AStarTile> openList = new List<AStarTile>();
        private List<AStarTile> closedList = new List<AStarTile>();


        public List<AStarTile> FindPath(List<AStarTile> gameBoard, Vector3 startPos, Vector3 endPos)
        {
            currentGameBoard = gameBoard;

            AStarTile startNode = GetTileAt(startPos);
            AStarTile endNode = GetTileAt(endPos);
            openList.Add(startNode);

            for (int i = 0; i < currentGameBoard.Count; i++)
            {
                AStarTile tile = currentGameBoard[i];
                tile.gCost = int.MaxValue;
                tile.parentTile = null;
            }

            if (startNode != null)
            {
                startNode.gCost = 0;
                startNode.hCost = CalculateDistanceCost(startNode, endNode);

                while (openList.Count > 0)
                {
                    AStarTile currentNode = GetLowestFCostTile(openList);

                    if (currentNode == endNode)
                    {
                        return CalculatePath(endNode);
                    }

                    openList.Remove(currentNode);
                    closedList.Add(currentNode);

                    foreach (AStarTile neighbourNode in GetNeighbours(currentNode))
                    {
                        if (closedList.Contains(neighbourNode)) continue;

                        int tGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                        if (tGCost < neighbourNode.gCost)
                        {
                            neighbourNode.parentTile = currentNode;
                            neighbourNode.gCost = tGCost;
                            neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);

                            if (!openList.Contains(neighbourNode))
                            {
                                openList.Add(neighbourNode);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private AStarTile GetTileAt(Vector3 pos)
        {
            return currentGameBoard.Where(tile => tile.tilePosition.X == pos.X && tile.tilePosition.Z == pos.Z && tile.isWalkable == true).FirstOrDefault();
        }

        private List<AStarTile> GetNeighbours(AStarTile node)
        {
            // This is good for when the data allows it to sort it via a parameter like "isWalkable"
            // but what if we wanted to use this algorithm with no prior data?
            // we need to change the whole process of this algorithm to work with an empty game board
            // and only the start and end positions
            // this way we have a pathfinding algorithm in empty space
            // this is really cool and usefull for many projects :)
            // maybe this dll was a good idea after all




            List<AStarTile> neighbours = new List<AStarTile>();

            Vector3 leftPos = new Vector3(node.tilePosition.X - 1, node.tilePosition.Y, node.tilePosition.Z);
            AStarTile leftTile = GetTileAt(leftPos);
            if (leftTile != null)
            {
                if (leftTile.isWalkable == true)
                    neighbours.Add(leftTile);
            }

            Vector3 rightPos = new Vector3(node.tilePosition.X + 1, node.tilePosition.Y, node.tilePosition.Z);
            AStarTile rightTile = GetTileAt(rightPos);
            if (rightTile != null)
            {
                if (rightTile.isWalkable == true)
                    neighbours.Add(rightTile);
            }

            Vector3 upPos = new Vector3(node.tilePosition.X, node.tilePosition.Y, node.tilePosition.Z + 1);
            AStarTile upTile = GetTileAt(upPos);
            if (upTile != null)
            {
                if (upTile.isWalkable == true)
                    neighbours.Add(upTile);
            }


            Vector3 downPos = new Vector3(node.tilePosition.X, node.tilePosition.Y, node.tilePosition.Z - 1);
            AStarTile downTile = GetTileAt(downPos);
            if (downTile != null)
            {
                if (downTile.isWalkable == true)
                    neighbours.Add(downTile);
            }


            return neighbours;
        }


        private List<AStarTile> CalculatePath(AStarTile endNode)
        {
            List<AStarTile> path = new List<AStarTile>();
            path.Add(endNode);
            AStarTile currentNode = endNode;
            while (currentNode.parentTile != null)
            {
                path.Add(currentNode.parentTile);
                currentNode = currentNode.parentTile;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(AStarTile a, AStarTile b)
        {
            int xDistance = Convert.ToInt32(MathF.Abs(Convert.ToInt32(a.tilePosition.X) - Convert.ToInt32(b.tilePosition.X)));
            int zDistance = Convert.ToInt32(MathF.Abs(Convert.ToInt32(a.tilePosition.Z) - Convert.ToInt32(b.tilePosition.Z)));
            int remaining = Convert.ToInt32(MathF.Abs(xDistance - zDistance));
            return Convert.ToInt32(MathF.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining);
        }

        private AStarTile GetLowestFCostTile(List<AStarTile> currentTiles)
        {
            var lowest = currentTiles
            .OrderBy(item => item.fCost)
            .FirstOrDefault();

            return lowest;
        }

    }
}
