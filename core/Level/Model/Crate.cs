using Sokoban.core.Level.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core.Level.Model
{
    class Crate : Tile
    {
        public Crate(int x, int y): base(x, y)
        {
            this.image = "crate";
        }

        public Crate(): base(0, 0)
        {
            this.image = "crate";
        }
    }
}
