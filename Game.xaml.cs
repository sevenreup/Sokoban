using Sokoban.core;
using Sokoban.core.Level;
using Sokoban.core.Level.Model;
using Sokoban.core.Level.power;
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

            GUIUpdates();
        }

        private void GUIUpdates()
        {
            targetStatus();
            updatePoweUpCount();
        }

        private void StartButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Init(modelLevel.Levels[0]);
        }

        private int pX, pY;
        private int nextX, nextY, secondX, secondY;
        private int direction = 0;
        private void keypress(object sender, KeyEventArgs e)
        {
            pX = modelLevel.levelData.player.X;
            pY = modelLevel.levelData.player.Y;

            switch (e.Key.ToString())
            {
                case "Left":
                case "A":
                    direction = Constant.Left;
                    move();
                    break;
                case "Right":
                case "D":
                    direction = Constant.Right;
                    move();
                    break;
                case "Up":
                case "W":
                    direction = Constant.UP;
                    move();
                    break;
                case "Down":
                case "S":
                    direction = Constant.Down;
                    move();
                    break;
                case "O":
                    usePowerUp(PowerUp.bullet);
                    break;
                case "I":
                    usePowerUp(PowerUp.phase);
                    break;
            }
        }

        private void move()
        {
            checkDirections();
            rotate();
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
        private void rotate()
        {
            RotateTransform angle = new RotateTransform();
            angle.CenterY = 15;
            angle.CenterX = 15;
            angle.Angle = direction;
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

        private void usePowerUp(PowerUp type)
        {
            checkDirections();
            for(int i = 0; i < modelLevel.levelData.PowerUps.Count(); i++)
            {
                if (modelLevel.levelData.PowerUps[i].powerUp == type)
                {
                    if (modelLevel.levelData.PowerUps[i].Count != 0)
                    {
                        
                        switch(modelLevel.levelData.PowerUps[i].powerUp)
                        {
                            case PowerUp.phase:
                                bool afterUse = usePhase();
                                if(afterUse)
                                {
                                    modelLevel.levelData.PowerUps[i].Count--;
                                    updatePoweUpCount();
                                }
                                break;
                            case PowerUp.bullet:
                                break;
                        }
                    }
                }
            }
        }

        private bool usePhase()
        {
            if (modelLevel.levelData.Tilemap[nextY, nextX] == null && modelLevel.levelData.Tiles[nextY][nextX] is Wall)
            {
                if ((secondX > 0 && secondY > 0) && (secondX < modelLevel.levelData.columns && secondY < modelLevel.levelData.rows))
                {
                    if (!(modelLevel.levelData.Tiles[secondY][secondX] is Wall) && modelLevel.levelData.Tilemap[nextY, nextX] == null)
                    {
                        modelLevel.levelData.Tilemap[secondY, secondX] = modelLevel.levelData.Tilemap[pY, pX];
                        modelLevel.levelData.Tilemap[pY, pX] = null;

                        modelLevel.levelData.Tilemap[secondY, secondX].X = secondX;
                        modelLevel.levelData.Tilemap[secondY, secondX].Y = secondY;

                        modelLevel.levelData.player.X = secondX;
                        modelLevel.levelData.player.Y = secondY;

                        gameGrid.reDrawPlayer();
                        return true;
                    }
                }
            }
            return false;
        }

        private void updatePoweUpCount()
        {
            foreach(PowerUpHolder powerUp in modelLevel.levelData.PowerUps)
            {
                switch(powerUp.powerUp)
                {
                    case PowerUp.phase:
                        pupPhase.Content = ": " + powerUp.Count;
                        break;
                    case PowerUp.bullet:
                        pupBullet.Content = ": " + powerUp.Count;
                        break;
                }
            }
        }

        private void checkDirections()
        {
            switch(direction)
            {
                case Constant.UP:
                    nextX = pX;
                    nextY = pY - 1;

                    secondX = pX;
                    secondY = pY - 2;
                    break;
                case Constant.Down:
                    nextX = pX;
                    nextY = pY + 1;

                    secondX = pX;
                    secondY = pY + 2;
                    break;
                case Constant.Left:
                    nextX = pX - 1;
                    nextY = pY;

                    secondX = pX - 2;
                    secondY = pY;
                    break;
                case Constant.Right:
                    nextX = pX + 1;
                    nextY = pY;

                    secondX = pX + 2;
                    secondY = pY;
                    break;
            }
        }
    }
}
