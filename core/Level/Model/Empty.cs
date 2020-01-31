using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core.Level.Model
{
    class Empty: Tile
    {
        public Empty(int x, int y): base(x, y)
        {
            image = "floor";
        }
    }
}
