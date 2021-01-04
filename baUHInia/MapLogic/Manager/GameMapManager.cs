using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using baUHInia.MapLogic.Helper;
using System.Text;
using baUHInia.Database;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.MapLogic.Manager
{
    class GameMapManager : IGameMapManager
    {
        // Database
        private BazaDanych db;

        // Logic
        private string Choice;
        private string ChoiceId;
        private string Keyword;
        private bool ClearSelection = true;

        // Modes - 0: MapLoad, 1: MapSave 2: GameLoad, 3: GameSave.

        private string[] MapNames;
        private Tuple<int,string>[] GameIDsNames;


        // Layout
        private Grid LoadMapContainerGrid;
        private Grid SaveMapContainerGrid;
        private Grid LoadGameContainerGrid;
        private Grid SaveGameContainerGrid;

        private TextBox SaveMapNameTextBox;
        private TextBox SaveGameNameTextBox;

        public GameMapManager()
        {
            Choice = "";
            ChoiceId = "";
            Keyword = "";

            db = new BazaDanych();
        }

        public Grid GetMapLoadGrid()
        {
            Choice = "";
            Keyword = "";

            MapNames = db.getMapNames();

            LoadMapContainerGrid = CreateContainerGrid();
            ScrollViewer loadListScrollViewer = CreateListScrollViewer();
            Grid loadListGrid = CreateListGrid();
            Grid loadSearchGrid = CreateSearchGrid(loadListGrid,0);

            LoadMapContainerGrid.Children.Add(loadSearchGrid);
            LoadMapContainerGrid.Children.Add(loadListScrollViewer);
            loadListScrollViewer.Content = loadListGrid;

            PopulateListGrid(loadListGrid,0);

            return LoadMapContainerGrid;
        }

        public Grid GetMapSaveGrid()
        {
            Choice = "";
            Keyword = "";

            MapNames = db.getMapNames();

            SaveMapContainerGrid = CreateContainerGrid();
            ScrollViewer saveListScrollViewer = CreateListScrollViewer();
            Grid saveListGrid = CreateListGrid();
            Grid saveSearchGrid = CreateSearchGrid(saveListGrid,1);
            Grid nameGrid = CreateNameGrid();
            SaveMapNameTextBox = CreateNameTextBox(saveListGrid);

            nameGrid.Children.Add(SaveMapNameTextBox);
            SaveMapContainerGrid.Children.Add(saveSearchGrid);
            SaveMapContainerGrid.Children.Add(saveListScrollViewer);
            saveListScrollViewer.Content = saveListGrid;
            SaveMapContainerGrid.Children.Add(nameGrid);

            PopulateListGrid(saveListGrid,1);

            return SaveMapContainerGrid;
        }

        public Grid GetGameLoadGrid()
        {
            GameIDsNames = db.getGameNamesAndID();

            LoadGameContainerGrid = CreateContainerGrid();
            ScrollViewer loadListScrollViewer = CreateListScrollViewer();
            Grid loadListGrid = CreateListGrid();
            Grid loadSearchGrid = CreateSearchGrid(loadListGrid, 2);

            LoadGameContainerGrid.Children.Add(loadSearchGrid);
            LoadGameContainerGrid.Children.Add(loadListScrollViewer);
            loadListScrollViewer.Content = loadListGrid;

            PopulateListGrid(loadListGrid, 2);

            return LoadGameContainerGrid;
        }


        public Grid GetGameSaveGrid()
        {
            GameIDsNames = db.getGameNamesAndID();

            SaveGameContainerGrid = CreateContainerGrid();
            ScrollViewer saveListScrollViewer = CreateListScrollViewer();
            Grid saveListGrid = CreateListGrid();
            Grid saveSearchGrid = CreateSearchGrid(saveListGrid, 3);
            Grid nameGrid = CreateNameGrid();
            SaveGameNameTextBox = CreateNameTextBox(saveListGrid);

            nameGrid.Children.Add(SaveGameNameTextBox);
            SaveGameContainerGrid.Children.Add(saveSearchGrid);
            SaveGameContainerGrid.Children.Add(saveListScrollViewer);
            saveListScrollViewer.Content = saveListGrid;
            SaveGameContainerGrid.Children.Add(nameGrid);

            PopulateListGrid(saveListGrid, 3);

            return SaveGameContainerGrid;
        }

        public Game LoadGame(string name) // Not tested might not work yet.
        {
            throw new NotImplementedException();

            if (Choice == "" || ChoiceId == "")
            {
                throw new Exception("Name or Id cannot be empty.");
            }

            string jsonStringGame = null;
            db.GetMap(ref jsonStringGame, Choice);

            JObject jsonGame = JObject.Parse(jsonStringGame);

            SerializationHelper.JsonGetPlacedObjects(jsonGame, out var placedObjects);

            string gameName = Choice;

            Choice = (string)jsonGame["MapName"]; // Using name instread of id , a temporary solution - or isnt it?

            Map map = LoadMap("co tam pietras pewnie błąd tu masz hehe");

            return new Game(123, 123, 123, gameName, placedObjects, map);
        }

        public Map LoadMap(string name)
        {
            // TODO get credentials from database.

            if (Choice == "")
            {
                throw new Exception("Name cannot be empty.");
            }

            string jsonStringMap = null;
            db.GetMap(ref jsonStringMap, Choice);

            JObject jsonMap = JObject.Parse(jsonStringMap);

            int size = 0;
            int availableMoney = 0;

            SerializationHelper.JsonGetBasicData(jsonMap, ref name, ref size, ref availableMoney);

            int[,] tileGrid = new int[size, size];
            bool[,] placeableGrid = new bool[size, size];

            Dictionary<int, string> indexer = new Dictionary<int, string>();

            SerializationHelper.JsonGetTileGridAndDictionary(jsonMap, indexer, tileGrid, placeableGrid);

            SerializationHelper.JsonGetAvailableTiles(jsonMap, out var availableTiles);

            SerializationHelper.JsonGetPlacedObjects(jsonMap, out var placedObjects);

            for (int i = 0; i < placedObjects.GetLength(0); i++)
            {
                Console.WriteLine(((Placement)placedObjects[i]).GameObject.TileObject.Name);
            }

            return new Map(123, 123, Choice, tileGrid, placeableGrid, indexer, availableTiles, availableMoney, placedObjects);
        }

        public bool SaveGame(ITileBinder tileBinder)
        {
            throw new NotImplementedException();

            if (Choice == "" || ChoiceId == "")
            {
                throw new Exception("Name or Id cannot be empty.");
            }

            int id = int.Parse(ChoiceId.Substring(2,ChoiceId.Length-2));

            JObject jsonGame = new JObject();
            SerializationHelper.JsonAddPlacements(jsonGame, tileBinder.PlacedObjects);

            db.addGame(123, Choice, jsonGame.ToString(Formatting.None), 123);

            Console.WriteLine(jsonGame.ToString(Formatting.None));

            return true;
        }

        public bool SaveMap(ITileBinder tileBinder)
        {
            if (Choice == "")
            {
                throw new Exception("Name cannot be empty.");
            }

            JObject jsonMap = new JObject();

            SerializationHelper.JsonAddBasicData(jsonMap, tileBinder, Choice);
            SerializationHelper.JsonAddTileGridAndDictionary(jsonMap, tileBinder.TileGrid);
            SerializationHelper.JsonAddPlacements(jsonMap, tileBinder.PlacedObjects);
            SerializationHelper.JsonAddAvailableTiles(jsonMap, tileBinder.AvailableObjects);

            BazaDanych db = new BazaDanych();
            int result = db.DodajMape(123, Choice, jsonMap.ToString(Formatting.None));

            if (result == 33)
            {
                return db.updateMap(123, jsonMap.ToString(Formatting.None), Choice);
            }

            if (result != 0)
            {
                Console.WriteLine("Attempt to save map '" + Choice + "' in database failed - code: " + result + ".");
                return false;
            }

            return true;
        }

        private Grid CreateContainerGrid()
        {
            return new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
        }

        private Grid CreateNameGrid()
        {
            return new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 30,
                Background = Brushes.Blue,
            };
        }

        private TextBox CreateNameTextBox(Grid listGrid)
        {
            TextBox nameTextBox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0),
                Background = (Brush)new BrushConverter().ConvertFrom("#FFA7A7A7"),
                Padding = new Thickness(5, 0, 5, 0),
            };
            nameTextBox.TextChanged += (s,a) => { NameTextChanged(s,listGrid); };

            return nameTextBox;
        }

        private Grid CreateSearchGrid(Grid listGrid, int mode)
        {
            Grid searchGrid = new Grid
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
            searchButton.Click += (s,a) => { SearchButtonClick(listGrid,mode); };

            searchGrid.Children.Add(searchTextBox);
            searchGrid.Children.Add(searchButton);

            return searchGrid;
        }

        private ScrollViewer CreateListScrollViewer()
        {
            return new ScrollViewer
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0, 30, 0, 0),
                Background = (Brush) new BrushConverter().ConvertFrom("#FFA7A7A7")
            };
        }

        private Grid CreateListGrid()
        {
            return new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Name = "ListGrid"
            };
        }

        private void PopulateListGrid(Grid listGrid,int mode)
        {
            listGrid.RowDefinitions.Clear();
            listGrid.Children.Clear();

            int index = 0;

            if (mode == 0 || mode == 1)
            {
                string[] filteredMapNames = MapNames.Where(m => m.Contains(Keyword)).ToArray();

                foreach (string name in filteredMapNames)
                {
                    listGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    Grid listItemGrid = new Grid {HorizontalAlignment = HorizontalAlignment.Stretch};
                    Grid.SetRow(listItemGrid, index);
                    listItemGrid.Children.Add(CreateListItemButton(name,listGrid));
                    listGrid.Children.Add(listItemGrid);
                    index++;
                }
            }
            else if (mode == 2 || mode == 3)
            {
                Tuple<int, string>[] filteredGameIDNames = GameIDsNames.Where(g => g.Item2.Contains(Keyword)).ToArray();

                foreach (Tuple<int, string> gameIDName in filteredGameIDNames)
                {
                    listGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    Grid listItemGrid = new Grid { HorizontalAlignment = HorizontalAlignment.Stretch };
                    Button listItemButton = CreateListItemButton(gameIDName.Item2, listGrid);
                    listItemButton.Name = "id" + gameIDName.Item1.ToString();
                    Grid.SetRow(listItemGrid, index);
                    listItemGrid.Children.Add(listItemButton);
                    listGrid.Children.Add(listItemGrid);
                    index++;
                }
            }
        }

        private Button CreateListItemButton(string name, Grid listGrid)
        {
            TextBlock nameTextBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                FontFamily = new FontFamily("Segoe UI"),
                FontWeight = FontWeights.Bold,
                Text = name
            };

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
                Content = nameTextBlock // Has to be a TextBlock, otherwise cuts out some special characters.
            };
            listItemButton.Click += (s,a) => { ListItemButtonClick(s,listGrid); };
            listItemButton.ClickMode = ClickMode.Press;

            return listItemButton;
        }

        // Event handlers

        private void ListItemButtonClick(object s, Grid listGrid)
        {
            foreach (Grid listItemGrid in listGrid.Children)
            {
                Button listItemButton = (Button)listItemGrid.Children[0];
                listItemButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF878787");
                listItemButton.Foreground = Brushes.White;
            }

            Button sender = (Button)s;
            sender.Background = (Brush)new BrushConverter().ConvertFrom("#FF8FAEEC");
            sender.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF474747");
            Choice = ((TextBlock)sender.Content).Text.ToString();
            ChoiceId = sender.Name;

            Console.WriteLine("Choice: " + Choice);
            Console.WriteLine(int.Parse(ChoiceId.Substring(2,ChoiceId.Length-2)));

            ClearSelection = false;

            if (!(SaveMapNameTextBox is null))
            {
                SaveMapNameTextBox.Text = Choice;
            }

            if (!(SaveGameNameTextBox is null))
            {
                SaveGameNameTextBox.Text = Choice;
            }
        }

        private void SearchTextChanged(object s, EventArgs e)
        {
            TextBox sender = (TextBox)s;
            Keyword = sender.Text;
        }

        private void NameTextChanged(object s, Grid listGrid)
        {
            TextBox sender = (TextBox)s;
            Choice = sender.Text;

            if (ClearSelection)
            {
                foreach (Grid listItemGrid in listGrid.Children)
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

        private void SearchButtonClick(Grid listGrid, int mode)
        {
            Choice = "";
            ChoiceId = "";
            PopulateListGrid(listGrid,mode);
        }
        
    }
}
