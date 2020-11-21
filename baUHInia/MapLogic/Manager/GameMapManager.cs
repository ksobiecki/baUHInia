using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Linq;

namespace baUHInia.MapLogic.Manager
{
    class GameMapManager : IGameMapManager
    {
        // Logic
        private string Choice;
        private string Keyword;
        private Map[] mockupMaps;
        private Game[] mockupGames;

        // Determines if operating on maps or games. Absolutely disgusting solution but thats all I can come up with at the moment.
        private int Mode; // 0: Map, 1: MapSave 2: GameLoad, 3: GameSave.
        private Map[] Maps;
        private Game[] Games;

        // Layout
        private Grid ContainerGrid;
        private Grid SearchGrid;
        private ScrollViewer ListScrollViewer;
        private Grid ListGrid;

        public GameMapManager()
        {
            // Debug purposes, remove later.
            Maps = new Map[50];
            Games = new Game[50];
            for (int i = 0; i < 50; i++)
            {
                Maps[i] = new Map("Some map nr" + i);
                Games[i] = new Game("Some game nr" + i);
            }

            CreateContainerGrid();
            CreateSearchGrid();
            CreateListScrollViewer();
            CreateListGrid();

            this.Choice = "";
            this.Keyword = "";
        }

        public Grid GetGameLoadGrid()
        {
            // TODO get games from database.

            Mode = 2;

            ContainerGrid.Children.Clear();
            ContainerGrid.Children.Add(SearchGrid);
            ContainerGrid.Children.Add(ListScrollViewer);
            ListScrollViewer.Content = ListGrid;

            PopulateListGrid();

            return ContainerGrid;
        }

        public Grid GetMapLoadGrid()
        {
            // TODO get maps from database.

            Mode = 0;

            ContainerGrid.Children.Clear();
            ContainerGrid.Children.Add(SearchGrid);
            ContainerGrid.Children.Add(ListScrollViewer);
            ListScrollViewer.Content = ListGrid;

            PopulateListGrid();

            return ContainerGrid;
        }

        public bool GetMapSaveGrid()
        {
            throw new NotImplementedException();
        }

        public Game LoadGame(string name)
        {
            throw new NotImplementedException();
        }

        public Map LoadMap(string name)
        {
            throw new NotImplementedException();
        }

        public bool SaveGame(ITileBinder tileBinder)
        {
            throw new NotImplementedException();
        }

        public bool SaveMap(ITileBinder tileBinder)
        {
            throw new NotImplementedException();
        }

        private void CreateContainerGrid()
        {
            ContainerGrid= new Grid();
            ContainerGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            ContainerGrid.VerticalAlignment = VerticalAlignment.Stretch;
        }

        private void CreateSearchGrid()
        {
            SearchGrid = new Grid();
            SearchGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            SearchGrid.VerticalAlignment = VerticalAlignment.Top;
            SearchGrid.Height = 30;
            SearchGrid.Background = Brushes.Gray;

            TextBox searchTextBox = new TextBox();
            searchTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            searchTextBox.VerticalAlignment = VerticalAlignment.Stretch;
            searchTextBox.Margin = new Thickness(0, 0, 50, 0);
            searchTextBox.Padding = new Thickness(5, 0, 5, 0);
            searchTextBox.VerticalContentAlignment = VerticalAlignment.Center;
            searchTextBox.Background = (Brush)new BrushConverter().ConvertFrom("#FFA7A7A7");
            searchTextBox.TextChanged += SearchTextChanged;

            Button searchButton = new Button();
            searchButton.HorizontalAlignment = HorizontalAlignment.Right;
            searchButton.VerticalAlignment = VerticalAlignment.Stretch;
            searchButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF4A9C38");
            searchButton.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF474747");
            searchButton.Width = 50;
            searchButton.Content = "Szukaj";
            searchButton.Foreground = Brushes.White;
            searchButton.FontFamily = new FontFamily("Segoe UI");
            searchButton.FontWeight = FontWeights.Bold;
            searchButton.Click += SearchButtonClick;

            SearchGrid.Children.Add(searchTextBox);
            SearchGrid.Children.Add(searchButton);
        }

        private void CreateListScrollViewer()
        {
            ListScrollViewer = new ScrollViewer();
            ListScrollViewer.HorizontalAlignment = HorizontalAlignment.Stretch;
            ListScrollViewer.VerticalAlignment = VerticalAlignment.Stretch;
            ListScrollViewer.Margin = new Thickness(0, 30, 0, 0);
            ListScrollViewer.Background = (Brush)new BrushConverter().ConvertFrom("#FFA7A7A7");
        }

        private void CreateListGrid()
        {
            ListGrid = new Grid();
            ListGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            ListGrid.VerticalAlignment = VerticalAlignment.Stretch;
            ListGrid.Name = "ListGrid";
        }

        private void PopulateListGrid()
        {
            ListGrid.RowDefinitions.Clear();
            ListGrid.Children.Clear();

            int index = 0;

            if (Mode == 0)
            {
                Map[] filteredMaps = Maps.Where(m => m.Name.Contains(Keyword)).ToArray();

                foreach (Map map in filteredMaps)
                {
                    ListGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    Grid listItemGrid = new Grid();
                    listItemGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    Grid.SetRow(listItemGrid, index);
                    listItemGrid.Children.Add(CreateListItemButton(map.Name));
                    ListGrid.Children.Add(listItemGrid);
                    index++;
                }
            }
            else if (Mode == 2)
            {
                Game[] filteredGames = Games.Where(g => g.Name.Contains(Keyword)).ToArray();

                foreach (Game game in filteredGames)
                {
                    ListGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    Grid listItemGrid = new Grid();
                    listItemGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    Button listItemButton = CreateListItemButton(game.Name);
                    Grid.SetRow(listItemGrid, index);
                    listItemGrid.Children.Add(listItemButton);
                    ListGrid.Children.Add(listItemGrid);
                    index++;
                }
            }
            
        }

        private Button CreateListItemButton(string name)
        {
            Button listItemButton = new Button();
            listItemButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            listItemButton.VerticalAlignment = VerticalAlignment.Stretch;
            listItemButton.HorizontalContentAlignment = HorizontalAlignment.Center;
            listItemButton.VerticalContentAlignment = VerticalAlignment.Center;
            listItemButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF878787");
            listItemButton.BorderThickness = new Thickness(0, 1, 0, 0);
            listItemButton.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FFA7A7A7");
            listItemButton.Foreground = Brushes.White;
            listItemButton.FontFamily = new FontFamily("Segoe UI");
            listItemButton.FontWeight = FontWeights.Bold;
            listItemButton.Content = name;
            listItemButton.Click += ListItemButtonClick;
            listItemButton.ClickMode = ClickMode.Press;

            return listItemButton;
        }

        // Event handlers

        private void ListItemButtonClick(object s, EventArgs e)
        {
            foreach (Grid listItemGrid in ListGrid.Children)
            {
                Button listItemButton = (Button)listItemGrid.Children[0];
                listItemButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF878787");
                listItemButton.Foreground = Brushes.White;
            }

            Button sender = (Button)s;
            sender.Background = (Brush)new BrushConverter().ConvertFrom("#FF8FAEEC");
            sender.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF474747");
            Choice = sender.Content.ToString();
        }

        private void SearchTextChanged(object s, EventArgs e)
        {
            TextBox sender = (TextBox)s;
            Keyword = sender.Text.ToString();
        }

        private void SearchButtonClick(object s, EventArgs e)
        {
            Choice = "";
            PopulateListGrid();
        }
    }
}
