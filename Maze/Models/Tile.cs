using Core.Game.Enums;
using Core.Game.Helpers;

#pragma warning disable 659

namespace Core.Game.Models
{
    public class Tile
    {
        public Tile(int x, int y, TileType type)
        {
            X = x;
            Y = y;
            TileType = type;
            Walls = Walls.GetAllWalls();
        }

        public int X { get; set; }
        public int Y { get; set; }

        public TileType TileType { get; set; }

        public Walls Walls { get; set; }

        public bool Visited { get; set; }
        public bool Portal { get; set; }
        public bool EntryPoint { get; set; }
        public bool ExitPoint { get; set; }

        public bool IsSpecialPoint => Portal || EntryPoint || ExitPoint;

        public override bool Equals(object obj)
        {
            if (!(obj is Tile item)) return false;
            return X == item.X && Y == item.Y;
        }
    }
}
