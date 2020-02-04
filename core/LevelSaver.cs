using Newtonsoft.Json.Linq;
using Sokoban.core.Level.Model;
using Sokoban.core.Level.power;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core
{
    public class LevelSaver
    {
        LevelData levelData;
        String name, file;
        List<List<Tile>> tiles;
        List<PowerUpHolder> powerUps = new List<PowerUpHolder>();
        int player = 0, destination = 0, crates = 0;
        string[] map;
        bool bullet, phase;
        String moves;
        int movesInt = 10;

        public LevelSaver(List<List<Tile>> tiles, String name, String moves, bool bullet, bool phase)
        {
            Console.WriteLine(name);
            this.name = name;
            this.tiles = tiles;
            this.phase = phase;
            this.bullet = bullet;
            this.moves = moves;
            levelData = new LevelData();
            map = new String[tiles.Count];
            parseString();
            save();
        }

        private void save()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < map.Length; i++)
            {
                String temp = map[i];
                if (i == map.Length - 1)
                    stringBuilder.Append(temp);
                else
                    stringBuilder.AppendLine(temp);
            }
            levelData.name = name;
            levelData.data = stringBuilder.ToString();
            if (bullet)
                powerUps.Add(new PowerUpHolder(1, PowerUp.bullet));
            if (phase)
                powerUps.Add(new PowerUpHolder(1, PowerUp.phase));
            Int32.TryParse(moves, out movesInt);
            levelData.Moves = movesInt;
            levelData.PowerUps = powerUps;
            JObject jObject = JObject.FromObject(levelData);
            file = $"levels/{name}.level";
            StreamWriter streamWriter = new StreamWriter(file);
            streamWriter.Write(jObject.ToString());
            streamWriter.Close();
        }

        private void parseString()
        {
            int line = 0;
            foreach (List<Tile> row in tiles)
            {
                String temp = "";
                foreach (Tile tile in row)
                {
                    switch (tile.ToString())
                    {
                        case "Sokoban.core.Level.Model.Player":
                            player++;
                            temp += "o";
                            break;
                        case "Sokoban.core.Level.Model.Wall":
                            temp += "#";
                            break;
                        case "Sokoban.core.Level.Model.Floor":
                            temp += " ";
                            break;
                        case "Sokoban.core.Level.Model.Dest":
                            destination++;
                            temp += "@";
                            break;
                        case "Sokoban.core.Level.Model.Crate":
                            crates++;
                            temp += "x";
                            break;
                        default:
                            temp += " ";
                            break;
                    }
                }
                map[line] = temp;
                line++;
            }
        }
    }
}