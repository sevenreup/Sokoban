using Sokoban.core;
using Sokoban.core.Level;
using Sokoban.core.Level.Model;
using Sokoban.core.Level.power;
using Sokoban.views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sokoban
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    ///

    public partial class Game : Window
    {
        private static RoutedCommand routedCommand = new RoutedCommand(); 
        LevelLoader levelLoader;
        ModelLevel modelLevel;
        GameGrid gameGrid;

        private int pX, pY;
        private int nextX, nextY, secondX, secondY;
        private int direction = 0;

        private int bulletX, bulletY, bulletDir;
        private bool isShooting = false;
        string nextMap = null;

        GameState gameState;

        public Game()
        {
            InitializeComponent();
            routedCommand.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(routedCommand, CommandBinding_Executed));
            modelLevel = new ModelLevel();
            levelLoader = new LevelLoader(modelLevel);
            gameState = new GameState(new BlockedResponse(checkBlockedState));
            levelLoader.loadLevels();
        }
        public void Init(String levelStr)
        {
            modelLevel.CurrentLevel = levelStr;
            levelLoader.initLevel();
            levelLoader.initiateTileSet();
            

            gameDef.Height = new GridLength(modelLevel.GDHeight);

            this.Cont.Width = 16 + (modelLevel.levelData.columns * modelLevel.GridSize);
            this.Cont.Height = 39 + (modelLevel.levelData.rows * modelLevel.GridSize) + modelLevel.GDHeight;

            Console.WriteLine("Window \n width: " + Cont.Width + "\nHeight: " + Cont.Height);

            this.Cont.Title = "Sokoban : " + modelLevel.levelData.name;

            if(gameGrid != null)
            {
                gameGrid.Visibility = Visibility.Collapsed;
            }

            gameGrid = new GameGrid(modelLevel);
            gameGrid.SetValue(Grid.ColumnProperty, 0);
            gameGrid.SetValue(Grid.RowProperty, 1);
            gameGrid.HorizontalAlignment = HorizontalAlignment.Center;
            gameGrid.VerticalAlignment = VerticalAlignment.Center;

            gameCanvas.Children.Add(gameGrid);

            gameCanvas.Visibility = Visibility.Visible;
            Console.WriteLine("Grid \n width: " + gameCanvas.Width + "\nHeight: " + gameCanvas.Height);
            splash.Visibility = Visibility.Hidden;
            modelLevel.inGame = true;
            GUIUpdates();
        }
        private void GUIUpdates()
        {
            moves.Content = "Moves : " + modelLevel.levelData.Moves;
            targetStatus();
            updatePoweUpCount();
        }
        private void StartButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Init(modelLevel.Levels[0]);
        }
        private void keypress(object sender, KeyEventArgs e)
        {
            if (modelLevel.inGame)
            {
                moveInit(e);
            }
        }
        private void moveInit(KeyEventArgs e)
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
            if (checkGameStatus())
            {
                if (modelLevel.levelData.Tilemap[nextY, nextX] == null)
                {
                    if (modelLevel.levelData.Tiles[nextY][nextX] is Floor || modelLevel.levelData.Tiles[nextY][nextX] is Dest)
                    {
                        updateMoves();

                        modelLevel.levelData.Tilemap[nextY, nextX] = modelLevel.levelData.Tilemap[pY, pX];
                        modelLevel.levelData.Tilemap[pY, pX] = null;

                        modelLevel.levelData.player.X = nextX;
                        modelLevel.levelData.player.Y = nextY;

                        gameGrid.reDrawPlayer();
                        gameState.computeBroke(modelLevel.levelData);
                    }
                }
                else
                {
                    if (modelLevel.levelData.Tilemap[secondY, secondX] == null && !(modelLevel.levelData.Tiles[secondY][secondX] is Wall))
                    {
                        updateMoves();

                        if (modelLevel.levelData.Tiles[secondY][secondX] is Dest)
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
                            modelLevel.levelData.Tilemap[nextY, nextX] = modelLevel.levelData.Tilemap[pY, pX];
                            modelLevel.levelData.Tilemap[pY, pX] = null;
                            gameGrid.reDrawFloor(nextX, nextY);

                        }
                        else
                        {
                            modelLevel.levelData.Tilemap[secondY, secondX] = modelLevel.levelData.Tilemap[nextY, nextX];
                            modelLevel.levelData.Tilemap[nextY, nextX] = modelLevel.levelData.Tilemap[pY, pX];
                            modelLevel.levelData.Tilemap[pY, pX] = null;
                        }

                        modelLevel.levelData.player.X = nextX;
                        modelLevel.levelData.player.Y = nextY;

                        modelLevel.levelData.Tilemap[secondY, secondX].X = secondX;
                        modelLevel.levelData.Tilemap[secondY, secondX].Y = secondY;

                        gameGrid.reDrawComp(secondX, secondY);

                        if(modelLevel.levelData.Tiles[secondY][secondX] is Dest)
                        {
                            modelLevel.levelData.remaingTargets--;
                        }
                        if(modelLevel.levelData.Tiles[nextY][nextX] is Dest)
                        {
                            modelLevel.levelData.remaingTargets++;
                        }
                        targetStatus();
                        
                        if (modelLevel.levelData.remaingTargets <= 0)
                        {
                            GameCompleteStatus(true, "all targets you win");
                        } else
                        {
                            gameState.computeBroke(modelLevel.levelData);
                        }
                    }
                }
            } 
        }
        private bool checkGameStatus()
        {
            if(modelLevel.levelData.Moves <= 0)
            {
                GameCompleteStatus(false, "No more moves");
                return false;
            } 
            return true;
        }
        private void rotate()
        {
            RotateTransform angle = new RotateTransform();
            angle.CenterY = modelLevel.GridSize / 2;
            angle.CenterX = modelLevel.GridSize / 2;
            angle.Angle = direction;
            //modelLevel.levelData.Tilemap[pY, pX].RenderTransform = angle;
            modelLevel.levelData.player.RenderTransform = angle;
        }
        private void updateMoves()
        {
            modelLevel.levelData.Moves--;
            moves.Content = "Moves : " + modelLevel.levelData.Moves;
        }
        private void targetStatus()
        {
            targets.Content = ": " + modelLevel.levelData.remaingTargets;
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
                                if(modelLevel.levelData.numberOfCrates > 0)
                                {
                                    if(!isShooting)
                                    {
                                        isShooting = true;
                                        bool afterUse2 = useBullet();
                                        if(afterUse2)
                                        {
                                            modelLevel.levelData.PowerUps[i].Count--;
                                            updatePoweUpCount();
                                        } 
                                    }
                                }
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
        private void restart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            retry();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameCanvas.Visibility = Visibility.Collapsed;
            splash.Visibility = Visibility.Visible;
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

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LevelEditor levelEditor = new LevelEditor(modelLevel);
            levelEditor.Show();
            levelEditor.Activate();
        }

        private void Help_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Help help = new Help();
            help.Owner = this;
            help.ShowDialog();

        }

        private void exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LevelEditor levelEditor = new LevelEditor(modelLevel);
            levelEditor.Show();
            levelEditor.Activate();
        }

        private bool useBullet()
        {
            if(modelLevel.levelData.Tilemap[nextY,nextX] is Crate)
            {
                gameGrid.destroyCrate(nextX, nextY);
                modelLevel.levelData.Tilemap[nextY, nextX] = null;
                isShooting = false;
                modelLevel.levelData.numberOfCrates--;
                if(modelLevel.levelData.numberOfCrates <= 0)
                {
                    GameCompleteStatus(false,"All crates have been destroyed");
                }
                return true;
            } else
            {
                if(!(modelLevel.levelData.Tiles[nextY][nextX] is Wall))
                {
                    Console.WriteLine("Hit");
                    bulletX = nextX;
                    bulletY = nextY;
                    bulletDir = direction;
                    gameGrid.spawnBullet(nextX, nextY, new EventHandler(handleBullet));
                    return true;
                }
            }
            isShooting = false;
            return false;
        }
        private void handleBullet(object sender, EventArgs e)
        {
            switch (bulletDir)
            {
                case Constant.UP:
                    bulletY--;
                    break;
                case Constant.Down:
                    bulletY++;
                    break;
                case Constant.Left:
                    bulletX --;
                    break;
                case Constant.Right:
                    bulletX++;
                    break;
            }
            
            if(bulletX >= 0 && bulletY >= 0 && bulletX < modelLevel.levelData.rows && bulletY < modelLevel.levelData.columns)
            {
                
                if (modelLevel.levelData.Tilemap[bulletY, bulletX] is Crate)
                {
                    isShooting = false;
                    gameGrid.destroyBullet(bulletX, bulletY);
                    gameGrid.destroyCrate(bulletX, bulletY);
                    modelLevel.levelData.Tilemap[bulletY, bulletX] = null;
                    Console.WriteLine("(" + bulletX + ", " + bulletY + ")");
                    modelLevel.levelData.numberOfCrates--;
                    if (modelLevel.levelData.numberOfCrates <= 0)
                    {
                        GameCompleteStatus(false, "All crates have been destroyed");
                    }
                } 
                else if(modelLevel.levelData.Tiles[bulletY][bulletX] is Wall)
                {
                    gameGrid.destroyBullet(bulletX, bulletY);
                    isShooting = false;
                } else
                {
                    gameGrid.redrawBullet(bulletX, bulletY);
                }
            }
            else
            {
                isShooting = false;
                gameGrid.destroyBullet(nextX, nextY);
            }
        }
        private void GameCompleteStatus(bool status, String message)
        {
            if (status) {
                for (int i = 0; i < modelLevel.Levels.Count(); i++)
                {
                    if (modelLevel.Levels[i].Equals(modelLevel.CurrentLevel))
                    {
                        if (i + 1 >= modelLevel.Levels.Count())
                        {
                            status = false;
                        }
                        else
                        {
                            nextMap = modelLevel.Levels[i + 1];
                            status = true;
                        }
                        break;
                    }
                }
            }
            modelLevel.inGame = false;
            StatusDialog failedDialog = new StatusDialog(status, message, new NextLevel(nextLevel), new RetryLevel(retry), new PreviousLevel(retry));
            failedDialog.Owner = this;
            failedDialog.ShowDialog();
        }
        public void nextLevel()
        {
            Init(nextMap);
        }
        public void retry()
        {
            this.Init(modelLevel.CurrentLevel);
        }
        private void checkBlockedState(bool blocked)
        {
            if(blocked)
            {
                GameCompleteStatus(false, "Crates cannot be moved");
            }
        }
    }
}
