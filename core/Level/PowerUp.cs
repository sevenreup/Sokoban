namespace Sokoban.core.Level.power
{
    public enum PowerUp
    {
        bullet,
        phase
    }

    public class PowerUpHolder
    {
        public PowerUpHolder ()
        {
            Count = 0;
        }
        public PowerUpHolder(int count, PowerUp powerUp)
        {
            this.Count = count;
            this.powerUp = powerUp;
        }
        public PowerUp powerUp
        {
            get;
            set;
        }
        public int Count
        {
            get;
            set;
        }
    }
}
