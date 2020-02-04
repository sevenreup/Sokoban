using Sokoban.core.Level.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sokoban.core.Level
{
    class GameGrid : Grid
    {
        LevelData ml;
        ModelLevel model;
        private DispatcherTimer bulletTimer;
        Bullet bullet;

        public GameGrid(ModelLevel model)
        {
            this.model = model;
            this.ml = model.levelData;

            createGrid();
            renderTile();
            renderCharacters();
            bulletTimer = new System.Windows.Threading.DispatcherTimer();
            bulletTimer.Interval = TimeSpan.FromMilliseconds(20);
        }

        private void renderTile()
        {
            for(int y = 0; y < ml.rows; y++)
            {
                for(int x = 0; x < ml.columns; x++)
                {
                    Tile tile = ml.Tiles[y][x];
                    Console.WriteLine("Tile : " + tile.image + ", (x: " + tile.X + ", Y: " + tile.Y + ")");
                    tile.SetValue(Grid.ColumnProperty, x);
                    tile.SetValue(Grid.RowProperty, y);
                    this.Children.Remove(tile);
                    this.Children.Add(tile);
                }
            }
        }
        private void renderCharacters()
        {
            for(int y = 0; y < ml.rows; y++)
            {
                for(int x = 0; x < ml.columns; x++)
                {
                    Tile tile = ml.Tilemap[y,x];
                    if (tile != null)
                    {
                        tile.SetValue(Grid.ColumnProperty, x);
                        tile.SetValue(Grid.RowProperty, y);
                        Console.WriteLine("Dynamic : " + tile.image + ", (x: " + tile.X + ", Y: " + tile.Y + ")");
                        this.Children.Remove(tile);
                        this.Children.Add(tile);

                        #region MyRegion
                        if ((tile is Player))
                            
                            {
                                Player player = (Player)tile;
                                player.player.SetValue(Grid.ColumnProperty, x);
                                player.player.SetValue(Grid.RowProperty, y);

                                this.Children.Remove(player.player);
                                this.Children.Add(player.player);
                            } 
                        #endregion

                    }
                }
            }
        }
        private void createGrid()
        {
            for(int i = 0; i < ml.rows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(model.GridSize);
                this.RowDefinitions.Add(row);
            }

            for (int i = 0; i < ml.columns; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(model.GridSize);
                this.ColumnDefinitions.Add(col);
            }
        }
        public void reDrawPlayer()
        {
            ml.player.SetValue(Grid.ColumnProperty, ml.player.X);
            ml.player.SetValue(Grid.RowProperty, ml.player.Y);

            this.Children.Remove(ml.player);
            this.Children.Add(ml.player);
        }
        public void reDrawFloor(int x, int y)
        {
            Tile t = ml.Tiles[y][x];
            t.SetValue(Grid.ColumnProperty, x);
            t.SetValue(Grid.RowProperty, y);
            this.Children.Remove(t);
            this.Children.Add(t);
        }
        public void reDrawComp(int x, int y)
        {
            reDrawPlayer();
            ml.Tilemap[y, x].SetValue(Grid.ColumnProperty, ml.Tilemap[y, x].X);
            ml.Tilemap[y, x].SetValue(Grid.RowProperty, ml.Tilemap[y, x].Y);
            this.Children.Remove(ml.Tilemap[y, x]);
            this.Children.Add(ml.Tilemap[y, x]);
        }

        public void spawnBullet(int x, int y, EventHandler handler)
        {
            bullet = new Bullet(x, y);
            bullet.SetValue(Grid.ColumnProperty, x);
            bullet.SetValue(Grid.RowProperty, y);

            this.Children.Add(bullet);
            bulletTimer.Tick += handler;
            bulletTimer.Start();
        }

        public void redrawBullet(int x, int y)
        {
            bullet.SetValue(Grid.ColumnProperty, x);
            bullet.SetValue(Grid.RowProperty, y);

            this.Children.Remove(bullet);
            this.Children.Add(bullet);
        }

        public void destroyCrate(int x, int y)
        {
            reDrawFloor(x, y);
            this.Children.Remove(ml.Tilemap[y, x]);
        }

        public void destroyBullet(int x, int y)
        {
            this.Children.Remove(bullet);
            bulletTimer.Stop();
            bulletTimer.Tick += null;
        }
    }
}