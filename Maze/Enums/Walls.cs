using System;

namespace Core.Game.Enums
{
    [Flags]
    public enum Walls
    {
        North = 1,
        South = 2,
        East = 4,
        West = 8
    }
}
