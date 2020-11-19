using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace baUHInia.MapLogic.Manager
{
    class GameMapManager : IGameMapManager
    {
        private string choice;

        public GameMapManager()
        {
            this.choice = "";
        }

        public Grid GetGameLoadGrid()
        {
            if (choice.Length == 0) {
                throw new Exception("Invalid game selected.");
            }

            throw new NotImplementedException();
        }

        public Grid GetMapLoadGrid()
        {
            return CreateGrid();
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

        private Grid CreateGrid()
        {
            Grid containerGrid = new Grid();
            containerGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            containerGrid.VerticalAlignment = VerticalAlignment.Stretch;

            Grid searchGrid = new Grid();
            searchGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            searchGrid.VerticalAlignment = VerticalAlignment.Top;
            searchGrid.Height = 40;
            searchGrid.Background = Brushes.Red;

            ScrollViewer listScrollViewer = new ScrollViewer();
            listScrollViewer.HorizontalAlignment = HorizontalAlignment.Stretch;
            listScrollViewer.VerticalAlignment = VerticalAlignment.Stretch;
            listScrollViewer.Margin = new Thickness(0, 40, 0, 0);

            Grid listGrid = new Grid();
            listGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            listGrid.VerticalAlignment = VerticalAlignment.Stretch;
            listGrid.Name = "ListGrid";
            listGrid.Children.Clear();
            listGrid.RowDefinitions.Clear();

            for (int i = 0; i < 40; i++)
            {
                listGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
                Grid listItemGrid = new Grid();
                listItemGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                Button listItemButton = new Button();
                listItemButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                listItemButton.VerticalAlignment = VerticalAlignment.Stretch;
                listItemButton.HorizontalContentAlignment = HorizontalAlignment.Center;
                listItemButton.VerticalContentAlignment = VerticalAlignment.Center;
                listItemButton.Content = "Some map name " + i;
                listItemButton.Click += (s,e) => { listItemButtonClick((Button) s, listGrid); };
                Grid.SetRow(listItemGrid, i);
                listItemGrid.Children.Add(listItemButton);
                listGrid.Children.Add(listItemGrid);
            }

            listScrollViewer.Content = listGrid;
            containerGrid.Children.Add(searchGrid);
            containerGrid.Children.Add(listScrollViewer);

            return containerGrid;
        }

        private void listItemButtonClick(Button sender, Grid listGrid)
        {
            foreach (Grid listItemGrid in listGrid.Children)
            {
                Button listItemButton = (Button) listItemGrid.Children[0];
                listItemButton.Background = Brushes.White;
            }

            sender.Background = Brushes.Red;
        }
    }
}
