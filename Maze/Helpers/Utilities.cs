using Core.Game.Enums;
using Core.Game.Models;

namespace Core.Game.Helpers
{
    public static class Utilities
    {
        public static Walls GetAllWalls(this Walls value)
        {
            return Walls.East | Walls.West | Walls.North | Walls.South;
        }

        public static int GetTileTypeUpper()
        {
            var enumValues = typeof(TileType).GetEnumValues();

            var i = 0;
            foreach (var enumValue in enumValues) { i += (int) enumValue; }
            return i;
        }

        public static Walls RemoveWall(this Walls value, Walls wallToRemove)
        {
            return value & ~wallToRemove;
        }
    }
}
