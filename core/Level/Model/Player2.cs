using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sokoban.core.Level.Model
{
    class Player2: Tile
    {
        private ImageBrush imageBrush;
        TranslateTransform SpriteSheetOffset;

        public Player2(int x, int y): base(x, y)
        {
            createRectangle();
        }
        private void createRectangle()
        {
            player = new Rectangle();
            player.Width = 100;
            player.Height = 100;
            imageBrush = new ImageBrush(new BitmapImage(new Uri(@"image\spritesheet\sokoban_tilesheet.png", UriKind.Relative)));
            SpriteSheetOffset = new TranslateTransform();
            SpriteSheetOffset.X = 0;
            SpriteSheetOffset.Y = 0;
            imageBrush.Transform = SpriteSheetOffset;
            player.Fill = imageBrush;
        }
        public Rectangle player {
            get;
            set;
        }
    }
}
