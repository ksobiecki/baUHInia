using baUHInia.MapLogic.Manager;
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

namespace baUHInia.MapLogic.View
{
    public partial class ChoiceWindow : Window
    {
        public bool[] mapList; // Mockup data for debug.
        public ChoiceWindow()
        {
            InitializeComponent();
            IGameMapManager GameMapManager = new GameMapManager();
            Grid target = this.FindName("TargetGrid") as Grid;
            target.Children.Add(GameMapManager.GetMapLoadGrid());
        }

        /*public void RenderList()
        {

            Grid targetGrid = this.FindName("TargetGrid") as Grid;

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

            int index = 0;

            foreach (bool map in mapList)
            {
                listGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
                Grid listItemGrid = new Grid();
                listItemGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                Button listItemButton = new Button();
                listItemButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                listItemButton.VerticalAlignment = VerticalAlignment.Stretch;
                listItemButton.HorizontalContentAlignment = HorizontalAlignment.Center;
                listItemButton.VerticalContentAlignment = VerticalAlignment.Center;
                listItemButton.Content = "Some map name " + index;
                listItemButton.Click += listItemButtonClick;
                Grid.SetRow(listItemGrid, index);
                listItemGrid.Children.Add(listItemButton);
                listGrid.Children.Add(listItemGrid);
                index++;
            }

            listScrollViewer.Content = listGrid;
            containerGrid.Children.Add(searchGrid);
            containerGrid.Children.Add(listScrollViewer);
            targetGrid.Children.Add(containerGrid);
        }

        private void listItemButtonClick(object sender, RoutedEventArgs e)
        {
            Grid listGrid = this.FindName("ListGrid") as Grid;

            foreach (Grid listItemGrid in listGrid.Children)
            {
                Button listItemButton = (Button) listItemGrid.Children[0];
                listItemButton.Background = Brushes.White;
            }

            Button s = (Button) sender;
            s.Background = Brushes.Red;
        */
    }
}
