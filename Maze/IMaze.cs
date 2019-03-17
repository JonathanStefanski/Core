using Core.Game.Models;

namespace Core.Game
{
    public interface IMaze
    {
        World Generate(int columns, int rows);

        void CreateMaze(World world);
    }
}
