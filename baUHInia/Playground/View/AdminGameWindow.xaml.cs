using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using baUHInia.Authorisation;
using baUHInia.Playground.Logic.Creators.Selector;
using baUHInia.Playground.Logic.Creators.Tiles;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.View
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class AdminGameWindow : ITileBinder
    {
        private const byte BoardDensity = 50;

        private GameGridCreator _gameGridCreator;
        private SelectorGridCreator _selectorGridCreator;

        public AdminGameWindow()
        {
            InitializeComponent();
            //LoadTilesFromResources();
            InitializeSelection();
            CreateGameBoard();
            CreateSelectorGrid();
            FillComboBoxWithCategories();
            AdjustWindowSizeAndPosition();
        }

        //========================= INTERFACE IMPLEMENTATIONS ========================//
        
        public Selection Selection { get; private set; }

        //TODO: rest
        public Tile[,] TileGrid { get; private set; }
        public List<Placement> PlacedObjects { get; }
        public ScrollViewer GameViewer { get; }
        public Grid SelectorGrid => AdminSelectorGrid;
        public List<GameObject> AvailableObjects { get; }
        public LoginData Credentials { get; }
        public int AvailableFounds { get; }

        //============================ PREDEFINED ACTIONS ============================//

        //private void LoadTilesFromResources() => TileCategories = ResourceLoader.LoadTileCategories();

        private void InitializeSelection() => Selection = new Selection(
            ResourceHolder.Get.Terrain.First(c => c.Name == "terrain").TileObjects.First(o => o.Name == "dirt"));

        private void AdjustWindowSizeAndPosition()
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Window.MaxHeight = SystemParameters.WorkArea.Height + 14.0;
        }

        private void CreateGameBoard()
        {
            TileGrid = new Tile[BoardDensity, BoardDensity];
            _gameGridCreator = new GameGridCreator(this, BoardDensity);
            _gameGridCreator.CreateGameGridInWindow(TileGrid, GameScroll);
        }

        private void CreateSelectorGrid() => _selectorGridCreator = new SelectorGridCreator(this, SelectorGrid);

        private void FillComboBoxWithCategories() =>
            CategorySelector.ItemsSource = ResourceHolder.Get.Terrain.Select(c => c.Name);

        //============================ ELEMENTS BEHAVIOUR =============================//

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string item = comboBox.SelectedItem as string;
            _selectorGridCreator.CreateSelectionPanel(ResourceHolder.Get.Terrain.First(c => c.Name == item));
        }

        private void Filler_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bi = new BitmapImage(new Uri("pack://application:,,,/resources/terrain/Test/Tester/oak.png"));
            Image img = new Image {Source = bi, IsHitTestVisible = false};
            Grid.SetRow(img, 5);
            Grid.SetColumn(img, 5);
            _gameGridCreator._gameGrid.Children.Add(img);
        }
    }
}