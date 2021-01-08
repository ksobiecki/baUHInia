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
        private int ChoiceId;
        private string Keyword;
        private bool ClearSelection = true;

        // Modes - 0: MapLoad, 1: MapSave 2: GameLoad, 3: GameSave.

        private Tuple<int, string>[] MapNames;
        private Tuple<int,string>[] GameIDsNames;


        // Layout
        private Grid LoadMapContainerGrid;
        private Grid SaveMapContainerGrid;
        private Grid LoadGameContainerGrid;
        private Grid SaveGameContainerGrid;

        private TextBox SaveMapNameTextBox;
        private TextBox SaveGameNameTextBox;

        private Label SaveMapErrorLabel;

        public GameMapManager()
        {
            Choice = "";
            ChoiceId = -1;
            Keyword = "";

            db = new BazaDanych();
        }

        public Grid GetMapLoadGrid(int userID)
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

        public Grid GetMapSaveGrid(int userID)
        {
            Choice = "";
            Keyword = "";

            MapNames = db.getMapNames();

            SaveMapContainerGrid = CreateContainerGrid();

            BrushConverter bc = new BrushConverter();
            SaveMapContainerGrid.Background = (Brush)bc.ConvertFrom("#4466AA");

            SaveMapErrorLabel = new Label
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Height = 40,
                Foreground = Brushes.Red
            };

            TextBox nameTextBox = new TextBox
            {
                Margin = new Thickness(20, 0, 20, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(0,5,0,5)
            };

            nameTextBox.TextChanged += (s, a) => { SaveMapNameTextChanged(s); };

            SaveMapContainerGrid.Children.Add(SaveMapErrorLabel);
            SaveMapContainerGrid.Children.Add(nameTextBox);
            return SaveMapContainerGrid;

            /*ScrollViewer saveListScrollViewer = CreateListScrollViewer();
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

            return SaveMapContainerGrid;*/
        }

        public Grid GetGameLoadGrid(int userID)
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


        public Grid GetGameSaveGrid(int userID)
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

        public Game LoadGame() // Not tested might not work yet.
        {
            if (Choice == "" || ChoiceId == -1)
            {
                throw new Exception("Name or Id cannot be empty.");
            }

            string jsonStringGame = null;

            db.GetGame(ChoiceId, ref jsonStringGame, ref ChoiceId); // From now on ChoiceId holds id of the map.
            
            JObject jsonGame = JObject.Parse(jsonStringGame);

            SerializationHelper.JsonGetPlacedObjects(jsonGame, out var placedObjects);

            string gameName = Choice;

            Map map = LoadMap(out int mapID);

            Choice = "";
            ChoiceId = -1;

            return new Game(ChoiceId, Choice, placedObjects, map);
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
                Console.WriteLine(((Placement)placedObjects[i]).GameObject.TileObject.Name);
            }

            mapID = ChoiceId;

            Choice = "";
            ChoiceId = -1;

            return new Map(Choice, tileGrid, placeableGrid, indexer, availableTiles, availableMoney, placedObjects);
        }

        public bool SaveGame(ITileBinder tileBinder, int mapID)
        {

            if (Choice == "" || ChoiceId == -1)
            {
                throw new Exception("Name or Id cannot be empty.");
            }

            JObject jsonGame = new JObject();
            SerializationHelper.JsonAddPlacements(jsonGame, tileBinder.PlacedObjects);

            bool result = db.addGame(tileBinder.Credentials.UserID, Choice, jsonGame.ToString(Formatting.None), mapID);

            Choice = "";
            ChoiceId = -1;

            return result;
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
            int result = db.DodajMape(tileBinder.Credentials.UserID, Choice, jsonMap.ToString(Formatting.None));

            Console.WriteLine("Saving result: " + result);

            if (result != 0) // Result equal to 0 is a successful write.
            {
                if (result != 33) // 33 - Map already exists, other code means diffrent issue.
                {
                    SaveMapErrorLabel.Content = "Nie udało się zapisać mapy.";
                    return false;
                }
                else // Checking for ownership.
                {
                    if (db.CheckIfTheLoggedInUserIsTheOwnerOfTheMapHeOrSheWantToOverwrite(tileBinder.Credentials.UserID, ChoiceId) != 1) // Result equal to 1 means user is the owner.
                    {
                        SaveMapErrorLabel.Content = "Nazwa mapy jest zajęta.";
                        return false;
                    }

                    bool result2 = db.updateMap(tileBinder.Credentials.UserID, jsonMap.ToString(Formatting.None), Choice); // Returns true even when 0 rows were affected, TODO make sure this works when db fixes their error detection.

                    if (!result2)
                    {
                        SaveMapErrorLabel.Content = "Nie udało się zapisać mapy.";
                        return false;
                    }
                }
            }

            // TODO FIND OUT WHY NAMES WITH POLISH LETTERS DO NOT GET SAVED PROPERLY (the special letters turn into normal ones - this also allows to save maps with names that are already occupied) 

            SaveMapErrorLabel.Content = "";
            MapNames = db.getMapNames();

            Choice = "";
            ChoiceId = -1;

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
                Tuple<int, string>[] filteredMapNames = MapNames.Where(m => m.Item2.Contains(Keyword)).ToArray();

                foreach (Tuple<int, string> mapIDNname in filteredMapNames)
                {
                    listGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    Grid listItemGrid = new Grid {HorizontalAlignment = HorizontalAlignment.Stretch};
                    Grid.SetRow(listItemGrid, index);
                    Button listItemButton = CreateListItemButton(mapIDNname.Item2, listGrid);
                    Tuple<int, string> temp = new Tuple<int, string>(0, mapIDNname.Item2);
                    listItemButton.Tag = mapIDNname;
                    listItemGrid.Children.Add(listItemButton);
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
                    listItemButton.Tag = gameIDName;
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

            Tuple<int, string> IdName = (Tuple<int,string>)sender.Tag;

            ChoiceId = IdName.Item1;
            Choice = IdName.Item2;
            
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
                    listItemButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF878787");
                    listItemButton.Foreground = Brushes.White;
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

        private void SearchButtonClick(Grid listGrid, int mode)
        {
            Choice = "";
            ChoiceId = -1;
            PopulateListGrid(listGrid,mode);
        }
        
    }
}
