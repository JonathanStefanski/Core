using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Game.Enums
{
    [Flags]
    public enum TileType
    {
        Grassland = 1 << 0,     // 1
        Shrubs = 1 << 1,        // 2
        LightForest = 1 << 2,   // 4
        DenseForest = 1 << 3    // 8
    }
}
