using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Sokoban.core.Level.Model
{
    class Tile: Image
    {
        public Tile(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }
        private String Image;
        public String image
        {
            get { return Image; }
            set { 
                this.Source = new BitmapImage(new Uri(@"C:\Users\Senpai\source\repos\Sokoban\image\" + value + ".png", UriKind.Absolute));
                Image = value;
            }
        }

    }
}
