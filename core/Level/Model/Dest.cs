using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core.Level.Model
{
    class Dest: Tile
    {
        public Dest(int x, int y) : base(x, y)
        {
            image = "target";
        }
        public Dest(): base(0, 0)
        {
            image = "target";
        }
    }
}
