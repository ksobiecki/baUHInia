using System;
using System.Windows;
using System.Windows.Controls;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Selector
{
    public class SelectorGridCreator
    {
        private readonly SelectorCreator _selectorCreator;
        private readonly Grid _selectorGrid;

        public SelectorGridCreator(ITileBinder binder, Grid selectorGrid)
        {
            _selectorCreator = new SelectorCreator(binder.Selection);
            _selectorGrid = selectorGrid;
        }

        public void CreateSelectionPanel(TileCategory tileCategory)
        {
            ClearSelectorGrid();
            int gridRowIndex = 0;

            foreach (TileObject tileObject in tileCategory.TileObjects)
            {
                CreateSubcategory(tileObject, gridRowIndex);
                gridRowIndex += 2;
            }
        }

        private void ClearSelectorGrid()
        {
            _selectorGrid.Children.Clear();
            _selectorGrid.RowDefinitions.Clear();
            _selectorGrid.ColumnDefinitions.Clear();
        }

        private void CreateSubcategory(TileObject tileObject, int gridRowIndex)
        {
            MakeSpaceForAnotherSubcategoryInSelectorGrid();
            AddSubcategoryLabelToSelectorGrid(tileObject.Name, gridRowIndex);
            Grid subGrid;

            if (tileObject.Sprite.Offsets != null)
            {
                (int x, int y) = tileObject.Sprite.SpriteWidthHeight();
                subGrid = AddCategorizedSubGridToSelectorGrid(y, x, gridRowIndex);
                CreateTileObjectInsideSubGrid(subGrid, tileObject);
            }
            else
            {
                subGrid = AddUncategorizedSubGridToSelectorGrid(tileObject.Sprite.Names.Length, gridRowIndex);
                CreateTilesInsideSubGrid(subGrid, tileObject);
            }
        }

        private void MakeSpaceForAnotherSubcategoryInSelectorGrid()
        {
            _selectorGrid.RowDefinitions.Add(new RowDefinition());
            _selectorGrid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
        }

        private void AddSubcategoryLabelToSelectorGrid(string subcategory, int gridRowIndex)
        {
            Label label = new Label {Content = subcategory, Height = 30};
            Grid.SetRow(label, gridRowIndex);
            _selectorGrid.Children.Add(label);
        }

        private Grid AddCategorizedSubGridToSelectorGrid(int height, int width, int gridRowIndex)
        {
            Grid grid = new Grid
            {
                Height = 100,//180 / (height == 0 ? 3 : height + 1), 
                Width = 100//180 / (width == 0 ? 3 : width + 1)
            };
            Grid.SetRow(grid, gridRowIndex + 1);
            DivideGridToAccommodateTileObject(grid, width, height);
            _selectorGrid.Children.Add(grid);
            return grid;
        }

        private Grid AddUncategorizedSubGridToSelectorGrid(int elementsCount, int gridRowIndex)
        {
            Grid grid = new Grid {Height = 40 * Math.Ceiling(elementsCount / 3.0), Width = 120};
            Grid.SetRow(grid, gridRowIndex + 1);
            DivideGridIntoRowsAndColumns(grid, elementsCount);
            _selectorGrid.Children.Add(grid);
            return grid;
        }

        private static void DivideGridToAccommodateTileObject(Grid selectorGrid, int width, int height)
        {
            for (int i = 0; i <= height; i++) selectorGrid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i <= width; i++) selectorGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        private static void DivideGridIntoRowsAndColumns(Grid selectorGrid, int count)
        {
            for (int i = 0; i < 3; i++) selectorGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < Math.Ceiling(count / 3.0); i++) selectorGrid.RowDefinitions.Add(new RowDefinition());
        }

        private void CreateTileObjectInsideSubGrid(Panel subGrid, TileObject tileObject)
        {
            (sbyte x, sbyte y) = tileObject.Sprite.SpriteMinCoordinates();
            for (int i = 0; i < tileObject.Sprite.Offsets.Length; i++)
            {
                string element = tileObject.Sprite.Names[tileObject.Sprite.Offsets[i].I];
                Button button = _selectorCreator.CreateSelectorTile(element, tileObject.Sprite.Bitmaps[element], tileObject.Tag);
                button.Margin = new Thickness(-1);
                button.Padding = new Thickness(-1.2);
                Grid.SetColumn(button, tileObject.Sprite.Offsets[i].X - x);
                Grid.SetRow(button, tileObject.Sprite.Offsets[i].Y - y);
                subGrid.Children.Add(button);
            }
        }

        private void CreateTilesInsideSubGrid(Panel subGrid, TileObject tileCollection)
        {
            int j = 0;
            int i = 0;
            foreach (string element in tileCollection.Sprite.Names)
            {
                if (j != 0 && j % 3 == 0) i++;
                Button button = _selectorCreator.CreateSelectorTile(element, tileCollection.Sprite.Bitmaps[element], tileCollection.Tag);
                Grid.SetRow(button, i);
                Grid.SetColumn(button, j % 3);
                subGrid.Children.Add(button);
                j++;
            }
        }
    }
}