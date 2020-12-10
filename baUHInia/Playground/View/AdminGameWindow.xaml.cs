using System;
using System.Collections.Generic;
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
using Application = System.Windows.Application;
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

        private Grid GameMapGrid { get; set; }

        public AdminGameWindow(LoginData credentials)
        {
            InitializeComponent();
            StoreMenuGrid();
            AddLoadCardAndInitializeManager();
            AdjustWindowSizeAndPosition();
            InitializeProperties(credentials);
        }

        //========================= INTERFACE IMPLEMENTATIONS ========================//

        public Selection Selection { get; private set; }
        public Tile[,] TileGrid { get; private set; }
        public List<Placement> PlacedObjects { get; private set; }
        public ScrollViewer GameViewer => GameScroll;
        public Grid SelectorGrid => AdminSelectorGrid;
        public List<GameObject> AvailableObjects { get; private set; }
        public LoginData Credentials { get; private set; }
        public int AvailableFounds { get; set; }

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

        private void BackToGame()
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
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
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
            NewGameTitle.Text = "POWRÓT DO PLANSZY";
            NewGameButton.Content = "POWRÓĆ";
            SideGrid.Visibility = Visibility.Visible;
            TileGrid = new Tile[BoardDensity, BoardDensity];
            TileObject tileObject = ResourceHolder.Get.GetTerrainTileObject("Plain Grass");
            _gameGridCreator = new PlacerGridCreator(this, BoardDensity, tileObject);
            _gameGridCreator.CreateGameGridInWindow(this, BoardDensity);
            GameMapGrid = GameScroll.Content as Grid;
            //CurrentGrid = GameMapGrid;
        }

        private void CreateSelectorGrid()
        {
            List<TileCategory> categories = ResourceHolder.Get.GetSelectedCategories();
            _selectorGridCreator = new AdminSelectorGridCreator(this, categories);
        }

        //============================== INITIAL WINDOW ==============================//

        private void AddLoadCardAndInitializeManager()
        {
            _manager = new GameMapManager();
            //TODO:
            //InitialMapGrid.Children.Add(_manager.GetMapLoadGrid());
        }

        private void InitializeProperties(LoginData credentials)
        {
            Credentials = credentials;
            PlacedObjects = new List<Placement>();
            AvailableObjects = new List<GameObject>();
            _admin = new AdminRestrictionsWindow(this);
        }

        //=============================== FUNCTIONALITY ==============================//

        private void CreateNewMap(object sender, RoutedEventArgs e)
        {
            if (GameMapGrid != null)
            {
                BackToGame();
                return;
            }

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

            CategorySelector.ItemsSource = ResourceHolder.Get.GetCategoryName();
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

            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = LoadMapGrid;
        }
        
        private void CreateSaveWindow(object source, RoutedEventArgs args)
        {
            if (SaveMapGrid == null)
            {
                Console.WriteLine("HERERERERERERERERERERERERERERERERERERERERERER");
                SaveMapGrid = Resources["SaveMapTemplate"] as Grid;
                Border border = SaveMapGrid.Children[0] as Border;
                Grid innerGrid = border.Child as Grid;
                ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetMapSaveGrid());
                ((Button) innerGrid.Children[3]).Click += (sender, arg) =>
                {
                    GameScroll.Content = AdminGrid;
                };
            }

            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = SaveMapGrid;
        }

        private void LoadMap(object sender, RoutedEventArgs args)
        {
            Map map = _manager.LoadMap("mapka_test");
            GameMapGrid.Children.Clear();
            AvailableObjects = _gameGridCreator.LoadMapIntoTheGameGrid(this, map);
            GameMapGrid = GameScroll.Content as Grid;
            Selection.Reset();
            SideGrid.Visibility = Visibility.Visible;
            Console.WriteLine("Passed loading");
        }

        private void SaveMap(object sender, RoutedEventArgs args)
        {
            _manager.SaveMap(this);
            Console.WriteLine("Passed saving");
        }

        private void NewMap(object source, RoutedEventArgs args)
        {
            CreateGameBoard();
            Selection.Reset();
            PlacedObjects.Clear();
        }

        private void OpenSelectorTab(object source, RoutedEventArgs args)
        {
            //TODO change to make compatible with load/save windows
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
                    CreateSaveWindow(null, null);
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
            UserGameWindow userWindow = new UserGameWindow(LoginData.GetInstance()) {Owner = this};
            Hide(); // not required if using the child events below
            userWindow.ShowDialog();
        }

        private void ReturnToMenu(object sender, RoutedEventArgs args)
        {
            SideGrid.Visibility = Visibility.Collapsed;
            GameScroll.Content = MenuGrid;
        }

        private void ReturnToLoginWindow(object sender, RoutedEventArgs args)
        {
            //TODO: change after Authorisation module is finished
            Close();
            DEBUG.DEBUG debug = new DEBUG.DEBUG();
            debug.Show();
        }
    }
}
