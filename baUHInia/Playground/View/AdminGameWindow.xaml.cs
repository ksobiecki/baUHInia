using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class AdminGameWindow : ITileBinder
    {
        private const byte BoardDensity = 50;

        //private GameGridCreator _gameGridCreator;
        //private SelectorGridCreator _selectorGridCreator;

        private IGameGridCreator _gameGridCreator;
        private ISelectorGridCreator _selectorGridCreator;

        private IGameMapManager _manager;
        //private IAdminSelectorTabCreator _adminCreator;

        public AdminGameWindow(LoginData credentials)
        {
            InitializeComponent();
            //InitializeSelection();
            //CreateGameBoard();
            //CreateSelectorGrid();
            //FillCardsAndComboBoxWithCategories();
            AdjustWindowSizeAndPosition();

            Credentials = credentials;

            // Debug purposes, remove later.
            _manager = new GameMapManager();
            SaveButton.Click += (o, arg) => _manager.SaveMap(this);
        }

        //========================= INTERFACE IMPLEMENTATIONS ========================//

        public Selection Selection { get; private set; }

        //TODO: rest
        public Tile[,] TileGrid { get; private set; }
        public List<Placement> PlacedObjects { get; private set; }
        public ScrollViewer GameViewer => GameScroll;
        public Grid SelectorGrid => AdminSelectorGrid;
        public List<GameObject> AvailableObjects { get; }
        public LoginData Credentials { get; }
        public int AvailableFounds { get; }

        public void ChangeMode(string text, System.Windows.Media.Brush color)
        {
            ModeText.Text = text;
            ModeText.Foreground = color;
        }

        //============================ PREDEFINED ACTIONS ============================//

        private void InitializeSelection() => Selection = new Selection(
            ResourceHolder.Get.Terrain.First(c => c.Name == "terrain").TileObjects.First(o => o.Name == "Plain Dirt"),
            this);

        private void AdjustWindowSizeAndPosition()
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Window.MaxHeight = SystemParameters.WorkArea.Height + 14.0;
        }

        private void CreateNewMap(Object sender, RoutedEventArgs e)
        {
            InitializeSelection();
            CreateGameBoard();
            CreateSelectorGrid();
            FillCardsAndComboBoxWithCategories();
            PlacedObjects = new List<Placement>();
            DeleteButton.Click += (o, args) => Selection.ChangeState(State.Remove);
            PlaceableButton.Click += (o, args) => Selection.ChangeState(State.Block); 
            /*foreach (Tile tile in TileGrid)
            {
                Button button = tile.GetUIElement() as Button;
                button.Width = button.ActualHeight;
            }*/
        }

        private void CreateGameBoard()
        {
            SideGrid.Visibility = Visibility.Visible;
            TileGrid = new Tile[BoardDensity, BoardDensity];
            _gameGridCreator = new TileGridCreator(this, BoardDensity);
            _gameGridCreator.CreateGameGridInWindow(this, BoardDensity);
        }

        private void CreateSelectorGrid() => _selectorGridCreator = new AdminSelectorGridCreator(this);

        private void FillCardsAndComboBoxWithCategories() =>
            CategorySelector.ItemsSource =
                ResourceHolder.Get.Terrain.Select(c => char.ToUpper(c.Name[0]) + c.Name.Substring(1));

        private void UpdateSelectionWindow()
        {
            //TODO: implement
        }

        private void UpdateComboBox()
        {
            //TODO: implement
        }

        private void LoadMap()
        {
            //TODO: implement
        }

        private void SaveMap()
        {
            //TODO: implement
        }

        private void OpenSelectorTab()
        {
            //TODO: implement
        }

        private void TestMap()
        {
            //TODO: implement
        }

        private void ReturnToLoginWindow()
        {
            //TODO: implement
        }

        //============================ ELEMENTS BEHAVIOUR =============================//

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string item = comboBox.SelectedItem as string;
            _selectorGridCreator.CreateSelectionPanel(ResourceHolder.Get.Terrain.First(c => c.Name == item.ToLower()),
                this);
        }
    }
}