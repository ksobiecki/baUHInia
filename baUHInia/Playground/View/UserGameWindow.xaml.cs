using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        
        private Dictionary<string, Grid> Grids { get; set; }

        private List<TileCategory> LoadedTileCategories { get; set; }
        private Map LoadedMap { get; set; }
        private int LoadedMapId { get; set; }
        private int InitialPlacerCount { get; set; }
        

        public UserGameWindow(LoginData credentials)
        {
            InitializeComponent();
            InitializeProperties(credentials);
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
        public int AvailableFounds { get; set; } = 1000000;

        //============================ PREDEFINED ACTIONS ============================//

        
        private void InitializeGrids()
        {
            Grids = new Dictionary<string, Grid> {{"Menu", GameScroll.Content as Grid}};
            CreateGameTab();
            CreateLoadMapTab();
            CreateLoadGameTab();
            CreateSaveGameTab();
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
        }

        private void ChangeDropdownSelection(object sender, SelectionChangedEventArgs e)
        {
            TileCategory category = !(CategorySelector.SelectedItem is string item)
                ? LoadedTileCategories[0]
                : LoadedTileCategories.First(c => c.Name == item);
            _selectorGridCreator.CreateSelectionPanel(category, this);
        }
        
        private void UnlockReturnGameButton()
        {
            if ((bool) UserReturnBtn.Tag) return;
            UserReturnText.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#CCCCCC");
            UserReturnImg.Opacity = 1.0f;
            UserReturnBtn.Style = FindResource("MenuButton") as Style;
            UserReturnBtn.Foreground = (SolidColorBrush) new BrushConverter().ConvertFromString("#DDDDDD");
            UserReturnBtn.Click += OpenGameTab;
            UserReturnBtn.Tag = true;
        }

        public void UpdateSelectionWindow(TileObject tileObject)
        {
            GameObject gameObject = AvailableObjects.FirstOrDefault(a => a.TileObject == tileObject);
            _selectionWindowCreator.UpdateSelectionWindow(SelectionGrid, gameObject);
        }

        //============================== INITIAL WINDOW ==============================//

        private void InitializeProperties(LoginData credentials)
        {
            Credentials = credentials;
            PlacedObjects = new List<Placement>();

            AccountName.Text += "\t\t" + credentials.name;
            AccountType.Text += "\t" + (credentials.isAdmin ? "WŁADZE MIASTA" : "MIESZKANIEC");
            Mode.Text += "\t\t" + "MIESZKAŃCA";
            UserReturnBtn.Tag = false;

            UnlockAdminFeatures(credentials.isAdmin);

            _selectionWindowCreator = new VerticalSelectionWindowCreator();
            _simulator = new Score(this, BoardDensity);
            _manager = new GameMapManager(Credentials);
        }

        //=============================== FUNCTIONALITY ==============================//

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
            
            Selection.AssignSelection(LoadedTileCategories[0].TileObjects[0]);
            CategorySelector.ItemsSource = LoadedTileCategories.Select(c => c.Name).ToList();
            CategorySelector.SelectedIndex = 0;
            _selectorGridCreator.UpdateTileGroup(LoadedTileCategories);
            _selectorGridCreator.CreateSelectionPanel(LoadedTileCategories[0], this);
        }
        
        private void ClearMap()
        {
            int count = BoardDensity * BoardDensity * 2;
            Grids["GameMap"].Children.RemoveRange(count, Grids["GameMap"].Children.Count - count);
            PlacedObjects.Clear();
            Selection.PersistBaseLayer(false);
            Selection.Reset();
            AvailableObjects = null;
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

        private void Simulate(object sender, RoutedEventArgs args) =>
            Points.Text = _simulator.SimulationScore().ToString(); 
        

        private void LoadMap(object sender, RoutedEventArgs args)
        {
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
            Game game;
            
            try { game = _manager.LoadGame(); }
            catch (Exception) { return; }
            
            LoadedMap = game.Map;
            PrepareLoadedMap(null, null);
            CurrentCash.Text = int.MaxValue.ToString();
            _gameGridCreator.LoadGameIntoTheGameGrid(this, game);
            
            int cost = game.PlacedObjects.Sum(p => p.GameObject.Price);
            CurrentCash.Text = (LoadedMap.AvailableMoney - cost).ToString();
        }

        private void PrepareLoadedMap(object source, RoutedEventArgs args)
        {
            ClearMap();
            OpenGameTab(null, null);
            Selection.PersistBaseLayer(true);
            
            AvailableObjects = _gameGridCreator.LoadMapIntoTheGameGrid(this, LoadedMap);
            PlacerGridCreator.InitializeElementsLayer(GameScroll.Content as Grid, Selection, BoardDensity);
            
            InitialPlacerCount = PlacedObjects.Count;
            AllCash.Text = CurrentCash.Text = AvailableFounds.ToString();
            MapName.Text = LoadedMap.Name;
            UpdateSelectorComboBox();
            UnlockReturnGameButton();
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
            GameScroll.Content = Grids["GameMap"];
            PlacedObjects = allPlacements;
        }
        
        //======================================// GRID SWITCH //======================================//

        private void OpenGameTab(object source, RoutedEventArgs args)
        {
            ChangeDisplayMode(true);
            GameScroll.Content = Grids["GameMap"];
        }

        private void OpenLoadMapTab(object source, RoutedEventArgs args)
        {
            ChangeDisplayMode(false);
            _manager.PopulatePlayLoadMapListGrid();
            GameScroll.Content = Grids["LoadMap"];
        }
        
        private void OpenLoadGameTab(object source, RoutedEventArgs args)
        {
            ChangeDisplayMode(false);
            _manager.PopulateUserLoadGameListGrid();
            GameScroll.Content = Grids["LoadGame"];
        }

        private void OpenSaveGameTab(object source, RoutedEventArgs args)
        {
            ChangeDisplayMode(false);
            _manager.PopulateSaveGameListGrid();
            GameScroll.Content = Grids["SaveGame"];
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
        
        private void CreateLoadMapTab()
        {
            Grids.Add("LoadMap", Resources["LoadMapTemplate"] as Grid);
            Border border = Grids["LoadMap"].Children[0] as Border;
            Grid innerGrid = border.Child as Grid;
            ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetMapLoadGrid());
            ((Button) innerGrid.Children[3]).Click += (sender, arg) => GameScroll.Content = Grids["Menu"];
        }

        private void CreateLoadGameTab()
        {
            Grids.Add("LoadGame", Resources["LoadGameTemplate"] as Grid);
            Border border = Grids["LoadGame"].Children[0] as Border;
            Grid innerGrid = border.Child as Grid;
            ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetGameLoadGrid());
            ((Button) innerGrid.Children[3]).Click += (sender, arg) => { GameScroll.Content = Grids["Menu"]; };
        }

        private void CreateSaveGameTab()
        {
            Grids.Add("SaveGame", Resources["SaveGameTemplate"] as Grid);
            Border border = Grids["SaveGame"].Children[0] as Border;
            Grid innerGrid = border.Child as Grid;
            ((Grid) innerGrid.Children[1]).Children.Add(_manager.GetGameSaveGrid());
            ((Button) innerGrid.Children[3]).Click += (sender, arg) =>
            {
                GameScroll.Content = Grids["GameMap"];
                ChangeDisplayMode(true);
            };
        }
        
        //=====================================// OTHER WINDOWS //=====================================//

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
            GameScroll.Content = Grids["Menu"];
        }

        private void ExitApplication(object sender, RoutedEventArgs args) => Application.Current.Shutdown();

        private void ChangeDisplayMode(bool visible) =>
            Bar.Visibility = SideGrid.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        
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