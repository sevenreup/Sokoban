using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core.Level.Model
{
    class Deliver : Tile
    {
        public Deliver(int x, int y): base(x, y)
        {
            image = "box_hit";
        }

        public Deliver() : base(0, 0)
        {
            image = "box_hit";
        }
    }
}
