﻿namespace Sokoban.core.Level.power
{
    enum PowerUp
    {
        bullet,
        teleport
    }

    class PowerUpHolder
    {
        public PowerUpHolder ()
        {
            Count = 0;
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
