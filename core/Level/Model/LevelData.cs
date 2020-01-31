using Sokoban.core.Level.power;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core.Level.Model
{
    class LevelData
    {
        public LevelData()
        {
            columns = 0;
            rows = 0;
        }

        private int gridSize = 30;
        public int GridSize
        {
            get { return gridSize; }
        }

        private Tile[,] tilemap;

        public void initMap(int x, int y) {
            Tilemap = new Tile[x, y];
        }
        // floor and walls
        private List<List<Tile>> tiles = new List<List<Tile>>();
        internal List<List<Tile>> Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }
        public Player player
        {
            get;
            set;
        }
        public String name
        {
            get;
            set;
        }
        public int rows
        {
            get;
            set;
        }
        public int columns
        {
            get;
            set;
        }
        public String data
        {
            get;
            set;
        }
        private List<PowerUpHolder> powerUps = new List<PowerUpHolder>();
        public List<PowerUpHolder> PowerUps
        {
            get { return powerUps; }
            set { powerUps = value; }
        }
        private int moves = 0;
        public int Moves
        {
            get { return moves; }
            set { moves = value; }
        }
        private int targets = 0;
        public int totalTargets
        {
            get { return targets; }
            set { targets = value; remaingTargets = value; }
        }
        public int remaingTargets
        {
            get;
            set;
        }
        private int crate = 0;
        public int numberOfCrates
        {
            get { return crate; }
            set { crate = value; }
        }
        internal Tile[,] Tilemap { get => tilemap; set => tilemap = value; }
    }
}
