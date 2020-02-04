using Sokoban.core.Level.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sokoban.core.Level.Model
{
    public class Player : Tile
    {

        public Player(int x, int y) : base(x, y)
        {
            this.image = Left;
        }

        public Player() : base(0, 0)
        {
            this.image = Left;
        }

        public String Left
        {
            get { return "player"; }
        }
        public String Right
        {
            get { return @"player\right"; }
        }
        public String Up
        {
            get { return @"player\up"; }
        }
        public String Down
        {
            get { return @"player\down"; }
        }
    }
}
