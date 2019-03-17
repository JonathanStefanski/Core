using System.Collections.Generic;

namespace Core.Game.Models
{
    public class World
    {
        public World(int seed, List<Tile> tiles)
        {
            Seed = seed;
            Tiles = tiles;
        }

        public int Seed { get; set; }

        public List<Tile> Tiles { get; set; }
    }
}
