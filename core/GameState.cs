using Sokoban.core.Level.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.core
{
    public delegate void BlockedResponse(bool response);

    class GameState
    {
        event BlockedResponse response;
        LevelData levelData;
        private int topX, topY, bottomX, bottomY, leftX, leftY, rightX, rightY;
        private bool topPass, bottomPass, rightPass, leftPass;
        private List<Boolean> crateStatuses = new List<bool>();
        private int blockedCount = 0;

        public GameState(BlockedResponse response)
        {
            this.response += response;
        }

        public void computeBroke(LevelData levelData)
        {
            this.levelData = levelData;
            checkCrates();
            foreach(bool blocked in crateStatuses)
            {
                if(blocked)
                {
                    blockedCount++;
                }
            }
            Console.WriteLine("remain :" + levelData.remaingTargets + " , blocked" + blockedCount);
            if(blockedCount == levelData.remaingTargets)
            {
                response.Invoke(true);
            } else
            {
                response.Invoke(false);
            }
            reset();
        }
        private void checkCrates()
        {
            foreach(Tile tile in levelData.Tilemap)
            {
                if(tile is Crate)
                {
                    topY = tile.X; topX = tile.Y - 1;
                    bottomY = tile.X; bottomX = tile.Y + 1;

                    leftY = tile.X -1; leftX = tile.Y;
                    rightY = tile.X + 1; rightX = tile.Y;

                    

                    topPass = checkBroked(topX, topY);
                    bottomPass = checkBroked(bottomX, bottomY);
                    leftPass = checkBroked(leftX, leftY);
                    rightPass = checkBroked(rightX, rightY);

                    Console.WriteLine($"Crate ({tile.X}, {tile.Y}) " +
                        $"\n\t top ({topX}, {topY}) {topPass} \t bottom ({bottomX}, {bottomY}) {bottomPass}" +
                        $"\n\t left ({leftX}, {leftY}) {leftPass} \t right ({rightX}, {rightY}) {rightPass}");

                    if ((topPass && leftPass && rightPass && bottomPass) ||
                        (topPass && bottomPass && rightPass) || (topPass && bottomPass && leftPass) ||
                        (topPass && leftPass) || (topPass && rightPass) || 
                        (bottomPass && leftPass) || (bottomPass && rightPass))
                    {
                        crateStatuses.Add(true);
                    } else
                    {
                        crateStatuses.Add(false);
                    }
                }
            }
        }
        private bool checkBroked(int x, int y)
        {
            if(levelData.Tiles[x][y] is Wall)
            {
                return true;
            }
            return false;
        }
        private void reset()
        {
            crateStatuses.Clear();
            blockedCount = 0;
        }
    }
}
