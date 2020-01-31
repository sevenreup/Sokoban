using Sokoban.core.Level.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core.Level.Model
{
    class ModelLevel
    {
        public ModelLevel()
        {
            this.Levels = new List<string>();
            this.tempLevData = new List<List<string>>();
        }
        public String CurrentLevel 
        {
            get;
            set;
        }
        private int gDHeight = 30;
        public int GDHeight
        {
            get { return gDHeight; }
            set { gDHeight = value; }
        }
        public List<String> Levels
        {
            get;
            set;
        }
        public LevelData levelData
        {
            get;
            set;
        }
        public List<List<String>> tempLevData
        {
            get;
            set;
        }
        public bool inGame
        {
            get;
            set;
        }
    }
}
