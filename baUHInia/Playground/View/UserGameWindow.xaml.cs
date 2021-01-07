using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
using baUHInia.Admin;
using Button = System.Windows.Controls.Button;

namespace baUHInia.Playground.View
{
    public partial class UserGameWindow : ITileBinder
    {
        private const byte BoardDensity = 50;
        
        private ISelectorGridCreator _selectorGridCreator;
        private IGameGridCreator _gameGridCreator;
        
        private IGameMapManager _manager;
        private ISimulate _simulator;
        //TODO: element zoom-in
        
        private Grid MenuGrid { get; set; }
        
        private Grid SaveGameGrid { get; set; }
        
        private Grid LoadGameGrid { get; set; }
        private Grid LoadMapGrid { get; set; }
        private Grid GameMapGrid { get; set; }
        

        public UserGameWindow(LoginData credentials)
        {
            InitializeComponent();
            StoreMenuGrid();
            AddLoadCardAndInitializeManager();
            AdjustWindowSizeAndPosition();
            AddLoadCardAndInitializeManager();
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
        public bool IsInAdminMode { get; } = true;
        public int AvailableFounds { get; set; }

        //============================ PREDEFINED ACTIONS ============================//

        private void StoreMenuGrid() => MenuGrid = GameScroll.Content as Grid;
        
        private void BackToGame(object sender, RoutedEventArgs args)
        {
            ChangeDisplayMode(true);
            GameScroll.Content = GameMapGrid;
        }
        
        private void AdjustWindowSizeAndPosition()
        {
            WindowState = WindowState.Maximized;
            Window.MaxHeight = SystemParameters.WorkArea.Height + 14.0;
        }
        
        private void InitializeInteractionChangers()
        {
            DeleteButton.Click += (o, args) => Selection.ChangeState(State.Remove);
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
            UserReturnText.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#CCCCCC");
            UserReturnImg.Opacity = 1.0f;
            UserReturnBtn.Style = FindResource("MenuButton") as Style;
            UserReturnBtn.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#DDDDDD");
            UserReturnBtn.Click += BackToGame;
        
            ChangeDisplayMode(true);
            TileGrid = new Tile[BoardDensity, BoardDensity];
            TileObject tileObject = ResourceHolder.Get.GetTerrainTileObject("Plain Grass");
            _gameGridCreator = new PlacerGridCreator(this, BoardDensity, tileObject);
            _gameGridCreator.CreateGameGridInWindow(this, BoardDensity);
            GameMapGrid = GameScroll.Content as Grid;
        }
        
        private void CreateSelectorGrid()
        {
            List<TileCategory> categories = ResourceHolder.Get.GetSelectedCategories();
            _selectorGridCreator = new AdminSelectorGridCreator(this, categories);
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
            
            AccountName.Text += "\t\t" + credentials.name;
            AccountType.Text += "\t" + (credentials.isAdmin ? "WŁADZE MIASTA" : "MIESZKANIEC");
            Mode.Text += "\t\t" + "MIESZKAŃCA";

            //TODO: FIND ANSWER
            new AdminRestrictionsWindow(this);
            
            UnlockAdminFeatures(credentials.isAdmin);
            
            _simulator = new Simulation.Simulation();
        }
        
        //=============================== FUNCTIONALITY ==============================//
        
        private void CreateNewMap(object sender, RoutedEventArgs args)
        {
            CreateGameBoard();
            CreateSelectorGrid();
            InitializeInteractionChangers();
        }
        
        private void UpdateSelectorComboBox(ResourceType type)
        {
            ResourceHolder.Get.ChangeResourceType(type);
            List<TileCategory> categories = ResourceHolder.Get.GetSelectedCategories();
        
            List<TileObject> tileObjects = AvailableObjects.Select(o => o.TileObject).ToList();
        
            foreach (TileCategory tileCategory in categories)
            {
                var objectsToRemove = tileCategory.TileObjects.Where(t => !tileObjects.Contains(t)).ToList();
                foreach (TileObject tileObject in objectsToRemove)
                {
                    tileCategory.TileObjects.Remove(tileObject);
                }
            }
        
            //TODO: test
            categories.RemoveAll(c => c.TileObjects.Count == 0);
            
        
            CategorySelector.ItemsSource = categories.Select(c => c.Name).ToList();
            CategorySelector.SelectedIndex = 0;
            _selectorGridCreator.UpdateTileGroup(categories);
            _selectorGridCreator.CreateSelectionPanel(categories[0], this);
        }
        
        private void CreateLoadWindow(object source, RoutedEventArgs args)
        {
            if (LoadMapGrid == null)
            {
                LoadMapGrid = Resources["LoadMapTemplate"] as Grid;
                Border border = LoadMapGrid.Children[0] as Border;
                Grid innerGrid = border.Child as Grid;
                ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetMapLoadGrid());
                ((Button) innerGrid.Children[3]).Click += (sender, arg) => { GameScroll.Content = MenuGrid; };
            }
        
            GameScroll.Content = LoadMapGrid;
        }
        
        private void CreateSaveWindow(object source, RoutedEventArgs args)
        {
            if (SaveGameGrid == null)
            {
                SaveGameGrid = Resources["SaveGameTemplate"] as Grid;
                Border border = SaveGameGrid.Children[0] as Border;
                Grid innerGrid = border.Child as Grid;
                //TODO: uncomment after saveGameGrid is completed
                ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetGameSaveGrid());
                ((Button) innerGrid.Children[3]).Click += (sender, arg) =>
                {
                    GameScroll.Content = GameMapGrid;
                    ChangeDisplayMode(true);
                };
            }
        
            ChangeDisplayMode(false);
            GameScroll.Content = SaveGameGrid;
        }
        
        private void Simulate(object sender, RoutedEventArgs args)
        {
            _simulator.Sim(this, BoardDensity);
            //Points.Text = _simulator.returnScoreTemperature();
        }
        
        private void LoadMap(object sender, RoutedEventArgs args)
        {
            Map map = _manager.LoadMap("mapka_test");
            Selection = new Selection(null, this);
            if (GameMapGrid == null) CreateNewMap(null, null);
            GameMapGrid.Children.Clear();
            AvailableObjects = _gameGridCreator.LoadMapIntoTheGameGrid(this, map);
            GameMapGrid = GameScroll.Content as Grid;
            
            ChangeDisplayMode(true);
            AllCash.Text = CurrentCash.Text = AvailableFounds.ToString();
            MapName.Text = map.Name;
            
            ((PlacerGridCreator) _gameGridCreator).InitializeElementsLayer(
                GameScroll.Content as Grid, 
                Selection,
                BoardDensity);
            Selection.TileObject = AvailableObjects[0].TileObject;
            UpdateSelectorComboBox(ResourceType.Foliage);
            Console.WriteLine("Passed loading");
        }
        
        private void SaveGame(object sender, RoutedEventArgs args)
        {
            _manager.SaveGame(this);
            //TODO: implement
            Console.WriteLine("Passed saving");
        }
        
        private void ClearMap(object source, RoutedEventArgs args)
        {
            CreateGameBoard();
            Selection.Reset();
            PlacedObjects.Clear();
        }
        
        private void ShowStatistics(object sender, RoutedEventArgs args)
        {
            Statistics.Statistics statistics = new Statistics.Statistics();
            statistics.Show();
        }
        
        private void ChangeGameMode(object sender, RoutedEventArgs args)
        {
            Owner.Show();
            Close();
        }
        
        private void ReturnToMenu(object sender, RoutedEventArgs args)
        {
            ChangeDisplayMode(false);
            GameScroll.Content = MenuGrid;
        }

        private void ReturnToLoginWindow(object sender, RoutedEventArgs args)
        {
            try { //Close();
                //Owner?.Close();
                this.Hide();
                Authorisation.Authorisation authorization = new Authorisation.Authorisation();
                authorization.Show();
                this.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        
        private void ChangeDisplayMode(bool visible)
        {
            Bar.Visibility = SideGrid.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
        
        private void UnlockAdminFeatures(bool isAdmin)
        {
            if (!isAdmin) return;
            AdminButton.Style = FindResource("MenuButton") as Style;
            AdminButton.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#DDDDDD");
            AdminTitle.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#CCCCCC");
            AdminButton.Click += ChangeGameMode;
            AdminImage.Opacity = 1.0f;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}