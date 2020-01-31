using Sokoban.core.Level.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sokoban.core.Level.Model
{
    class Player : Tile
    {

        public Player(int x, int y) : base(x, y)
        {
            this.image = "forklift";
        }

        public Player() : base(0, 0)
        {
            this.image = "forklift";
        }
    }
}
