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
        private DispatcherTimer clock;
        public GameGrid(LevelData ml)
        {
            this.ml = ml;

            var bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom("#FF5EC5F5");

            createGrid();
            renderTile();
            renderCharacters();
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
                    }
                }
            }
        }
        private void createGrid()
        {
            for(int i = 0; i < ml.rows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(ml.GridSize);
                this.RowDefinitions.Add(row);
            }

            for (int i = 0; i < ml.columns; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(ml.GridSize);
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
    }
}