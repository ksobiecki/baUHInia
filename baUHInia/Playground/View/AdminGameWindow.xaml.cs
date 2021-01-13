using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        private Grid MenuGrid { get; set; }
        private Grid AdminGrid { get; set; }
        private Grid SaveMapGrid { get; set; }
        private Grid LoadMapGrid { get; set; }
        private Grid LoadObserverGrid { get; set; }
        private Grid GameMapGrid { get; set; }

        private UserGameWindow UserWindow { get; set; }

        public AdminGameWindow(LoginData credentials)
        {
            InitializeComponent();
            StoreMenuGrid();
            InitializeProperties(credentials);
            AddLoadCardAndInitializeManager();
            AdjustWindowSizeAndPosition();
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
        private bool Observing { get; set; } = false;

        //============================ PREDEFINED ACTIONS ============================//

        public void ChangeInteractionMode(string text, Brush color)
        {
            string[] strings = text.Split('/');
            if (strings.Length < 4) return;

            (ModeText.Text, FirstTip.Text, SecondTip.Text, ThirdTip.Text) =
                (strings[0], strings[1], strings[2], strings[3]);

            (ModeText.Foreground, FirstTip.Foreground, SecondTip.Foreground, ThirdTip.Foreground) =
                (color, color, color, color);
        }

        private void StoreMenuGrid() => MenuGrid = GameScroll.Content as Grid;

        private void BackToGame(object sender, RoutedEventArgs args)
        {
            SideGrid.Visibility = Visibility.Visible;
            GameScroll.Content = GameMapGrid;
        }

        private void InitializeSelection()
        {
            TileObject tileObject = ResourceHolder.Get.GetTerrainTileObject("Plain Dirt");
            Selection = new Selection(tileObject, this);
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

        private void CreateGameBoard()
        {
            NewGameTitle.Text = "MAPA";
            if (!Observing)
            {
                ReturnToGameButton.IsHitTestVisible = true;
                ReturnToGameButton.Click += BackToGame;
                ReturnToGameButton.Style = FindResource("MenuButton") as Style;
                ReturnToGameButton.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#DDDDDD");
            }

            SideGrid.Visibility = Visibility.Visible;
            TileGrid = new Tile[BoardDensity, BoardDensity];
            TileObject tileObject = ResourceHolder.Get.GetTerrainTileObject("Plain Grass");
            _gameGridCreator = new PlacerGridCreator(this, BoardDensity, tileObject);
            _gameGridCreator.CreateElementsInWindow(this, BoardDensity);
            GameMapGrid = GameScroll.Content as Grid;
            GameMapGrid.Visibility = Visibility.Visible;
        }

        private void CreateSelectorGrid()
        {
            List<TileCategory> categories = ResourceHolder.Get.GetSelectedCategories();
            _selectorGridCreator = new AdminSelectorGridCreator(this, categories);
        }

        //============================== INITIAL WINDOW ==============================//

        private void AddLoadCardAndInitializeManager()
        {
            _manager = new GameMapManager(Credentials);
            //TODO:
            //InitialMapGrid.Children.Add(_manager.GetMapLoadGrid());
        }

        private void InitializeProperties(LoginData credentials)
        {
            Credentials = credentials;
            PlacedObjects = new List<Placement>();
            AvailableObjects = new List<GameObject>();
            _admin = new AdminRestrictionsWindow(this);

            AccountName.Text += "\t\t" + credentials.name;
            AccountType.Text += "\t" + (credentials.isAdmin ? "WŁADZE MIASTA" : "MIESZKANIEC");
            Mode.Text += "\t\t" + "WŁADZ MIASTA";
        }

        //=============================== FUNCTIONALITY ==============================//

        private void CreateNewMap(object sender, RoutedEventArgs e)
        {
            InitializeSelection();
            CreateGameBoard();
            CreateSelectorGrid();
            InitializeSwitches();
            InitializeInteractionChangers();
            UpdateSelectorComboBox(ResourceType.Terrain);
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

        private void CreateLoadWindow(object source, RoutedEventArgs args)
        {
            if (LoadMapGrid == null)
            {
                LoadMapGrid = Resources["LoadMapTemplate"] as Grid;
                Border border = LoadMapGrid.Children[0] as Border;
                Grid innerGrid = border.Child as Grid;
                ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetMapLoadGrid());
                ((Button) innerGrid.Children[3]).Click += (sender, arg) =>
                {
                    GameScroll.Content = GameMapGrid ?? MenuGrid;
                    SideGrid.Visibility = GameMapGrid == null ? Visibility.Collapsed : Visibility.Visible;
                };
            }

            _manager.PopulateEditLoadMapListGrid();
            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = LoadMapGrid;
        }

        private void CreateLoadObserverWindow(object source, RoutedEventArgs args)
        {
            if (LoadObserverGrid == null)
            {
                LoadObserverGrid = Resources["LoadObserverTemplate"] as Grid;
                Border border = LoadObserverGrid.Children[0] as Border;
                Grid innerGrid = border.Child as Grid;
                ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetGameLoadGrid());
                ((Button) innerGrid.Children[3]).Click += (sender, arg) => { GameScroll.Content = MenuGrid; };
            }

            _manager.PopulateObserverLoadGameListGrid();
            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = LoadObserverGrid;
        }

        private void ShowObserver(object source, RoutedEventArgs args)
        {
            Game game;
            PlacedObjects.Clear();
            try { game = _manager.LoadGame(); }
            catch (Exception) { return; }

            Observing = true;
            if (GameMapGrid == null) CreateNewMap(null, null);
            GameMapGrid.Children.Clear();

            _gameGridCreator.LoadMapIntoTheGameGrid(this, game.Map);
            _gameGridCreator.LoadGameIntoTheGameGrid(this, game);

            SideGrid.Visibility = Visibility.Collapsed;
            GameMapGrid.Visibility = Visibility.Visible;
            Bar.Visibility = Visibility.Visible;

            int currentCash = game.Map.AvailableMoney - game.PlacedObjects.Sum(o => o.GameObject.Price);
            AllCash.Text = game.Map.AvailableMoney.ToString();
            CurrentCash.Text = currentCash.ToString();
            GameName.Text = game.Name;
            GameMapGrid = GameScroll.Content as Grid;
            
            ISimulate simulate = new Score(this, BoardDensity);
            List<Placement> allPlacements = PlacedObjects;
            int newCount = PlacedObjects.Count - game.Map.PlacedObjects.Length;
            PlacedObjects = PlacedObjects.GetRange(game.Map.PlacedObjects.Length, newCount); 
            Points.Text = simulate.SimulationScore().ToString();
            PlacedObjects = allPlacements;

            foreach (Tile tile in TileGrid)
            {
                Button button = tile.GetUiElement() as Button;
                button.IsHitTestVisible = false;
            }
        }

        private void ReturnToObservableList(object sender, RoutedEventArgs args)
        {
            ReturnToGameButton.IsHitTestVisible = false;
            ReturnToGameButton.Style = FindResource("MenuButtonNotClickable") as Style;
            Bar.Visibility = Visibility.Collapsed;
            GameMapGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = LoadObserverGrid;
            _manager.PopulateObserverLoadGameListGrid();
            PlacedObjects.Clear();
            GameMapGrid = null;
            Observing = false;
        }

        private void CreateSaveWindow(object source, RoutedEventArgs args)
        {
            if (SaveMapGrid == null)
            {
                SaveMapGrid = Resources["SaveMapTemplate"] as Grid;
                Border border = SaveMapGrid.Children[0] as Border;
                Grid innerGrid = border.Child as Grid;
                ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetMapSaveGrid());
                ((Button) innerGrid.Children[3]).Click += (sender, arg) => GameScroll.Content = AdminGrid;
            }

            _manager.PopulateSaveMapListGrid();
            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = SaveMapGrid;
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
            
            if (GameMapGrid == null) CreateNewMap(null, null);
            GameMapGrid.Children.Clear();
            AvailableObjects = _gameGridCreator.LoadMapIntoTheGameGrid(this, map);
            GameMapGrid = GameScroll.Content as Grid;
            SideGrid.Visibility = Visibility.Visible;
            _admin = new AdminRestrictionsWindow(this);
            Console.WriteLine("Passed loading");
        }

        private void SaveMap(object sender, RoutedEventArgs args)
        {
            try { _manager.SaveMap(this); }
            catch (Exception) { return; }
            SideGrid.Visibility = Visibility.Visible;
            GameScroll.Content = GameMapGrid;
        }

        private void NewMap(object source, RoutedEventArgs args)
        {
            CreateGameBoard();
            Selection.Reset();
            PlacedObjects.Clear();
        }

        private void OpenSelectorTab(object source, RoutedEventArgs args)
        {
            SideGrid.Visibility = Visibility.Collapsed;
            if (AdminGrid == null)
            {
                _admin.GetReturnButton().Click += (sender, eventArgs) =>
                {
                    AvailableObjects = _admin.GetModifiedAvailableObjects();
                    AvailableFounds = _admin.GetBudget();
                    GameScroll.Content = GameMapGrid;
                    SideGrid.Visibility = Visibility.Visible;
                };
                _admin.GetApplyButton().Click += (sender, eventArgs) =>
                {
                    AvailableObjects = _admin.GetModifiedAvailableObjects();
                    AvailableFounds = _admin.GetBudget();
                    if (AvailableObjects.Count < 3) return;
                    CreateSaveWindow(null, null);

                    _manager.PopulateSaveMapListGrid();
                };
                AdminGrid = _admin.GetAdminSelectorTableGrid();
                AdminGrid.VerticalAlignment = VerticalAlignment.Center;
                AdminGrid.HorizontalAlignment = HorizontalAlignment.Center;
                GameScroll.Content = AdminGrid;
            }
            else GameScroll.Content = AdminGrid;
        }

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
            GameScroll.Content = MenuGrid;
        }

        private void ReturnToLoginWindow(object sender, RoutedEventArgs args)
        {
            Authorisation.Authorisation authorisation = new Authorisation.Authorisation();
            authorisation.Show();
            Close();
        }

        private void Window_Closed(object sender, EventArgs e) { }
    }
}