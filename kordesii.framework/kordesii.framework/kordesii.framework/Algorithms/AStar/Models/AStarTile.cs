using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace kordesii.framework.Algorithms.AStar.Models
{
    public class AStarTile
    {
        public int gCost { get; set; }
        public int hCost { get; set; }
        public int fCost { get { return gCost + hCost; } }

        public Vector3 tilePosition { get; set; } = new Vector3();
        public bool isWalkable { get; set; } = false;
        public AStarTile parentTile = new AStarTile(Vector3.Zero);

        public AStarTile(Vector3 pos)
        {
            tilePosition = pos;

        }

        public override string ToString()
        {
            return "X: " + tilePosition.X + " Y: " + tilePosition.Y + " Z: " + tilePosition.Z;
        }
    }
}
