using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Admin;
using baUHInia.Authorisation;
using baUHInia.MapLogic.Manager;
using baUHInia.MapLogic.Model;
using baUHInia.Playground.Logic.Creators;
using baUHInia.Playground.Logic.Creators.Selector;
using baUHInia.Playground.Logic.Creators.Tiles;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;
using baUHInia.Simulation;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace baUHInia.Playground.View
{
    public partial class AdminGameWindow : ITileBinder
    {
        private const byte BoardDensity = 50;

        private ISelectorGridCreator _selectorGridCreator;
        private IGameGridCreator _gameGridCreator;
        private IAdminSelectorTabCreator _admin;
        private IGameMapManager _manager;

        private Dictionary<string, Grid> Grids { get; set; }
        private UserGameWindow UserWindow { get; set; }

        public AdminGameWindow(LoginData credentials)
        {
            InitializeComponent();
            InitializeProperties(credentials);
            InitializeSwitches();
            InitializeInteractionChangers();
            AdjustWindowSizeAndPosition();
            InitializeGrids();
        }

        //========================= INTERFACE IMPLEMENTATIONS ========================//

        public Selection Selection { get; private set; }
        public Tile[,] TileGrid { get; private set; }
        public List<Placement> PlacedObjects { get; private set; }
        public ScrollViewer GameViewer => GameScroll;
        public Grid SelectorGrid => AdminSelectorGrid;
        public List<GameObject> AvailableObjects { get; private set; }
        public LoginData Credentials { get; private set; }

        public bool IsInAdminMode { get; } = true;
        public int AvailableFounds { get; set; }

        private int MapId { get; set; }
        private bool Observing { get; set; }

        //============================ PREDEFINED ACTIONS ============================//

        private void InitializeGrids()
        {
            Grids = new Dictionary<string, Grid>
            {
                {"Menu", GameScroll.Content as Grid},
                {"Admin", CreateAdminTab()} 
            };
            CreateGameTab();
            CreateLoadMapTab();
            CreateSaveMapTab();
            CreateObserverTab();
        }

        public void ChangeInteractionMode(string text, Brush color)
        {
            string[] strings = text.Split('/');
            if (strings.Length < 4) return;

            (ModeText.Text, FirstTip.Text, SecondTip.Text, ThirdTip.Text) =
                (strings[0], strings[1], strings[2], strings[3]);

            (ModeText.Foreground, FirstTip.Foreground, SecondTip.Foreground, ThirdTip.Foreground) =
                (color, color, color, color);
        }

        private void AdjustWindowSizeAndPosition()
        {
            WindowState = WindowState.Maximized;
            Window.MaxHeight = SystemParameters.WorkArea.Height + 14.0;
        }

        private void InitializeInteractionChangers()
        {
            DeleteButton.Click += (o, args) => Selection.ChangeState(State.Remove);
            PlaceableButton.Click += (o, args) => Selection.ChangeState(State.Block);
        }

        private void InitializeSwitches()
        {
            TerrainSwitch.Click += (o, args) => UpdateSelectorComboBox(ResourceType.Terrain);
            StructureSwitch.Click += (o, args) => UpdateSelectorComboBox(ResourceType.Structure);
            FoliageSwitch.Click += (o, args) => UpdateSelectorComboBox(ResourceType.Foliage);
        }

        private void ChangeDropdownSelection(object sender, SelectionChangedEventArgs e)
        {
            TileCategory category = !(CategorySelector.SelectedItem is string item)
                ? ResourceHolder.Get.GetFirstTileCategory()
                : ResourceHolder.Get.GetCategoryByName(item);
            _selectorGridCreator.CreateSelectionPanel(category, this);
        }

        private void UnlockReturnToMapButton()
        {
            NewGameTitle.Text = "MAPA";
            ReturnToGameButton.IsHitTestVisible = true;
            ReturnToGameButton.Style = FindResource("MenuButton") as Style;
            ReturnToGameButton.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#DDDDDD");
        }

        //============================== INITIAL WINDOW ==============================//

        private void InitializeProperties(LoginData credentials)
        {
            Credentials = credentials;
            PlacedObjects = new List<Placement>();
            AvailableObjects = new List<GameObject>();
            _manager = new GameMapManager(Credentials);
            _admin = new AdminRestrictionsWindow(this);

            AccountName.Text += "\t\t" + credentials.name;
            AccountType.Text += "\t" + (credentials.isAdmin ? "WŁADZE MIASTA" : "MIESZKANIEC");
            Mode.Text += "\t\t" + "WŁADZ MIASTA";
        }

        //=============================== FUNCTIONALITY ==============================//

        private void CreateNewMap(object sender, RoutedEventArgs args)
        {
            ClearMap();
            UpdateSelectorComboBox(ResourceType.Terrain);
            AvailableObjects.Clear();
            _admin = new AdminRestrictionsWindow(this);
            Grids["Admin"] = CreateAdminTab();
            ShowGameBoard();
        }

        private void ShowGameBoard()
        {
            if (!Observing) UnlockReturnToMapButton();
            OpenGameTab(null, null);
        }

        private void UpdateSelectorComboBox(ResourceType type)
        {
            ResourceHolder.Get.ChangeResourceType(type);
            List<TileCategory> categories = ResourceHolder.Get.GetSelectedCategories();
            TileCategory category = ResourceHolder.Get.GetFirstTileCategory();

            IEnumerable<string> strings = ResourceHolder.Get.GetCategoryName();
            CategorySelector.ItemsSource = strings;
            CategorySelector.SelectedIndex = 0;
            _selectorGridCreator.UpdateTileGroup(categories);
            _selectorGridCreator.CreateSelectionPanel(category, this);
        }

        private void ClearMap()
        {
            int count = BoardDensity * BoardDensity * 2;
            Grids["GameMap"].Children.RemoveRange(count, Grids["GameMap"].Children.Count - count);
            PlacedObjects.Clear();
            Selection.Reset();
            string str = "Plain Grass";
            TileObject grass = ResourceHolder.Get.GetTerrainTileObject(str);
            foreach (Tile tile in TileGrid)
            {
                Button button = tile.GetUiElement() as Button;
                if (tile.GetTextureName() != "grass.png") tile.Change(grass[0], str);
                if (!button.IsHitTestVisible) button.IsHitTestVisible = true;
                tile.Placeable = true;
            }
        }

        private void LoadMap(object sender, RoutedEventArgs args)
        {
            Map map;
            try
            {
                map = _manager.LoadMap(out int mapId);
                MapId = mapId;
            }
            catch (Exception) { return; }
            
            ClearMap();
            ShowGameBoard();
            UpdateSelectorComboBox(ResourceType.Terrain);
            AvailableObjects = _gameGridCreator.LoadMapIntoTheGameGrid(this, map);
            _admin = new AdminRestrictionsWindow(this);
            Grids["Admin"] = CreateAdminTab();
            SideGrid.Visibility = Visibility.Visible;
        }

        private void SaveMap(object sender, RoutedEventArgs args)
        {
            try { _manager.SaveMap(this); }
            catch (Exception) { return; }

            SideGrid.Visibility = Visibility.Visible;
            GameScroll.Content = Grids["GameMap"];
        }

        //===================================// OBSERVER BEHAVIOUR //==================================//

        private void ObserveGame(object source, RoutedEventArgs args)
        {
            Game game;
            try { game = _manager.LoadGame(); }
            catch (Exception) { return; }

            Observing = true;
            ClearMap();

            _gameGridCreator.LoadMapIntoTheGameGrid(this, game.Map);
            _gameGridCreator.LoadGameIntoTheGameGrid(this, game);
            
            OpenObservedGameTab();
            DisplayGameProperties(game);
            CalculatePoints(game.Map.PlacedObjects.Length);
            RemoveButtonsBehaviour();
        }

        private void ReturnToObservableList(object sender, RoutedEventArgs args)
        {
            ReturnToGameButton.IsHitTestVisible = false;
            ReturnToGameButton.Style = FindResource("MenuButtonNotClickable") as Style;
            Bar.Visibility = Visibility.Collapsed;
            GameScroll.Content = Grids["Observer"];
            _manager.PopulateObserverLoadGameListGrid();
            Observing = false;
        }
        
        private void DisplayGameProperties(Game game)
        {
            int currentCash = game.Map.AvailableMoney - game.PlacedObjects.Sum(o => o.GameObject.Price);
            AllCash.Text = game.Map.AvailableMoney.ToString();
            CurrentCash.Text = currentCash.ToString();
            Grids["GameMap"] = GameScroll.Content as Grid;
            GameName.Text = game.Name;
        }

        private void CalculatePoints(int mapPlacersCount)
        {
            ISimulate simulate = new Score(this, BoardDensity);
            //List<Placement> allPlacements = PlacedObjects;
            //int newCount = PlacedObjects.Count - mapPlacersCount;

            //PlacedObjects = PlacedObjects.GetRange(mapPlacersCount, newCount);
            Points.Text = simulate.SimulationScore().ToString();
            //PlacedObjects = allPlacements;
        }

        private void RemoveButtonsBehaviour()
        {
            foreach (Tile tile in TileGrid)
            {
                Button button = tile.GetUiElement() as Button;
                button.IsHitTestVisible = false;
            }
        }

        //======================================// GRID SWITCH //======================================//

        private void OpenGameTab(object source, RoutedEventArgs args)
        {
            SideGrid.Visibility = Visibility.Visible;
            GameScroll.Content = Grids["GameMap"];
        }
        
        private void OpenAdminTab(object source, RoutedEventArgs args)
        {
            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = Grids["Admin"];
        }

        private void OpenLoadMapTab(object source, RoutedEventArgs args)
        {
            _manager.PopulateEditLoadMapListGrid();
            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = Grids["LoadMap"];
        }

        private void OpenSaveMapTab()
        {
            _manager.PopulateSaveMapListGrid();
            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = Grids["SaveMap"];
        }

        private void OpenObserverTab(object source, RoutedEventArgs args)
        {
            _manager.PopulateObserverLoadGameListGrid();
            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = Grids["Observer"];
        }

        private void OpenObservedGameTab()
        {
            SideGrid.Visibility = Visibility.Collapsed;
            Bar.Visibility = Visibility.Visible;
            GameScroll.Content = Grids["GameMap"];
        }

        //=====================================// GRID CREATION //=====================================//

        private void CreateGameTab()
        {
            TileObject dirt = ResourceHolder.Get.GetTerrainTileObject("Plain Dirt");
            Selection = new Selection(dirt, this);
            
            TileObject grass = ResourceHolder.Get.GetTerrainTileObject("Plain Grass");
            TileGrid = new Tile[BoardDensity, BoardDensity];
            
            List<TileCategory> categories = ResourceHolder.Get.GetSelectedCategories();
            _selectorGridCreator = new AdminSelectorGridCreator(this, categories);
            
            _gameGridCreator = new PlacerGridCreator(this, BoardDensity, grass);
            Grids["GameMap"] = _gameGridCreator.CreateElementsInWindow(this, BoardDensity); 
        }
        
        private Grid CreateAdminTab()
        {
            _admin.GetReturnButton().Click += (sender, eventArgs) =>
            {
                AvailableObjects = _admin.GetModifiedAvailableObjects();
                AvailableFounds = _admin.GetBudget();
                GameScroll.Content = Grids["GameMap"];
                SideGrid.Visibility = Visibility.Visible;
            };
            _admin.GetApplyButton().Click += (sender, eventArgs) =>
            {
                AvailableObjects = _admin.GetModifiedAvailableObjects();
                AvailableFounds = _admin.GetBudget();
                if (AvailableObjects.Count < 3) return;
                _manager.PopulateSaveMapListGrid();
                OpenSaveMapTab();
            };
            Grid grid = _admin.GetAdminSelectorTableGrid();
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            return grid;
        }

        private void CreateSaveMapTab()
        {
            Grids.Add("SaveMap", Resources["SaveMapTemplate"] as Grid);
            Border border = Grids["SaveMap"].Children[0] as Border;
            Grid innerGrid = border.Child as Grid;
            ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetMapSaveGrid());
            ((Button) innerGrid.Children[3]).Click += (sender, arg) => GameScroll.Content = Grids["Admin"];
        }

        private void CreateLoadMapTab()
        {
            Grids.Add("LoadMap", Resources["LoadMapTemplate"] as Grid);
            Border border = Grids["LoadMap"].Children[0] as Border;
            Grid innerGrid = border.Child as Grid;
            ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetMapLoadGrid());
            ((Button) innerGrid.Children[3]).Click += (sender, arg) => GameScroll.Content = Grids["Menu"];
        }

        private void CreateObserverTab()
        {
            Grids.Add("Observer", Resources["LoadObserverTemplate"] as Grid);
            Border border = Grids["Observer"].Children[0] as Border;
            Grid innerGrid = border.Child as Grid;
            ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetGameLoadGrid());
            ((Button) innerGrid.Children[3]).Click += (sender, arg) => { GameScroll.Content = Grids["Menu"]; };
        }

        //=====================================// OTHER WINDOWS //=====================================//

        private void ShowStatistics(object sender, RoutedEventArgs args)
        {
            Statistics.Statistics statistics = new Statistics.Statistics();
            statistics.Show();
        }

        private void ChangeGameMode(object sender, RoutedEventArgs args)
        {
            UserWindow = UserWindow ?? new UserGameWindow(LoginData.GetInstance()) {Owner = this};
            UserWindow.Show();
            Hide();
        }

        private void ReturnToMenu(object sender, RoutedEventArgs args)
        {
            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = Grids["Menu"];
        }

        private void ExitApplication(object sender, RoutedEventArgs args) => Application.Current.Shutdown();
    }
}