using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Admin;
using baUHInia.Authorisation;
using baUHInia.MapLogic.Manager;
using baUHInia.Playground.Logic.Creators;
using baUHInia.Playground.Logic.Creators.Selector;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Playground.View
{
    public partial class AdminGameWindow : ITileBinder
    {
        private const byte BoardDensity = 50;

        private IGameGridCreator _gameGridCreator;
        private ISelectorGridCreator _selectorGridCreator;

        private IGameMapManager _manager;

        private IAdminSelectorTabCreator _admin;

        private UIElement _storedElement;

        public AdminGameWindow(LoginData credentials)
        {
            InitializeComponent();
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
            ModeText.Text = text;
            ModeText.Foreground = color;
        }
        
        private void InitializeSelection() => Selection = new Selection(
            ResourceHolder.Get.GetTerrainTileObject("Plain Dirt"), this);

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
            string item = CategorySelector.SelectedItem as string;
            TileCategory category = ResourceHolder.Get.GetCategoryByName(item);
            _selectorGridCreator.CreateSelectionPanel(category, this);
        }

        private void CreateGameBoard()
        {
            SideGrid.Visibility = Visibility.Visible;
            TileGrid = new Tile[BoardDensity, BoardDensity];
            _gameGridCreator = new TileGridCreator(this, BoardDensity);
            _gameGridCreator.CreateGameGridInWindow(this, BoardDensity);
        }
        
        private void CreateSelectorGrid() => _selectorGridCreator = new AdminSelectorGridCreator(this);
        
        private void SwapBoardAndAdminPanel()
        {
            UIElement temp = _storedElement;
            _storedElement = GameScroll.Content as UIElement;
            _storedElement.Visibility = Visibility.Visible;
            GameScroll.Content = temp;
            temp.Visibility = Visibility.Visible;
        }
        
        //============================== INITIAL WINDOW ==============================//

        private void AddLoadCardAndInitializeManager()
        {
            _manager = new GameMapManager();
            InitialMapGrid.Children.Add(_manager.GetMapLoadGrid());
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
            TileCategory category = ResourceHolder.Get.GetFirstTileCategory();

            CategorySelector.ItemsSource = ResourceHolder.Get.GetCategoryName();
            CategorySelector.SelectedIndex = 0;
            _selectorGridCreator.CreateSelectionPanel(category, this);
        }

        private void LoadMap()
        {
            //TODO: implement
        }

        private void SaveMap()
        {
            //TODO: implement
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
            if (_storedElement == null)
            {
                _admin.GetReturnButton().Click += (sender, eventArgs) =>
                {
                    AvailableObjects = _admin.GetModifiedAvailableObjects();
                    AvailableFounds = _admin.GetBudget();
                    SwapBoardAndAdminPanel();
                };
                _storedElement = GameScroll.Content as UIElement;
                Grid grid = _admin.GetAdminSelectorTableGrid();
                grid.VerticalAlignment = VerticalAlignment.Center;
                grid.HorizontalAlignment = HorizontalAlignment.Center;
                GameScroll.Content = grid;
                _storedElement.Visibility = Visibility.Hidden;
            }
            else SwapBoardAndAdminPanel();
        }

        private void ChangeGameMode(object sender, RoutedEventArgs args)
        {
            UserGameWindow userWindow = new UserGameWindow {Owner = this};
            Hide(); // not required if using the child events below
            userWindow.ShowDialog();
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