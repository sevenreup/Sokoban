using Sokoban.core;
using Sokoban.core.Level;
using Sokoban.core.Level.Model;
using Sokoban.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sokoban
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    ///
    
    public partial class Game : Window
    {
        LevelLoader levelLoader;
        ModelLevel modelLevel;
        GameGrid gameGrid;

        public Game()
        {
            InitializeComponent();

            modelLevel = new ModelLevel();
            levelLoader = new LevelLoader(modelLevel);

            levelLoader.loadLevels();
            
            
        }

        public void Init(String levelStr)
        {
            modelLevel.CurrentLevel = levelStr;
            levelLoader.initLevel();
            levelLoader.initiateTileSet();
            modelLevel.inGame = true;

            gameDef.Height = new GridLength(modelLevel.GDHeight);

            this.Cont.Width = 16 + (modelLevel.levelData.columns * modelLevel.levelData.GridSize);
            this.Cont.Height = 39 + (modelLevel.levelData.rows * modelLevel.levelData.GridSize) + modelLevel.GDHeight;

            Console.WriteLine("Window \n width: " + Cont.Width + "\nHeight: " + Cont.Height);

            this.Cont.Title = "Sokoban : " + modelLevel.levelData.name;

            if(gameGrid != null)
            {
                gameGrid.Visibility = Visibility.Collapsed;
            }

            gameGrid = new GameGrid(modelLevel.levelData);
            gameGrid.SetValue(Grid.ColumnProperty, 0);
            gameGrid.SetValue(Grid.RowProperty, 1);
            gameGrid.HorizontalAlignment = HorizontalAlignment.Center;
            gameGrid.VerticalAlignment = VerticalAlignment.Center;

            gameCanvas.Children.Add(gameGrid);

            gameCanvas.Visibility = Visibility.Visible;
            Console.WriteLine("Grid \n width: " + gameCanvas.Width + "\nHeight: " + gameCanvas.Height);
            splash.Visibility = Visibility.Hidden;

            targetStatus();
        }

        private void StartButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Init(modelLevel.Levels[0]);
        }

        private int pX, pY;
        private int nextX, nextY, secondX, secondY;
        private void keypress(object sender, KeyEventArgs e)
        {
            pX = modelLevel.levelData.player.X;
            pY = modelLevel.levelData.player.Y;
            Console.WriteLine("player : " + modelLevel.levelData.player.X + ", " + modelLevel.levelData.player.Y);

            switch (e.Key.ToString())
            {
                case "Left":
                case "A":
                    nextX = pX - 1;
                    nextY = pY;

                    secondX = pX - 2;
                    secondY = pY;

                    move(180);
                    break;
                case "Right":
                case "D":
                    nextX = pX + 1;
                    nextY = pY;

                    secondX = pX + 2;
                    secondY = pY;

                    move(0);
                    break;
                case "Up":
                case "W":
                    nextX = pX;
                    nextY = pY - 1;

                    secondX = pX;
                    secondY = pY - 2;

                    move(270);
                    break;
                case "Down":
                case "S":
                    nextX = pX;
                    nextY = pY + 1;

                    secondX = pX;
                    secondY = pY + 2;

                    move(90);
                    break;
            }
        }

        private void move(int rotation)
        {
            Console.WriteLine(pX + " : " + pY);
            rotate(rotation);
            if (modelLevel.levelData.Tilemap[nextY, nextX] == null)
            {
                if(modelLevel.levelData.Tiles[nextY][nextX] is Floor || modelLevel.levelData.Tiles[nextY][nextX] is Dest)
                {
                    updateMoves();

                    modelLevel.levelData.Tilemap[nextY, nextX] = modelLevel.levelData.Tilemap[pY, pX];
                    modelLevel.levelData.Tilemap[pY, pX] = null;

                    modelLevel.levelData.player.X = nextX; 
                    modelLevel.levelData.player.Y = nextY;

                    gameGrid.reDrawPlayer();
                }
            }
            else
            {
                if(modelLevel.levelData.Tilemap[secondY, secondX] == null && !(modelLevel.levelData.Tiles[secondY][secondX] is Wall))
                {
                    updateMoves();

                    if(modelLevel.levelData.Tiles[secondY][secondX] is Dest)
                    {
                        Deliver delivery = new Deliver();
                        modelLevel.levelData.Tilemap[secondY, secondX] = delivery;
                        modelLevel.levelData.Tilemap[nextY, nextX] = modelLevel.levelData.Tilemap[pY, pX];
                        modelLevel.levelData.Tilemap[pY, pX] = null;
                        gameGrid.reDrawFloor(nextX, nextY);
                    } 
                    else if (modelLevel.levelData.Tilemap[nextY, nextX] is Deliver)
                    {
                        Crate crate = new Crate();
                        modelLevel.levelData.Tilemap[secondY, secondX] = crate;
                        modelLevel.levelData.Tilemap[nextY, nextX] = modelLevel.levelData.Tilemap[pY,pX];
                        modelLevel.levelData.Tilemap[pY, pX] = null;
                        gameGrid.reDrawFloor(nextX, nextY);

                    }
                    else
                    {
                        modelLevel.levelData.Tilemap[secondY, secondX] = modelLevel.levelData.Tilemap[nextY, nextX];
                        modelLevel.levelData.Tilemap[nextY, nextX] = modelLevel.levelData.Tilemap[pY,pX];
                        modelLevel.levelData.Tilemap[pY, pX] = null;
                    }

                    modelLevel.levelData.player.X = nextX;
                    modelLevel.levelData.player.Y = nextY;

                    modelLevel.levelData.Tilemap[secondY, secondX].X = secondX;
                    modelLevel.levelData.Tilemap[secondY, secondX].Y = secondY;

                    gameGrid.reDrawComp(secondX, secondY);

                    checkGameStatus();
                }
            }
        }

        private void checkGameStatus()
        {
            if(modelLevel.levelData.Tiles[secondY][secondX] is Dest)
            {
                modelLevel.levelData.remaingTargets--;
            }
            if(modelLevel.levelData.Tiles[nextY][nextX] is Dest)
            {
                modelLevel.levelData.remaingTargets++;
            }
            if(modelLevel.levelData.remaingTargets.Equals(0))
            {
                levelFinished();
            }
            targetStatus();
        }
        private void rotate(int x)
        {
            RotateTransform angle = new RotateTransform();
            angle.CenterY = 15;
            angle.CenterX = 15;
            angle.Angle = x;
            //modelLevel.levelData.Tilemap[pY, pX].RenderTransform = angle;
            modelLevel.levelData.player.RenderTransform = angle;
        }

        private void updateMoves()
        {
            modelLevel.levelData.Moves++;
            moves.Content = "Moves : " + modelLevel.levelData.Moves;
        }

        private void targetStatus()
        {
            targets.Content = ": " + modelLevel.levelData.remaingTargets;
        }

        private void levelFinished()
        {
            modelLevel.inGame = false;
            LevelComplete completeDLG = new LevelComplete(true, new core.events.ReplayLevel(nextLevel));
            completeDLG.Owner = this;
            completeDLG.ShowDialog();
        }

        public void nextLevel()
        {
            bool next = false;
            string nextMap = null;

            for(int i = 0; i < modelLevel.Levels.Count(); i++)
            {
                if(modelLevel.Levels[i].Equals(modelLevel.CurrentLevel))
                {
                    if(i + 1 >= modelLevel.Levels.Count())
                    {
                        next = false;
                    } else
                    {
                        nextMap = modelLevel.Levels[i + 1];
                        next = true;
                    }
                    break;
                }
            }
            if(next)
            {
                Init(nextMap);
            }
            else
            {
                Console.WriteLine("no other map");
            }
        }

        private void powerup()
        {
        }
    }
}
