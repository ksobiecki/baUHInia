﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using baUHInia.Admin;
using baUHInia.Authorisation;
using baUHInia.MapLogic.Manager;
using baUHInia.MapLogic.Model;
using baUHInia.Playground.Logic.Creators;
using baUHInia.Playground.Logic.Creators.Selector;
using baUHInia.Playground.Logic.Creators.Tiles;
using baUHInia.Playground.Logic.Utils;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;
using baUHInia.Simulation;
using Microsoft.Win32;
using Button = System.Windows.Controls.Button;

namespace baUHInia.Playground.View
{
    public partial class UserGameWindow : ITileBinder
    {
        private const byte BoardDensity = 50;

        private ISelectionWindowCreator _selectionWindowCreator;

        private ISelectorGridCreator _selectorGridCreator;

        private IGameGridCreator _gameGridCreator;

        private IGameMapManager _manager;

        private ISimulate _simulator;

        private Grid MenuGrid { get; set; }

        private Grid SaveGameGrid { get; set; }

        private Grid LoadGameGrid { get; set; }
        private Grid LoadMapGrid { get; set; }
        private Grid GameMapGrid { get; set; }
        
        private Map LoadedMap { get; set; }
        private int LoadedMapId { get; set; }
        private int InitialPlacerCount { get; set; }
        
        private List<TileCategory> LoadedTileCategories { get; set; }

        public UserGameWindow(LoginData credentials)
        {
            InitializeComponent();
            StoreMenuGrid();
            InitializeProperties(credentials);
            AddLoadCardAndInitializeManager();
            AdjustWindowSizeAndPosition();
            AddLoadCardAndInitializeManager();
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
        public int AvailableFounds { get; set; } = 1000000;

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
                ? LoadedTileCategories[0]
                : LoadedTileCategories.First(c => c.Name == item);
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
            _gameGridCreator.CreateElementsInWindow(this, BoardDensity);
            GameMapGrid = GameScroll.Content as Grid;
        }

        private void CreateSelectorGrid()
        {
            _selectorGridCreator = new AdminSelectorGridCreator(this, LoadedTileCategories);
        }

        public void UpdateSelectionWindow(TileObject tileObject)
        {
            GameObject gameObject = AvailableObjects.FirstOrDefault(a => a.TileObject == tileObject);
            _selectionWindowCreator.UpdateSelectionWindow(SelectionGrid, gameObject);
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
            _manager = new GameMapManager(Credentials);
            //TODO:
            //InitialMapGrid.Children.Add(_manager.GetMapLoadGrid());
        }

        private void InitializeProperties(LoginData credentials)
        {
            Credentials = credentials;
            PlacedObjects = new List<Placement>();
            //AvailableObjects = new List<GameObject>();

            AccountName.Text += "\t\t" + credentials.name;
            AccountType.Text += "\t" + (credentials.isAdmin ? "WŁADZE MIASTA" : "MIESZKANIEC");
            Mode.Text += "\t\t" + "MIESZKAŃCA";

            UnlockAdminFeatures(credentials.isAdmin);

            _selectionWindowCreator = new VerticalSelectionWindowCreator();
            _simulator = new Score(this, BoardDensity);
        }

        //=============================== FUNCTIONALITY ==============================//

        private void CreateNewMap(object sender, RoutedEventArgs args)
        {
            CreateGameBoard();
            CreateSelectorGrid();
            InitializeInteractionChangers();
        }

        private void UpdateSelectorComboBox()
        {
            ResourceHolder.Get.ChangeResourceType(ResourceType.Foliage);
            List<TileCategory> rootCategories = ResourceHolder.Get.GetSelectedCategories();
            
            LoadedTileCategories = rootCategories
                .Select(c => new TileCategory(c.Name, new List<TileObject>(c.TileObjects)))
                .ToList();

            List<TileObject> tileObjects = AvailableObjects.Select(o => o.TileObject).ToList();

            foreach (TileCategory tileCategory in LoadedTileCategories)
            {
                var objectsToRemove = tileCategory.TileObjects.Where(t => !tileObjects.Contains(t)).ToList();
                foreach (TileObject tileObject in objectsToRemove)
                {
                    tileCategory.TileObjects.Remove(tileObject);
                }
            }

            LoadedTileCategories.RemoveAll(c => c.TileObjects.Count == 0);
            
            CategorySelector.ItemsSource = LoadedTileCategories.Select(c => c.Name).ToList();
            CategorySelector.SelectedIndex = 0;
            _selectorGridCreator.UpdateTileGroup(LoadedTileCategories);
            _selectorGridCreator.CreateSelectionPanel(LoadedTileCategories[0], this);
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

            _manager.PopulatePlayLoadMapListGrid();

            GameScroll.Content = LoadMapGrid;
        }
        
        private void CreateLoadGameWindow(object source, RoutedEventArgs args)
        {
            if (LoadGameGrid == null)
            {
                LoadGameGrid = Resources["LoadGameTemplate"] as Grid;
                Border border = LoadGameGrid.Children[0] as Border;
                Grid innerGrid = border.Child as Grid;
                ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetGameLoadGrid());
                ((Button) innerGrid.Children[3]).Click += (sender, arg) => { GameScroll.Content = MenuGrid; };
            }

            _manager.PopulateUserLoadGameListGrid();
            GameScroll.Content = LoadGameGrid;
        }

        private void CreateSaveWindow(object source, RoutedEventArgs args)
        {
            if (SaveGameGrid == null)
            {
                SaveGameGrid = Resources["SaveGameTemplate"] as Grid;
                Border border = SaveGameGrid.Children[0] as Border;
                Grid innerGrid = border.Child as Grid;
                ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetGameSaveGrid());
                ((Button) innerGrid.Children[3]).Click += (sender, arg) =>
                {
                    GameScroll.Content = GameMapGrid;
                    ChangeDisplayMode(true);
                };
            }

            _manager.PopulateSaveGameListGrid();

            ChangeDisplayMode(false);
            GameScroll.Content = SaveGameGrid;
        }

        private void Simulate(object sender, RoutedEventArgs args)
        {
            Points.Text = _simulator.SimulationScore().ToString(); 
        }

        private void LoadMap(object sender, RoutedEventArgs args)
        {
            if (LoadedMap != null) ClearMap();
            try
            {
                LoadedMap = _manager.LoadMap(out int mapId);
                LoadedMapId = mapId;
            }
            catch (Exception) { return; }
            PrepareLoadedMap(null, null);
        }
        
        private void LoadGame(object sender, RoutedEventArgs args)
        {
            if (LoadedMap != null) ClearMap();
            Game game;
            
            try { game = _manager.LoadGame(); }
            catch (Exception) { return; }
            
            LoadedMap = game.Map;
            PrepareLoadedMap(null, null);
            _gameGridCreator.LoadGameIntoTheGameGrid(this, game);
        }

        private void PrepareLoadedMap(object sender, RoutedEventArgs args)
        {
            AvailableObjects = null;
            Selection = new Selection(null, this);
            CreateNewMap(null, null);
            AvailableObjects = _gameGridCreator.LoadMapIntoTheGameGrid(this, LoadedMap);
            GameMapGrid = GameScroll.Content as Grid;
            InitialPlacerCount = PlacedObjects.Count;

            ChangeDisplayMode(true);
            AllCash.Text = CurrentCash.Text = AvailableFounds.ToString();
            MapName.Text = LoadedMap.Name;

            PlacerGridCreator creator = _gameGridCreator as PlacerGridCreator;
            creator.InitializeElementsLayer(GameScroll.Content as Grid, Selection, BoardDensity);
            Selection.TileObject = AvailableObjects[0].TileObject;
            UpdateSelectionWindow(AvailableObjects[0].TileObject);
            UpdateSelectorComboBox();
        }

        private void SaveGame(object sender, RoutedEventArgs args)
        {
            List<Placement> allPlacements = PlacedObjects;
            int newCount = PlacedObjects.Count - InitialPlacerCount;
            PlacedObjects = PlacedObjects.GetRange(InitialPlacerCount, newCount);

            try { _manager.SaveGame(this,LoadedMapId); }
            catch (Exception)
            {
                PlacedObjects = allPlacements;
                return;
            }
            ChangeDisplayMode(true);
            GameScroll.Content = GameMapGrid;
            
            PlacedObjects = allPlacements;
            Console.WriteLine("Passed saving");
        }

        private void SaveStateAsJpg(object sender, RoutedEventArgs args)
        {
            RenderTargetBitmap renderTargetBitmap = ImageWriter.CopyAsBitmap(GameScroll.Content as Grid);
            byte[] bytes = ImageWriter.Encode(renderTargetBitmap, new JpegBitmapEncoder());
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = ".jpeg",
                FileName = LoadedMap.Name,
                Title = "Zapisz zrzut mapy jako",
                AddExtension = true,
                OverwritePrompt = true
            };
            if (saveFileDialog.ShowDialog() == true) File.WriteAllBytes(saveFileDialog.FileName, bytes);
        }

        private void ClearMap()
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
            Hide();
        }

        private void ReturnToMenu(object sender, RoutedEventArgs args)
        {
            ChangeDisplayMode(false);
            GameScroll.Content = MenuGrid;
        }

        private void ReturnToLoginWindow(object sender, RoutedEventArgs args)
        {
            try
            {
                Close();
                Authorisation.Authorisation authorization = new Authorisation.Authorisation();
                authorization.Show();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private void ChangeDisplayMode(bool visible) =>
            Bar.Visibility = SideGrid.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;


        private void UnlockAdminFeatures(bool isAdmin)
        {
            if (!isAdmin) return;
            AdminButton.Style = FindResource("MenuButton") as Style;
            AdminButton.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#DDDDDD");
            AdminTitle.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#CCCCCC");
            AdminButton.Click += ChangeGameMode;
            AdminImage.Opacity = 1.0f;
        }
    }
}