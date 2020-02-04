using Sokoban.core;
using Sokoban.core.Level;
using Sokoban.core.Level.Model;
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

namespace Sokoban.views
{
    /// <summary>
    /// Interaction logic for LevelEditor.xaml
    /// </summary>
    public partial class LevelEditor : Window
    {
        List<List<Tile>> tiles = new List<List<Tile>>();
        int size = 29;
        ModelLevel modelLevel;
        LevelLoader levelLoader;
        String file;

        public LevelEditor(ModelLevel modelLevel)
        {
            InitializeComponent();
            this.modelLevel = modelLevel;
            levelLoader = new LevelLoader(modelLevel);
            availableLevels.ItemsSource = modelLevel.Levels;

            tileList.Items.Add(new Player());
            tileList.Items.Add(new Wall());
            tileList.Items.Add(new Floor());
            tileList.Items.Add(new Dest());
            tileList.Items.Add(new Crate());
            tileList.Items.Add(new Empty());
            tileList.SelectedIndex = 0;

            setup();
        }

        private void setup()
        {
            for (int i = 0; i < size; i++)
            {
                List<Tile> temp = new List<Tile>();

                for (int y = 0; y < size; y++)
                {
                    temp.Add(new Empty());
                }
                tiles.Add(temp);

            }
            for (int i = 0; i < size; i++)
            {
                horizontal();
                vertical();
            }

        }

        private void levelTile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int mX = (int)Mouse.GetPosition(levelTiles).X;
            int mY = (int)Mouse.GetPosition(levelTiles).Y;

            int column = mX / modelLevel.GridSize;
            int row = mY / modelLevel.GridSize;

            createTile(column, row);
        }

        private void createTile(int x, int y)
        {
            Tile tile;
            Console.WriteLine(tileList.SelectedItem.ToString());
            switch (tileList.SelectedItem.ToString())
            {
                case "Sokoban.core.Level.Model.Player":
                    tile = new Player();
                    break;
                case "Sokoban.core.Level.Model.Wall":
                    tile = new Wall();
                    break;
                case "Sokoban.core.Level.Model.Floor":
                    tile = new Floor();
                    break;
                case "Sokoban.core.Level.Model.Dest":
                    tile = new Dest();
                    break;
                case "Sokoban.core.Level.Model.Crate":
                    tile = new Crate();
                    break;
                default:
                    tile = new Empty();
                    break;
            }

            tile.SetValue(Grid.ColumnProperty, x);
            tile.SetValue(Grid.RowProperty, y);

            levelTiles.Children.Remove(tiles[y][x]);
            tiles[y][x] = tile;
            levelTiles.Children.Add(tiles[y][x]);
        }

        private void delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            levelTiles.Children.Clear();
            levelTiles.ColumnDefinitions.Clear();
            levelTiles.RowDefinitions.Clear();
            setup();
        }

        private void clear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            levelTiles.Children.Clear();
            levelTiles.ColumnDefinitions.Clear();
            levelTiles.RowDefinitions.Clear();
            tiles = new List<List<Tile>>();
            setup();
        }

        private void horizontal()
        {
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(modelLevel.GridSize);
            levelTiles.ColumnDefinitions.Add(column);
        }

        public void vertical()
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(modelLevel.GridSize);
            levelTiles.RowDefinitions.Add(row);
        }

        private void save_MouseDown(object sender, MouseButtonEventArgs e)
        {

            LevelSaver levelSaver = new LevelSaver(tiles, levelname.Text, numberMoves.Text, bullet.IsChecked.Value, warp.IsChecked.Value);
        }

        private void useMap()
        {
            levelTiles.Children.Clear();
            levelTiles.ColumnDefinitions.Clear();
            levelTiles.RowDefinitions.Clear();
            tiles.Clear();

            List<List<String>> tempM = modelLevel.tempLevData;

            for (int i = 0; i < tempM.Count; i++)
            {
                horizontal();
                vertical();
            }

            for (int y = 0; y < tempM.Count; y++)
            {
                List<Tile> temp = new List<Tile>();

                for (int x = 0; x < tempM.Count; x++)
                {
                    Tile tile;
                    switch (tempM[y][x])
                    {
                        case "o":
                            tile = new Player();
                            break;
                        case "#":
                            tile = new Wall();
                            break;
                        case " ":
                            tile = new Floor();
                            break;
                        case "@":
                            tile = new Dest();
                            break;
                        case "x":
                            tile = new Crate();
                            break;
                        default:
                            tile = new Empty();
                            break;
                    }
                    tile.SetValue(Grid.ColumnProperty, x);
                    tile.SetValue(Grid.RowProperty, y);

                    temp.Add(tile);
                    levelTiles.Children.Remove(tile);
                    levelTiles.Children.Add(tile);
                }
                tiles.Add(temp);

            }

        }

        private void availableLevels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            modelLevel.CurrentLevel = availableLevels.SelectedItem.ToString();
            levelLoader.initLevel();
            levelLoader.initiateTileSet();
            useMap();
            file = availableLevels.SelectedItem.ToString();
        }
    }
}