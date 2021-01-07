using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Utility;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Playground.Logic.Creators.Selector
{
    public class VerticalSelectionWindowCreator : ISelectionWindowCreator
    {
        private Grid _grid;

        public void UpdateSelectionWindow(Grid displayGrid, GameObject gameObject)
        {
            _grid = displayGrid;
            ResetGrid();
            PutLabelInGrid(gameObject.TileObject.Name);
            DisplayImage(gameObject.TileObject);
            PutCostWidthHeightAndStats(gameObject);
        }

        private void ResetGrid()
        {
            _grid.Children.Clear();
            _grid.RowDefinitions.Clear();
            _grid.ColumnDefinitions.Clear();
        }

        private void PutLabelInGrid(string name)
        {
            _grid.RowDefinitions.Add(new RowDefinition());
            _grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

            Label label = new Label
            {
                Content = name, Height = 50, FontSize = 13, FontWeight = FontWeights.Bold,
                Foreground = Brushes.Azure, HorizontalAlignment = HorizontalAlignment.Center
            };
            _grid.Children.Add(label);
        }

        private void DisplayImage(TileObject tileObject)
        {
            (int width, int height) = tileObject.Sprite.SpriteWidthHeight();
            (sbyte x, sbyte y) = tileObject.Sprite.SpriteMinCoordinates();
            Grid subgrid = AddSubGridToSelectorGrid(height, width);
            subgrid.Margin = new Thickness(0, 10, 0, 10);

            foreach (Offset offset in tileObject.Sprite.Offsets)
            {
                string element = tileObject.Sprite.Names[offset.I];
                Button button = SelectorCreator.CreateSelector(element, tileObject[element]);
                button.Margin = new Thickness(-1, 0, -1, 0);
                button.Padding = new Thickness(-1.1);
                Grid.SetColumn(button, offset.X - x);
                Grid.SetRow(button, height - offset.Y + y);
                subgrid.Children.Add(button);
            }
        }

        private void PutCostWidthHeightAndStats(GameObject gameObject)
        {
            (byte x, byte y) tuple = gameObject.TileObject.Sprite.SpriteWidthHeight();
            tuple.x++;
            tuple.y++;

            string[] strings =
            {
                $"Kwota: {gameObject.Price}",
                $"Zacienienie: {gameObject.ChangeValue}",
                $"Szerokość: {tuple.x}",
                $"Wysokość: {tuple.y}"
            };

            for (int i = 0; i < strings.Length; i++)
            {
                TextBlock label = new TextBlock
                {
                    Text = strings[i], Height = 20, FontSize = 13, FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Azure, HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                _grid.RowDefinitions.Add(new RowDefinition());
                Grid.SetRow(label, i + 2);
                _grid.Children.Add(label);
            }
        }

        private Grid AddSubGridToSelectorGrid(int height, int width)
        {
            Grid grid = new Grid {Height = width == height ? 100 : (height + 1) * 33.33, Width = 100};
            Grid.SetRow(grid, 1);
            DivideGridToAccommodateTileObject(grid, width, height);
            _grid.Children.Add(grid);
            return grid;
        }

        private static void DivideGridToAccommodateTileObject(Grid selectorGrid, int width, int height)
        {
            for (int i = 0; i <= height; i++) selectorGrid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i <= width; i++) selectorGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }
    }
}