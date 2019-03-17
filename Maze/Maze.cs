using System;
using System.Collections.Generic;
using System.Linq;
using Core.Game.Enums;
using Core.Game.Helpers;
using Core.Game.Models;

namespace Core.Game
{
    public class Maze : IMaze
    {
        public World Generate(int columns, int rows)
        {
            // Generate all variables
            var seed = Guid.NewGuid().GetHashCode();
            var rng = new Random(seed);
            var tiles = new List<Tile>();

            // Instantiate tiles
            var tileTypeUpperBound = Utilities.GetTileTypeUpper() + 1;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    tiles.Add(new Tile(j, i, (TileType) rng.Next(1, tileTypeUpperBound)));
                }
            }

            return new World(seed, tiles);
        }

        public void CreateMaze(World world)
        {
            var seed = world.Seed;
            var tiles = world.Tiles;
            var rng = new Random(seed);

            // Pick Start Tile
            var portalTile = PickStartingTile(tiles, seed);
            portalTile.Portal = true;

            // Pick Entry and Exit point
            var borderPoints = PickExitTiles(tiles, seed, portalTile).ToList();
            borderPoints.First().EntryPoint = true;
            borderPoints.Last().ExitPoint = true;

            // Carve MazeOpenings
            CarveExits(tiles, seed);

            // Carve the maze!
            var stack = new Stack<Tile>();

            // 1. Set PortalTile as current and add to stack
            var currentTile = portalTile;
            stack.Push(currentTile);
            
            // Loop through all fields on the board
            while (stack.Count > 0)
            {
                currentTile.Visited = true;
                var neighbors = FindClosest(currentTile, tiles);

                // If neighbors found, add new neighbor as current and throw on the stack
                if (neighbors.Length > 0)
                {
                    var nextTile = neighbors[rng.Next(0, neighbors.Length)];
                    CarvePassage(currentTile, nextTile);
                    currentTile = nextTile;
                    stack.Push(nextTile);
                }
                // If no viable neighbors found, return to last tile on the stack
                else
                {
                    currentTile = stack.Pop();
                }
            }
        }

        private static Tile PickStartingTile(IReadOnlyCollection<Tile> tileSet, int seed)
        {
            var xMinBound = tileSet.Min(t => t.X);
            var xMaxBound = tileSet.Max(t => t.X);
            var yMinBound = tileSet.Min(t => t.Y);
            var yMaxBound = tileSet.Max(t => t.Y);

            var rng = new Random(seed);

            var x = rng.Next(xMinBound, xMaxBound + 1);
            var y = rng.Next(yMinBound, yMaxBound + 1);

            return tileSet.FirstOrDefault(t => t.X == x && t.Y == y);
        }

        private static IEnumerable<Tile> PickExitTiles(List<Tile> tileSet, int seed, Tile portalTile)
        {
            var xMinBound = tileSet.Min(t => t.X);
            var xMaxBound = tileSet.Max(t => t.X);
            var yMinBound = tileSet.Min(t => t.Y);
            var yMaxBound = tileSet.Max(t => t.Y);

            var rng = new Random(seed);

            // Select all border tiles
            var cornerTiles = tileSet
                .Where(t => t.X == xMinBound || t.X == xMaxBound || 
                            t.Y == yMinBound || t.Y == yMaxBound)
                .Distinct().ToArray();

            Tile entryTile, exitTile;

            // Pick entry tile
            do
                entryTile = cornerTiles[rng.Next(0, cornerTiles.Length)];
            while (entryTile.Equals(portalTile));

            // Pick exit tile
            do
                exitTile = cornerTiles[rng.Next(0, cornerTiles.Length)];
            while (exitTile.Equals(portalTile) || exitTile.Equals(entryTile));
            
            return new List<Tile>{ entryTile, exitTile };
        }

        private Tile[] FindClosest(Tile current, List<Tile> tileSet)
        {
            var neighbors = new List<Tile>
            {
                tileSet.Where(t => !t.Visited).FirstOrDefault(t => current.X - 1 == t.X && current.Y == t.Y),
                tileSet.Where(t => !t.Visited).FirstOrDefault(t => current.X + 1 == t.X && current.Y == t.Y),
                tileSet.Where(t => !t.Visited).FirstOrDefault(t => current.X == t.X && current.Y - 1 == t.Y),
                tileSet.Where(t => !t.Visited).FirstOrDefault(t => current.X == t.X && current.Y + 1 == t.Y)
            };

            // Remove neighbors that fall from the playing field
            neighbors.RemoveAll(t => t == null);

            return neighbors.ToArray();
        }

        private void CarvePassage(Tile tileA, Tile tileB)
        {
            // Find direction of equality
            // Horizontal direction
            if (tileA.Y == tileB.Y)
            {
                if (tileA.X + 1 == tileB.X)
                {
                    // tile B lies east of tile A
                    tileA.Walls = tileA.Walls.RemoveWall(Walls.East);
                    tileB.Walls = tileB.Walls.RemoveWall(Walls.West);
                }
                else if (tileA.X - 1 == tileB.X)
                {
                    // tile B lies west of tile A
                    tileA.Walls = tileA.Walls.RemoveWall(Walls.West);
                    tileB.Walls = tileB.Walls.RemoveWall(Walls.East);
                }
            }
            // Vertical direction
            else if (tileA.X == tileB.X)
            {
                if (tileA.Y + 1 == tileB.Y)
                {
                    // tile B lies North of tile A
                    tileA.Walls = tileA.Walls.RemoveWall(Walls.North);
                    tileB.Walls = tileB.Walls.RemoveWall(Walls.South);
                }
                else if (tileA.Y - 1 == tileB.Y)
                {
                    // tile B lies South of tile A
                    tileA.Walls = tileA.Walls.RemoveWall(Walls.South);
                    tileB.Walls = tileB.Walls.RemoveWall(Walls.North);
                }
            }
        }

        private static void CarveExits(List<Tile> tileSet, int seed)
        {
            var xMinBound = tileSet.Min(t => t.X);
            var xMaxBound = tileSet.Max(t => t.X);
            var yMinBound = tileSet.Min(t => t.Y);
            var yMaxBound = tileSet.Max(t => t.Y);

            var rng = new Random(seed);

            var tileSubset = tileSet.Where(t => t.EntryPoint || t.ExitPoint).ToList();

            foreach (var tile in tileSubset)
            {
                var carvePoints = new Dictionary<Walls, bool>
                {
                    {Walls.North, tile.Y == yMaxBound},
                    {Walls.South, tile.Y == yMinBound},
                    {Walls.East, tile.X == xMinBound},
                    {Walls.West, tile.X == xMaxBound}
                }.Where(d => d.Value).ToArray();

                var carvePoint = carvePoints[rng.Next(0, carvePoints.Length)].Key;

                tile.Walls = tile.Walls.RemoveWall(carvePoint);
            }
        }
    }
}
