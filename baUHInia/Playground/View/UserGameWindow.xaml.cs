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
    public partial class UserGameWindow : ITileBinder
    {
        private const byte BoardDensity = 50;

        //private GameGridCreator _gameGridCreator;
        //private SelectorGridCreator _selectorGridCreator;

        private IGameGridCreator _gameGridCreator;
        private ISelectorGridCreator _selectorGridCreator;
        private ISelectionWindowCreator _selectionWindowCreator;

        private IGameMapManager _manger;
        //private ISimulate _simulator;

        //TODO: add credentials
        public UserGameWindow()
        {
            InitializeComponent();
            //InitializeSelection();
            ShowStartupPanel();
            //CreateGameBoard();
            //CreateSelectorGrid();
            //FillComboBoxWithCategories();
            AdjustWindowSizeAndPosition();
        }

        //========================= INTERFACE IMPLEMENTATIONS ========================//
        public Selection Selection { get; private set; }

        //TODO: rest
        public Tile[,] TileGrid { get; private set; }
        public List<Placement> PlacedObjects { get; }
        public ScrollViewer GameViewer => GameScroll;
        public Grid SelectorGrid => UserSelectorGrid;
        public List<GameObject> AvailableObjects { get; }
        public LoginData Credentials { get; }
        public int AvailableFounds { get; set; }
        public void ChangeInteractionMode(string text, System.Windows.Media.Brush color) => throw new NotImplementedException();

        //============================ PREDEFINED ACTIONS ============================//

        private void InitializeSelection() => Selection = new Selection(
            ResourceHolder.Get.GetTerrainTileObject("Plain Dirt"), this);

        private void AdjustWindowSizeAndPosition()
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Window.MaxHeight = SystemParameters.WorkArea.Height + 14.0;
        }

        //TODO: change!
        private void FillComboBoxWithCategories() =>
            CategorySelector.ItemsSource = ResourceHolder.Get.Terrain.Select(c => c.Name);

        private void ShowStartupPanel()
        {
            Grid loadMapGrid = new Grid {Width = 200, Height = 300}; //_manger
            Grid loadGameGrid = new Grid {Width = 200, Height = 300}; //_manger
            InitialMapGrid = loadMapGrid;
            InitialGameGrid = loadGameGrid;
        }

        private void Simulate()
        {
            //TODO: implement
        }

        private void LoadMap()
        {
            //TODO: implement
        }

        private void LoadGame()
        {
            //TODO: implement
        }

        private void SaveGame()
        {
            //TODO: implement
        }

        private void SaveStateAsJpg()
        {
            //TODO: implement
        }

        private void GoToAdminMode()
        {
            //TODO: implement
        }

        private void ReturnToLoginWindow()
        {
            //TODO: implement
        }

        //TODO: move to admin
        private void CreateGameBoard()
        {
            TileGrid = new Tile[BoardDensity, BoardDensity];
            TileObject tileObject = ResourceHolder.Get.GetTerrainTileObject("Plain Grass");
            _gameGridCreator = new PlacerGridCreator(this, BoardDensity, tileObject);
            _gameGridCreator.CreateGameGridInWindow(this, BoardDensity);
        }

        private void CreateSelectorGrid()
        {
            List<TileCategory> categories = ResourceHolder.Get.GetSelectedCategories();
            _selectorGridCreator = new UserSelectorGridCreator(this, categories);
        }

        private void UpdateSelectionWindow()
        {
            //TODO: implement
        }


        //============================ ELEMENTS BEHAVIOUR =============================//

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string item = comboBox.SelectedItem as string;
            _selectorGridCreator.CreateSelectionPanel(ResourceHolder.Get.Terrain.First(c => c.Name == item), this);
        }
    }
}