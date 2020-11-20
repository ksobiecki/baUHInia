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
        private string Choice;
        private string Keyword;
        private Map[] mockupMaps;

        public GameMapManager()
        {
            // Debug purposes, remove later.
            mockupMaps = new Map[50];
            for (int i = 0; i < 50; i++)
            {
                mockupMaps[i] = new Map("Some map nr" + i);
            }

            this.Choice = "";
            this.Keyword = "";
        }

        public Grid GetGameLoadGrid()
        {
            throw new NotImplementedException();
        }

        public Grid GetMapLoadGrid()
        {
            Grid containerGrid = new Grid();
            containerGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            containerGrid.VerticalAlignment = VerticalAlignment.Stretch;

            ScrollViewer listScrollViewer = new ScrollViewer();
            listScrollViewer.HorizontalAlignment = HorizontalAlignment.Stretch;
            listScrollViewer.VerticalAlignment = VerticalAlignment.Stretch;
            listScrollViewer.Margin = new Thickness(0, 30, 0, 0);
            listScrollViewer.Background = (Brush) new BrushConverter().ConvertFrom("#FFA7A7A7");

            Grid searchGrid = CreateSearchAndListGrid(listScrollViewer);

            containerGrid.Children.Add(searchGrid);
            containerGrid.Children.Add(listScrollViewer);

            return containerGrid;
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

        private Grid CreateListGrid(Map[] maps)
        {
            Grid listGrid = new Grid();
            listGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            listGrid.VerticalAlignment = VerticalAlignment.Stretch;
            listGrid.Name = "ListGrid";

            int index = 0;
            foreach (Map map in maps)
            {
                listGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                Grid listItemGrid = new Grid();
                listItemGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                Button listItemButton = CreateListButton(map.Name, listGrid);
                Grid.SetRow(listItemGrid, index);
                listItemGrid.Children.Add(listItemButton);
                listGrid.Children.Add(listItemGrid);
                index++;
            }

            return listGrid;
        }

        private Button CreateListButton(string name, Grid listGrid)
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
            listItemButton.Click += (s, e) => { ListItemButtonClick((Button) s, listGrid); };
            listItemButton.ClickMode = ClickMode.Press;

            return listItemButton;
        }

        private void ListItemButtonClick(Button sender, Grid listGrid)
        {
            foreach (Grid listItemGrid in listGrid.Children)
            {
                Button listItemButton = (Button)listItemGrid.Children[0];
                listItemButton.Background = (Brush) new BrushConverter().ConvertFrom("#FF878787");
                listItemButton.Foreground = Brushes.White;
            }

            Choice = sender.Content.ToString();
            sender.Background = (Brush) new BrushConverter().ConvertFrom("#FF8FAEEC");
            sender.Foreground = (Brush) new BrushConverter().ConvertFrom("#FF474747");
        }

        private Grid CreateSearchAndListGrid(ScrollViewer listScrollViewer)
        {
            Grid searchGrid = new Grid();
            searchGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            searchGrid.VerticalAlignment = VerticalAlignment.Top;
            searchGrid.Height = 30;
            searchGrid.Background = Brushes.Gray;

            TextBox searchTextBox = new TextBox();
            searchTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            searchTextBox.VerticalAlignment = VerticalAlignment.Stretch;
            searchTextBox.Margin = new Thickness(0,0,50,0);
            searchTextBox.Padding = new Thickness(5,0,5,0);
            searchTextBox.VerticalContentAlignment = VerticalAlignment.Center;
            searchTextBox.Background = (Brush) new BrushConverter().ConvertFrom("#FFA7A7A7");
            searchTextBox.TextChanged += SearchTextChanged;

            Button searchButton = new Button();
            searchButton.HorizontalAlignment = HorizontalAlignment.Right;
            searchButton.VerticalAlignment = VerticalAlignment.Stretch;
            searchButton.Background = (Brush) new BrushConverter().ConvertFrom("#FF4A9C38");
            searchButton.Foreground = (Brush) new BrushConverter().ConvertFrom("#FF474747");
            searchButton.Width = 50;
            searchButton.Content = "Szukaj";
            searchButton.Foreground = Brushes.White;
            searchButton.FontFamily = new FontFamily("Segoe UI");
            searchButton.FontWeight = FontWeights.Bold;
            searchButton.Click += (s, e) => { SearchButtonClick(listScrollViewer); };

            searchGrid.Children.Add(searchTextBox);
            searchGrid.Children.Add(searchButton);

            Grid listGrid = CreateListGrid(mockupMaps);
            listScrollViewer.Content = listGrid;

            return searchGrid;
        }

        private void SearchTextChanged(object sender, EventArgs e)
        {
            TextBox s = (TextBox) sender;
            Keyword = s.Text.ToString();
        }

        private void SearchButtonClick(ScrollViewer listScrollViewer)
        {
            Choice = "";
            Map[] filteredMaps = mockupMaps.Where(m => m.Name.Contains(Keyword)).ToArray();
            Grid listGrid = CreateListGrid(filteredMaps);
            listScrollViewer.Content = listGrid;
        }
    }
}
