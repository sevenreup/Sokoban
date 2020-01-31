using Newtonsoft.Json;
using Sokoban.core.Level.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sokoban.core.Level
{
    class LevelLoader
    {
        private ModelLevel modelLevel;

        public LevelLoader(ModelLevel modelLevel)
        {
            this.modelLevel = modelLevel;
        }
        private void cleanLevelData()
        {
            modelLevel.tempLevData = new List<List<string>>();
            modelLevel.levelData = new LevelData();
        }
        public void loadLevels() {

            String temp = Directory.GetCurrentDirectory().ToString();
            List<String> holder = new List<string>();
            foreach(String level in Directory.GetFiles(@"levels"))
            {
                if(level.EndsWith(".level"))
                {
                    holder.Add(level);
                    Console.WriteLine(holder.ToString());
                }
            }
            modelLevel.Levels.AddRange(holder);
            Console.WriteLine(modelLevel.Levels.ToString());
        }

        public void initLevel()
        {
            cleanLevelData();
            String levelFile = modelLevel.CurrentLevel;
            StreamReader reader = new StreamReader(levelFile);

            String temp = reader.ReadToEnd();
            reader.Close();

            LevelData json = JsonConvert.DeserializeObject<LevelData>(temp);
            modelLevel.levelData = json;
        }

        public void initiateTileSet() {
            String data = modelLevel.levelData.data;
            List<String> rows = new List<string>();
            rows.AddRange(data.Split('\n'));
            modelLevel.levelData.rows = rows.Count();

            foreach(String lines in rows) {
                List<String> tempContainer = new List<string>();
                if(modelLevel.levelData.columns == 0)
                {
                    modelLevel.levelData.columns = lines.Count() -1;
                }
                for(int i = 0; i < lines.Count(); i++)
                {
                    String ver = lines.Substring(i, 1);
                    tempContainer.Add(ver);
                }
                modelLevel.tempLevData.Add(tempContainer);
            }
            modelLevel.levelData.initMap(modelLevel.levelData.rows, modelLevel.levelData.columns);
            createTileSet();
        }

        private void createTileSet()
        {
            modelLevel.levelData.Tiles.Clear();

            for(int y = 0; y < modelLevel.levelData.rows; y++)
            {
                List<Tile> row = new List<Tile>();
                for(int x = 0; x < modelLevel.levelData.columns; x++)
                {
                    switch (modelLevel.tempLevData[y][x])
                    {
                        case "#":
                            Wall wall = new Wall(x, y);
                            row.Add(wall);
                            break;
                        case "o":
                            Floor floor2 = new Floor(x, y);
                            row.Add(floor2);

                            Player player = new Player(x, y);
                            modelLevel.levelData.Tilemap[y, x] = player;
                            Console.WriteLine("Player (" + y + ", " + x);
                            modelLevel.levelData.player = player;
                            break;
                        case "x":
                            Floor floor3 = new Floor(y, x);
                            row.Add(floor3);

                            Crate crate = new Crate(x, y);
                            modelLevel.levelData.numberOfCrates++;
                            modelLevel.levelData.Tilemap[y, x] = crate;
                            break;
                        case "@":
                            modelLevel.levelData.totalTargets++;
                            Dest destination = new Dest(x, y);
                            row.Add(destination);
                            break;
                        case " ":
                            Floor floor = new Floor(x, y);
                            row.Add(floor);
                            break;
                        case "+":
                            Empty empty = new Empty(x, y);
                            row.Add(empty);
                            break;
                    }
                }
                modelLevel.levelData.Tiles.Add(row);
            }
        }
    }
}
