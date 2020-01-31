using Sokoban.core.Level.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core.Level.Model
{
    class Floor : Tile
    {
        public Floor(int x, int y): base(x, y)
        {
            this.image = "floor";
        }
    }
}
