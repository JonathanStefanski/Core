using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.API.Helpers
{
    public class MazeParams
    {
        private int _rows = 10;
        private int _columns = 10;

        public int Rows
        {
            get => _rows;
            set => _rows = value <= 0 ? 1 : value > 100 ? 100 : value;
        }

        public int Columns
        {
            get => _columns;
            set => _columns = value <= 0 ? 1 : value > 100 ? 100 : value;
        }
    }
}
