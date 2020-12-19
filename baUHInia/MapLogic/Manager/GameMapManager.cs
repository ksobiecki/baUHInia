using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using baUHInia.Playground.Model.Tiles;

using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using baUHInia.Playground.Model.Wrappers;
using System.IO;
using baUHInia.MapLogic.Helper;
using System.Text;

namespace baUHInia.MapLogic.Manager
{
    class GameMapManager : IGameMapManager
    {
        // Logic
        private string Choice;
        private string Keyword;
        private bool ClearSelection = true;

        // Determines if operating on maps or games. Absolutely disgusting solution but thats all I can come up with at the moment.
        private int Mode; // 0: Map, 1: MapSave 2: GameLoad, 3: GameSave.
        private Map[] Maps;
        private Game[] Games;

        // Layout
        private Grid LoadContainerGrid;
        private Grid SaveContainerGrid;
        private Grid SearchGrid;
        private ScrollViewer ListScrollViewer;
        private Grid ListGrid;
        private Grid SaveGrid;
        TextBox nameTextBox;

        public GameMapManager()
        {
            // Debug purposes, remove later.
            Maps = new Map[50];
            Games = new Game[50];
            for (int i = 0; i < 50; i++)
            {
                Maps[i] = new Map("Some map nr" + i);
                Games[i] = new Game("Some game nr" + i);
            }

            CreateContainerGrids();
            CreateSearchGrid();
            CreateListScrollViewer();
            CreateListGrid();

            Choice = "";
            Keyword = "";
        }

        public Grid GetGameLoadGrid()
        {
            // TODO get games from database.

            Mode = 2;
            Choice = "";
            Keyword = "";

            LoadContainerGrid.Children.Clear();
            LoadContainerGrid.Children.Add(SearchGrid);
            LoadContainerGrid.Children.Add(ListScrollViewer);
            ListScrollViewer.Content = ListGrid;

            PopulateListGrid();

            return LoadContainerGrid;
        }

        public Grid GetMapLoadGrid()
        {
            // TODO get maps from database.

            Mode = 0;
            Choice = "";
            Keyword = "";

            LoadContainerGrid.Children.Clear();
            LoadContainerGrid.Children.Add(SearchGrid);
            LoadContainerGrid.Children.Add(ListScrollViewer);
            ListScrollViewer.Content = ListGrid;

            PopulateListGrid();

            return LoadContainerGrid;
        }

        public Grid GetMapSaveGrid()
        {
            Mode = 1;
            Choice = "";
            Keyword = "";
            SaveContainerGrid.Children.Clear();
            SaveContainerGrid.Children.Add(SearchGrid);
            SaveContainerGrid.Children.Add(ListScrollViewer);
            ListScrollViewer.Content = ListGrid;

            PopulateListGrid();
            CreateSaveGrid();
            SaveContainerGrid.Children.Add(SaveGrid);
            return SaveContainerGrid;
        }

        public Grid GetGameSaveGrid()
        {
            throw new NotImplementedException();
        }

        public Game LoadGame(string name)
        {
            string readText = File.ReadAllText("C:/test_game.txt", Encoding.UTF8); // TODO switch to database methods when they are available.

            JObject jsonGame = JObject.Parse(readText);
            Placement[] placedObjects = null;

            SerializationHelper.JsonGetPlacedObjects(jsonGame, placedObjects);

            return new Game(123, 123, 123, name, placedObjects, null);
        }

        public Map LoadMap(string name)
        {
            string readText = File.ReadAllText("C:/test_map.txt", Encoding.UTF8); // TODO switch to database methods when they are available.
            // TODO get credentials from database.

            JObject jsonMap = JObject.Parse(readText);

            int size = 0;
            int availableMoney = 0;

            SerializationHelper.JsonGetBasicData(jsonMap, ref name, ref size, ref availableMoney);

            int[,] tileGrid = new int[size, size];
            bool[,] placeableGrid = new bool[size, size];

            Dictionary<int, string> indexer = new Dictionary<int, string>();

            SerializationHelper.JsonGetTileGridAndDictionary(jsonMap, indexer, tileGrid, placeableGrid);

            GameObject[] availableTiles = null;

            SerializationHelper.JsonGetAvailableTiles(jsonMap, availableTiles);

            Placement[] placedObjects = null;

            SerializationHelper.JsonGetPlacedObjects(jsonMap, placedObjects);

            return new Map(123, 123, name, tileGrid, placeableGrid, indexer, availableTiles, availableMoney, placedObjects);
        }

        public bool SaveGame(ITileBinder tileBinder)
        {
            JObject jsonGame = new JObject();
            SerializationHelper.JsonAddPlacements(jsonGame, tileBinder.PlacedObjects);

            File.WriteAllText("C:/test_game.txt", jsonGame.ToString(Formatting.None), Encoding.UTF8); // TODO switch to database methods when they are available.
            Console.WriteLine(jsonGame.ToString(Formatting.None));

            return true;
        }

        public bool SaveMap(ITileBinder tileBinder)
        {
            JObject jsonMap = new JObject();

            SerializationHelper.JsonAddBasicData(jsonMap, tileBinder);
            SerializationHelper.JsonAddTileGridAndDictionary(jsonMap, tileBinder.TileGrid);
            SerializationHelper.JsonAddPlacements(jsonMap, tileBinder.PlacedObjects);
            SerializationHelper.JsonAddAvailableTiles(jsonMap, tileBinder.AvailableObjects);

            File.WriteAllText("C:/test_map.txt", jsonMap.ToString(Formatting.None), Encoding.UTF8); // TODO switch to database methods when they are available.
            Console.WriteLine(jsonMap.ToString(Formatting.None));

            return true;
        }

        private void CreateContainerGrids()
        {
            LoadContainerGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch, 
                VerticalAlignment = VerticalAlignment.Stretch
            };

            SaveContainerGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch, 
                VerticalAlignment = VerticalAlignment.Stretch
            };
        }

        private void CreateSaveGrid()
        {
            SaveGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 30,
                Background = Brushes.Blue,
            };

            nameTextBox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0),
                Background = (Brush)new BrushConverter().ConvertFrom("#FFA7A7A7"),
                Padding = new Thickness(5, 0, 5, 0),
            };
            nameTextBox.TextChanged += SaveTextChanged;

            SaveGrid.Children.Add(nameTextBox);
        }

        private void CreateSearchGrid()
        {
            SearchGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 30,
                Background = Brushes.Gray
            };

            TextBox searchTextBox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0, 0, 50, 0),
                Padding = new Thickness(5, 0, 5, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = (Brush) new BrushConverter().ConvertFrom("#FFA7A7A7")
            };
            searchTextBox.TextChanged += SearchTextChanged;

            Button searchButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = (Brush) new BrushConverter().ConvertFrom("#FF4A9C38"),
                Foreground = (Brush) new BrushConverter().ConvertFrom("#FF474747"),
                Width = 50,
                Content = "Szukaj"
            };
            searchButton.Foreground = Brushes.White;
            searchButton.FontFamily = new FontFamily("Segoe UI");
            searchButton.FontWeight = FontWeights.Bold;
            searchButton.Click += SearchButtonClick;

            SearchGrid.Children.Add(searchTextBox);
            SearchGrid.Children.Add(searchButton);
        }

        private void CreateListScrollViewer()
        {
            ListScrollViewer = new ScrollViewer
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0, 30, 0, 0),
                Background = (Brush) new BrushConverter().ConvertFrom("#FFA7A7A7")
            };
        }

        private void CreateListGrid()
        {
            ListGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Name = "ListGrid"
            };
        }

        private void PopulateListGrid()
        {
            ListGrid.RowDefinitions.Clear();
            ListGrid.Children.Clear();

            int index = 0;

            if (Mode == 0 || Mode == 1)
            {
                Map[] filteredMaps = Maps.Where(m => m.Name.Contains(Keyword)).ToArray();

                foreach (Map map in filteredMaps)
                {
                    ListGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    Grid listItemGrid = new Grid {HorizontalAlignment = HorizontalAlignment.Stretch};
                    Grid.SetRow(listItemGrid, index);
                    listItemGrid.Children.Add(CreateListItemButton(map.Name));
                    ListGrid.Children.Add(listItemGrid);
                    index++;
                }
            }
            else if (Mode == 2 || Mode == 3)
            {
                Game[] filteredGames = Games.Where(g => g.Name.Contains(Keyword)).ToArray();

                foreach (Game game in filteredGames)
                {
                    ListGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    Grid listItemGrid = new Grid {HorizontalAlignment = HorizontalAlignment.Stretch};
                    Button listItemButton = CreateListItemButton(game.Name);
                    Grid.SetRow(listItemGrid, index);
                    listItemGrid.Children.Add(listItemButton);
                    ListGrid.Children.Add(listItemGrid);
                    index++;
                }
            }
            
        }

        private Button CreateListItemButton(string name)
        {
            Button listItemButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = (Brush) new BrushConverter().ConvertFrom("#FF878787"),
                BorderThickness = new Thickness(0, 1, 0, 0),
                BorderBrush = (Brush) new BrushConverter().ConvertFrom("#FFA7A7A7"),
                Foreground = Brushes.White,
                FontFamily = new FontFamily("Segoe UI"),
                FontWeight = FontWeights.Bold,
                Content = name
            };
            listItemButton.Click += ListItemButtonClick;
            listItemButton.ClickMode = ClickMode.Press;

            return listItemButton;
        }

        // Event handlers

        private void ListItemButtonClick(object s, EventArgs e)
        {
            foreach (Grid listItemGrid in ListGrid.Children)
            {
                Button listItemButton = (Button)listItemGrid.Children[0];
                listItemButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF878787");
                listItemButton.Foreground = Brushes.White;
            }

            Button sender = (Button)s;
            sender.Background = (Brush)new BrushConverter().ConvertFrom("#FF8FAEEC");
            sender.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF474747");
            Choice = sender.Content.ToString();
            ClearSelection = false;
            if (Mode == 1 || Mode == 3)
            {
                nameTextBox.Text = Choice;
            }
        }

        private void SearchTextChanged(object s, EventArgs e)
        {
            TextBox sender = (TextBox)s;
            Keyword = sender.Text;
        }

        private void SaveTextChanged(object s, EventArgs e)
        {
            TextBox sender = (TextBox)s;
            Choice = sender.Text;

            if (ClearSelection)
            {
                foreach (Grid listItemGrid in ListGrid.Children)
                {
                    Button listItemButton = (Button)listItemGrid.Children[0];
                    listItemButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF878787");
                    listItemButton.Foreground = Brushes.White;
                }
            }
            else
            {
                ClearSelection = true;
            }
        }

        private void SearchButtonClick(object s, EventArgs e)
        {
            Choice = "";
            PopulateListGrid();
        }
        
    }
}
