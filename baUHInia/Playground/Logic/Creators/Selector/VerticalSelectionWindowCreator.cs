using System;
using System.Windows;
using System.Windows.Controls;
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
            ((Grid) _grid.Children[1]).RowDefinitions.Clear();
            ((Grid) _grid.Children[1]).ColumnDefinitions.Clear();
            ((Grid) _grid.Children[1]).Children.Clear();
        }

        private void PutLabelInGrid(string name)
        {
            ((Label) _grid.Children[0]).Content = name;
        }

        private void DisplayImage(TileObject tileObject)
        {
            (int width, int height) = tileObject.Sprite.SpriteWidthHeight();
            (sbyte x, sbyte y) = tileObject.Sprite.SpriteMinCoordinates();
            Grid subgrid = AddSubGridToSelectorGrid(height, width);

            foreach (Offset offset in tileObject.Sprite.Offsets)
            {
                string element = tileObject.Sprite.Names[offset.I];
                Button button = SelectorCreator.CreateSelector(element, tileObject[element]);
                Grid.SetColumn(button, offset.X - x);
                Grid.SetRow(button, height - offset.Y + y);
                subgrid.Children.Add(button);
            }
        }

        private void PutCostWidthHeightAndStats(GameObject gameObject)
        {
            (byte x, byte y) = gameObject.TileObject.Sprite.SpriteWidthHeight();
            x++;
            y++;

            TextBlock[] blocks =
            {
                (TextBlock) _grid.Children[2],
                (TextBlock) _grid.Children[3],
                (TextBlock) _grid.Children[4],
                (TextBlock) _grid.Children[5]
            };
            string[] strings =
            {
                $"Kwota: {gameObject.Price}",
                $"Zacienienie: {gameObject.ChangeValue}",
                $"Szerokość: {x}",
                $"Wysokość: {y}"
            };

            for (int i = 0; i < strings.Length; i++) blocks[i].Text = strings[i];
        }

        private Grid AddSubGridToSelectorGrid(int height, int width)
        {
            Grid grid = _grid.Children[1] as Grid;
            DivideGridToAccommodateTileObject(grid, width, height);
            return grid;
        }

        private static void DivideGridToAccommodateTileObject(Grid selectorGrid, int width, int height)
        {
            double size = 90.0 / (Math.Max(width, height) + 1);
            
            for (int i = 0; i <= height; i++)
                selectorGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(size),
                    MaxHeight = size
                });
            
            for (int i = 0; i <= width; i++)
                selectorGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(size),
                    MaxWidth = size
                });
        }
    }
}