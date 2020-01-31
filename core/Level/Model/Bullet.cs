namespace Sokoban.core.Level.Model
{
    class Bullet: Tile
    {
        public Bullet(int x, int y) : base(x, y)
        {
            image = "trash";
        }

        public Bullet(): base(0, 0)
        {
            image = "trash";
        }
    }
}
