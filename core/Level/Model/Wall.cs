using Sokoban.core.Level.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core.Level.Model
{
    class Wall : Tile
    {
        public Wall(int x, int y): base(x, y)
        {
            this.image = "wall";
        }
        public Wall(): base(0, 0)
        {
            this.image = "wall";
        }
    }
}
