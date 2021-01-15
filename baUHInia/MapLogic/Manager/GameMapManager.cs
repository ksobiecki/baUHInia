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
using baUHInia.Authorisation;

namespace baUHInia.MapLogic.Manager
{
    class GameMapManager : IGameMapManager
    {
        // Database
        private BazaDanych db;

        // Logic
        private string Choice;
        private int ChoiceId;
        private bool Publish = false;
        private string Keyword;
        private bool ClearSelection = true;
        private LoginData Credentials;

        private IEnumerable<dynamic> LastPopulated;

        // Modes - 0: MapLoad, 1: MapSave 2: GameLoad, 3: GameSave.

        private Tuple<int, int, string, bool>[] MapList;
        private Tuple<int, int, string, bool>[] GameList;


        // Layout
        private Grid LoadMapContainerGrid;
        private Grid SaveMapContainerGrid;
        private Grid LoadGameContainerGrid;
        private Grid SaveGameContainerGrid;

        private Grid LoadMapListGrid;
        private Grid SaveMapListGrid;
        private Grid LoadGameListGrid;
        private Grid SaveGameListGrid;


        private TextBox SaveMapNameTextBox;
        private TextBox SaveGameNameTextBox;
        private CheckBox SaveMapCheckBox;
        private CheckBox SaveGameCheckBox;

        public GameMapManager(LoginData credentials)
        {
            Choice = "";
            ChoiceId = -1;
            Keyword = "";
            Credentials = credentials;

            db = new BazaDanych();
        }

        public Grid GetMapLoadGrid()
        {
            Choice = "";
            Keyword = "";

            MapList = db.getMapList();

            LoadMapContainerGrid = CreateContainerGrid();
            ScrollViewer loadListScrollViewer = CreateListScrollViewer(0);

            LoadMapListGrid = CreateListGrid();

            Grid loadSearchGrid = CreateSearchGrid(LoadMapListGrid, 0);

            LoadMapContainerGrid.Children.Add(loadSearchGrid);
            LoadMapContainerGrid.Children.Add(loadListScrollViewer);
            loadListScrollViewer.Content = LoadMapListGrid;

            return LoadMapContainerGrid;
        }

        public Grid GetMapSaveGrid()
        {
            Choice = "";
            Keyword = "";

            MapList = db.getMapList();

            SaveMapContainerGrid = CreateContainerGrid();

            ScrollViewer saveListScrollViewer = CreateListScrollViewer(1);
            SaveMapListGrid = CreateListGrid();
            Grid saveSearchGrid = CreateSearchGrid(SaveMapListGrid, 1);
            Grid nameGrid = CreateNameGrid(ref SaveMapCheckBox, SaveMapListGrid, 1);
            SaveMapNameTextBox = CreateNameTextBox(SaveMapListGrid);

            nameGrid.Children.Add(SaveMapNameTextBox);
            SaveMapContainerGrid.Children.Add(saveSearchGrid);
            SaveMapContainerGrid.Children.Add(saveListScrollViewer);
            saveListScrollViewer.Content = SaveMapListGrid;
            SaveMapContainerGrid.Children.Add(nameGrid);

            SaveMapCheckBox.IsChecked = false;

            return SaveMapContainerGrid;
        }

        public Grid GetGameLoadGrid()
        {
            if (Credentials.isAdmin)
            {
                GameList = db.getGameList();
            }
            else
            {
                GameList = db.getGameList(); // Replace with method that returns only games given user owns.
            }

            LoadGameContainerGrid = CreateContainerGrid();
            ScrollViewer loadListScrollViewer = CreateListScrollViewer(2);
            LoadGameListGrid = CreateListGrid();
            Grid loadSearchGrid = CreateSearchGrid(LoadGameListGrid, 2);

            LoadGameContainerGrid.Children.Add(loadSearchGrid);
            LoadGameContainerGrid.Children.Add(loadListScrollViewer);
            loadListScrollViewer.Content = LoadGameListGrid;


            return LoadGameContainerGrid;
        }


        public Grid GetGameSaveGrid()
        {
            GameList = db.getGameList();

            SaveGameContainerGrid = CreateContainerGrid();
            ScrollViewer saveListScrollViewer = CreateListScrollViewer(3);
            SaveGameListGrid = CreateListGrid();
            Grid saveSearchGrid = CreateSearchGrid(SaveGameListGrid, 3);
            Grid nameGrid = CreateNameGrid(ref SaveGameCheckBox, SaveGameListGrid, 3);
            SaveGameNameTextBox = CreateNameTextBox(SaveGameListGrid);

            nameGrid.Children.Add(SaveGameNameTextBox);
            SaveGameContainerGrid.Children.Add(saveSearchGrid);
            SaveGameContainerGrid.Children.Add(saveListScrollViewer);
            saveListScrollViewer.Content = SaveGameListGrid;
            SaveGameContainerGrid.Children.Add(nameGrid);

            SaveGameCheckBox.IsChecked = false;

            return SaveGameContainerGrid;
        }

        public Game LoadGame() // Not tested might not work yet.
        {
            if (Choice == "" && ChoiceId == -1)
            {
                throw new Exception("Name or Id cannot be empty.");
            }

            string jsonStringGame = null;

            db.GetGame(ChoiceId, ref jsonStringGame, ref ChoiceId); // From now on ChoiceId holds id of the map.

            JObject jsonGame = JObject.Parse(jsonStringGame);

            SerializationHelper.JsonGetPlacedObjects(jsonGame, out var placedObjects);

            string gameName = Choice;

            Map map = LoadMap(out int mapId);

            Game game = new Game(ChoiceId, gameName, placedObjects, map);
            
            foreach (Placement placement in game.PlacedObjects)
            {
                GameObject gameObject = map.AvailableTiles
                    .First(t => t.TileObject.Name == placement.GameObject.TileObject.Name);
                
                placement.GameObject.Price = gameObject.Price;
                placement.GameObject.ChangeValue = gameObject.ChangeValue;
            }

            Choice = "";
            ChoiceId = -1;

            return game;
        }

        public Map LoadMap(out int mapID)
        {

            if (Choice == "")
            {
                throw new Exception("Name cannot be empty.");
            }

            string jsonStringMap = null;

            db.GetMapByID(ref jsonStringMap, ChoiceId);

            JObject jsonMap = JObject.Parse(jsonStringMap);

            int authorID = 0;
            int size = 0;
            int availableMoney = 0;
            string name = null;

            SerializationHelper.JsonGetBasicData(jsonMap, ref name, ref authorID, ref size, ref availableMoney);

            int[,] tileGrid = new int[size, size];
            bool[,] placeableGrid = new bool[size, size];

            Dictionary<int, string> indexer = new Dictionary<int, string>();

            SerializationHelper.JsonGetTileGridAndDictionary(jsonMap, indexer, tileGrid, placeableGrid);

            SerializationHelper.JsonGetAvailableTiles(jsonMap, out var availableTiles);

            SerializationHelper.JsonGetPlacedObjects(jsonMap, out var placedObjects);

            for (int i = 0; i < placedObjects.GetLength(0); i++)
            {
                Console.WriteLine(placedObjects[i].GameObject.TileObject.Name);
            }

            mapID = ChoiceId;

            Map map = new Map(Choice, tileGrid, placeableGrid, indexer, availableTiles, availableMoney, placedObjects);

            ChoiceId = -1;
            Choice = "";

            return map;
        }

        public void SaveGame(ITileBinder tileBinder, int mapID)
        {
            if (Choice == "")
            {
                throw new Exception("Name cannot be empty.");
            }

            JObject jsonGame = new JObject();
            SerializationHelper.JsonAddPlacements(jsonGame, tileBinder.PlacedObjects);

            int result = db.CheckGameNameOccupation(Choice, Credentials.UserID);

            if (result == 0)
            {
                result = db.addGame(Credentials.UserID, Choice, jsonGame.ToString(Formatting.None), mapID, Publish);
            }
            else
            {
                result = db.updateGame(Credentials.UserID, jsonGame.ToString(Formatting.None), Choice, mapID, Publish) ? 0 : -1;
            }

            Choice = "";
            ChoiceId = -1;
            SaveGameCheckBox.IsChecked = false;
            Publish = false;

            if (result != 0)
            {
                throw new Exception("Wystąpił problem przy zapisie gry.");
            }
        }

        public void SaveMap(ITileBinder tileBinder)
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

            int result;

            if (Publish)
            {
                result = db.CheckPublishedMapNameOccupation(Choice, Credentials.UserID);

                if (result == 0)
                {
                    result = db.DodajMape(Credentials.UserID, Choice, jsonMap.ToString(Formatting.None), true);
                }
            }
            else
            {
                result = db.CheckMapNameOccupation(Choice, Credentials.UserID);

                if (result == 0)
                {
                    result = db.DodajMape(Credentials.UserID, Choice, jsonMap.ToString(Formatting.None), false);
                }
                else if (result == 33) // Map name is occupied.
                {
                    result = db.updateMap(Credentials.UserID, jsonMap.ToString(Formatting.None), Choice) ? 0 : -1;
                }
            }
            
            Choice = "";
            ChoiceId = -1;
            SaveMapCheckBox.IsChecked = false;
            Publish = false;

            if (result != 0)
            {
                throw new Exception("Wystąpił problem przy zapisie mapy.");
            }
        }

        private Grid CreateContainerGrid()
        {
            return new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }

        private Grid CreateNameGrid(ref CheckBox publishCheckBox, Grid listGrid, int mode)
        {
            Grid nameGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF949494"),
                Height = 30,
            };

            Label nameLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF898989"),
                Margin = new Thickness(0, 0, 0, 0),
                Width = 60,
                Content = "Nazwa:"
            };

            Grid publishGrid = CreatePublishGrid(ref publishCheckBox, listGrid, mode);
            nameGrid.Children.Add(nameLabel);
            nameGrid.Children.Add(publishGrid);

            return nameGrid;
        }

        private TextBox CreateNameTextBox(Grid listGrid)
        {
            TextBox nameTextBox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(60, 0, 80, 0),
                Background = (Brush)new BrushConverter().ConvertFrom("#FF949494"),
                Padding = new Thickness(5, 0, 5, 0),
                BorderThickness = new Thickness(0, 0, 0, 0)
            };
            nameTextBox.TextChanged += (s, a) => { NameTextChanged(s, listGrid); };

            return nameTextBox;
        }

        private Grid CreatePublishGrid(ref CheckBox publishCheckBox, Grid listGrid, int mode)
        {
            Grid publishGrid =  new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 80,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF898989")
            };

            publishCheckBox = new CheckBox
            {
                Width = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
            };
            publishCheckBox.Click += (s, a) => { PublishCheckBoxClick(s, listGrid, mode); };

            Label publishLabel = new Label
            {
                Width = 60,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(20, 0, 0, 0),
                Content = "Publikuj"
            };

            publishGrid.Children.Add(publishCheckBox);
            publishGrid.Children.Add(publishLabel);

            return publishGrid;
        }

        private Grid CreateSearchGrid(Grid listGrid, int mode)
        {
            Grid searchGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF949494"),
                Height = 30,
            };

            TextBox searchTextBox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0, 0, 50, 0),
                Padding = new Thickness(5, 0, 5, 0),
                Background = (Brush)new BrushConverter().ConvertFrom("#FF949494"),
                BorderThickness = new Thickness(0, 0, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            searchTextBox.TextChanged += SearchTextChanged;

            Button searchButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF4A9C38"),
                Foreground = (Brush)new BrushConverter().ConvertFrom("#FF474747"),
                Width = 50,
                Content = "Szukaj"
            };
            searchButton.Foreground = Brushes.White;
            searchButton.FontFamily = new FontFamily("Segoe UI");
            searchButton.FontWeight = FontWeights.Bold;
            searchButton.Click += (s, a) => { SearchButtonClick(listGrid); };

            searchGrid.Children.Add(searchTextBox);
            searchGrid.Children.Add(searchButton);

            return searchGrid;
        }

        private ScrollViewer CreateListScrollViewer(int mode)
        {
            return new ScrollViewer
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = (mode == 2 || mode == 0) ? new Thickness(0, 30, 0, 0) : new Thickness(0, 30, 0, 30),
                Background = (Brush)new BrushConverter().ConvertFrom("#FFA7A7A7")
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

        public void PopulateSaveMapListGrid()
        {
            SaveMapNameTextBox.Text = "";
            Tuple<int,int,string, bool>[] items = db.getMapList();
            items = items.Where(i => i.Item4 == Publish).ToArray(); // Depending of Publish flag, when true get only public items.
            items = items.Where(i => i.Item2 == Credentials.UserID).ToArray();
            string[] names = items.Select(i => i.Item3).ToArray();
            var itemList = items.Zip(names, (i, n) => new { Item = i, Name = n });

            LastPopulated = itemList;

            PopulateListGrid(SaveMapListGrid, itemList);
        }

        public void PopulateEditLoadMapListGrid()
        {
            Tuple<int, int, string, bool>[] items = db.getMapList();
            items = items.Where(i => i.Item4 == false).ToArray(); // Only non public.
            items = items.Where(i => i.Item2 == Credentials.UserID).ToArray();
            string[] names = items.Select(i => i.Item3).ToArray();
            var itemList = items.Zip(names, (i, n) => new { Item = i, Name = n });

            LastPopulated = itemList;

            PopulateListGrid(LoadMapListGrid, itemList);
        }

        public void PopulatePlayLoadMapListGrid()
        {
            Tuple<int, int, string, bool>[] items = db.getMapList();
            items = items.Where(i => i.Item4 == true).ToArray(); // Only public.
            string[] names = items.Select(i => ((i.Item2 == Credentials.UserID) ? "(Ty) " : "(" + i.Item2 + ") ") + i.Item3).ToArray();
            var itemList = items.Zip(names, (i, n) => new { Item = i, Name = n });

            LastPopulated = itemList;

            PopulateListGrid(LoadMapListGrid, itemList);
        }

        public void PopulateUserLoadGameListGrid()
        {
            Tuple<int, int, string, bool>[] items = db.getGameList();
            items = items.Where(i => i.Item2 == Credentials.UserID).ToArray();
            string[] names = items.Select(i => i.Item3).ToArray();
            var itemList = items.Zip(names, (i, n) => new { Item = i, Name = n });

            LastPopulated = itemList;

            PopulateListGrid(LoadGameListGrid, itemList);
        }

        public void PopulateObserverLoadGameListGrid()
        {
            Tuple<int, int, string, bool>[] items = db.getGameList();
            items = items.Where(i => i.Item4 == true).ToArray(); // Only public.
            string[] names = items.Select(i => ((i.Item2 == Credentials.UserID) ? "(Ty) " : "(" + i.Item2 + ") ") + i.Item3).ToArray();
            var itemList = items.Zip(names, (i, n) => new { Item = i, Name = n });

            LastPopulated = itemList;

            PopulateListGrid(LoadGameListGrid, itemList);
        }

        public void PopulateSaveGameListGrid()
        {
            SaveGameNameTextBox.Text = "";
            Tuple<int, int, string, bool>[] items = db.getGameList();
            items = items.Where(i => i.Item2 == Credentials.UserID).ToArray();
            string[] names = items.Select(i => i.Item3).ToArray();
            var itemList = items.Zip(names, (i, n) => new { Item = i, Name = n });

            LastPopulated = itemList;

            PopulateListGrid(SaveGameListGrid, itemList);
        }

        private void PopulateListGrid(Grid listGrid, IEnumerable<dynamic> itemList)
        {
            listGrid.RowDefinitions.Clear();
            listGrid.Children.Clear();

            int index = 0;

            foreach (var item in itemList)
            {

                listGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                Grid listItemGrid = new Grid { HorizontalAlignment = HorizontalAlignment.Stretch };
                Grid.SetRow(listItemGrid, index);
                Button listItemButton = CreateListItemButton(item.Name, listGrid);

                if (Publish && item.Item.Item4)
                {
                    listItemButton.IsEnabled = false;
                    listItemButton.Foreground = Brushes.Gray;
                }

                listItemButton.Tag = item.Item;
                listItemGrid.Children.Add(listItemButton);
                listGrid.Children.Add(listItemGrid);
                index++;
            }
        }

        private Button CreateListItemButton(string name, Grid listGrid)
        {
            TextBlock nameTextBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                FontFamily = new FontFamily("Segoe UI"),
                FontWeight = FontWeights.Bold
            };

            nameTextBlock.Text = name;

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
                Content = nameTextBlock, // Has to be a TextBlock, otherwise cuts out some special characters.
            };
            listItemButton.Click += (s,a) => { ListItemButtonClick(s,listGrid); };
            listItemButton.ClickMode = ClickMode.Press;

            return listItemButton;
        }

        // Event handlers

        private void PublishCheckBoxClick(object s, Grid listGrid, int mode) 
        {
            CheckBox sender = (CheckBox)s;
            Publish = (bool)sender.IsChecked;

            if (mode == 1)
            {
                PopulateSaveMapListGrid();
            }
        }

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

            Tuple<int, int, string, bool> IdName = (Tuple<int, int,string, bool>)sender.Tag;

            ChoiceId = IdName.Item1;
            Choice = IdName.Item3;
            
            Console.WriteLine(Choice + " " + ChoiceId);

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
                    Tuple<int, int, string, bool> item = (Tuple<int, int, string, bool>)listItemButton.Tag;

                    listItemButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF878787");

                    if (Publish && item.Item4)
                    {
                        listItemButton.IsEnabled = false;
                        listItemButton.Foreground = Brushes.Gray;
                    }
                    else
                    {
                        listItemButton.Foreground = Brushes.White;
                    }
                }
            }
            else
            {
                ClearSelection = true;
            }
        }

        private void SaveMapNameTextChanged(object s)
        {
            TextBox sender = (TextBox)s;
            Choice = sender.Text;
        }

        private void SearchButtonClick(Grid listGrid)
        {
            Choice = "";
            ChoiceId = -1;

            if (LastPopulated != null)
            {
                IEnumerable<dynamic> itemList = LastPopulated.Where(i => i.Item.Item3.Contains(Keyword));
                PopulateListGrid(listGrid, itemList);
            }
        }
    }
}
