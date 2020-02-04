using Sokoban.core.Level.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sokoban.core.Level.Model
{
    public class Player : Tile
    {
        private ImageBrush imageBrush;
        TranslateTransform SpriteSheetOffset;

        public Player(int x, int y) : base(x, y)
        {
            this.image = Left;
            createRectangle();
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

        private void createRectangle()
        {
            player = new Rectangle();
            player.Width = 64;
            player.Height = 64;
            imageBrush = new ImageBrush(new BitmapImage(Util.Get("image/spritesheet/sokoban_tilesheet.png")));
            imageBrush.Stretch = Stretch.None;
            imageBrush.AlignmentX = AlignmentX.Left;
            imageBrush.AlignmentY = AlignmentY.Top;
            SpriteSheetOffset = new TranslateTransform();
            SpriteSheetOffset.X = 0;
            SpriteSheetOffset.Y = 0;
            imageBrush.Transform = SpriteSheetOffset;
            player.Fill = imageBrush;
        }
        public Rectangle player
        {
            get;
            set;
        }
    }
}
