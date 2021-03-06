﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Selector
{
    public class PlacerSelectorGridCreator : ISelectorGridCreator
    {
        private readonly SelectorCreator _selectorCreator;
        private SelectorGridFiller _selectorGridFiller;

        private readonly Grid _selectorGrid;

        public PlacerSelectorGridCreator(ITileBinder binder, List<TileCategory> categories)
        {
            _selectorCreator = new SelectorCreator(binder.Selection, categories);
            _selectorGrid = binder.SelectorGrid;
            _selectorGrid.HorizontalAlignment = HorizontalAlignment.Center;
        }

        public void CreateSelectionPanel(TileCategory tileCategory)
        {
            ClearSelectorGrid();
            int gridRowIndex = 0;

            IEnumerable<string> groups = GetGroups(tileCategory);
            IEnumerable<TileObject> otherObjects = GetStandaloneObjects(tileCategory);
            CreateGroups(groups, tileCategory.TileObjects, ref gridRowIndex);
            CreateStandaloneObjects(otherObjects, ref gridRowIndex);
        }
        public void UpdateTileGroup(List<TileCategory> categories) => _selectorCreator.UpdateTileGroup(categories);

        private void CreateGroups(IEnumerable<string> groups, List<TileObject> tileObjects, ref int rowIndex)
        {
            foreach (string group in groups)
            {
                TileObject[] objects = GetElementsOfGroup(tileObjects, group);
                CreateElementHeader(group, rowIndex);
                int gridHeight = (objects.Length - 1) / 3;
                Grid selectorGrid = AddSubGridToSelectorGrid(gridHeight, 2, rowIndex);
                _selectorGridFiller = new SelectorGridFiller(selectorGrid, _selectorCreator);
                _selectorGridFiller.CreateTilesInsideSubGrid(objects);
                rowIndex += 2;
            }
        }

        private void CreateStandaloneObjects(IEnumerable<TileObject> otherObjects, ref int gridRowIndex)
        {
            foreach (TileObject otherObject in otherObjects)
            {
                CreateElementHeader(otherObject.Name, gridRowIndex);
                (int x, int y) = otherObject.Sprite.SpriteWidthHeight();
                Grid selectorGrid = AddSubGridToSelectorGrid(y, x, gridRowIndex);
                _selectorGridFiller = new SelectorGridFiller(selectorGrid, _selectorCreator);
                _selectorGridFiller.CreateGameObjectInsideSubGrid(otherObject);
                gridRowIndex += 2;
            }
        }

        private void ClearSelectorGrid()
        {
            _selectorGrid.Children.Clear();
            _selectorGrid.RowDefinitions.Clear();
            _selectorGrid.ColumnDefinitions.Clear();
        }

        private void CreateElementHeader(string subcategory, int gridRowIndex)
        {
            _selectorGrid.RowDefinitions.Add(new RowDefinition());
            _selectorGrid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
            Label label = new Label
            {
                Content = subcategory, Height = 30, FontSize = 13, FontWeight = FontWeights.Bold,
                Foreground = Brushes.DarkSlateGray
            };
            Grid.SetRow(label, gridRowIndex);
            _selectorGrid.Children.Add(label);
        }

        private Grid AddSubGridToSelectorGrid(int height, int width, int gridRowIndex)
        {
            Grid grid = new Grid {Margin = new Thickness(10, 0, 10, 0)};
            Grid.SetRow(grid, gridRowIndex + 1);
            DivideGridToAccommodateTileObject(grid, width, height);

            _selectorGrid.Children.Add(grid);
            return grid;
        }

        private static void DivideGridToAccommodateTileObject(Grid selectorGrid, int width, int height)
        {
            for (int i = 0; i <= height; i++) selectorGrid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i <= width; i++) selectorGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        
        //===================================== HELPERS =====================================//

        private static TileObject[] GetElementsOfGroup(IEnumerable<TileObject> tileObjects, string group) => tileObjects
            .Where(o => o.Config.Group == group)
            .ToArray();
        
        private static IEnumerable<string> GetGroups(TileCategory tileCategory) => tileCategory.TileObjects
            .Where(o => o.Config.Group != null)
            .Select(o => o.Config.Group)
            .Distinct()
            .ToArray();

        private static IEnumerable<TileObject> GetStandaloneObjects(TileCategory tileCategory) => 
            tileCategory.TileObjects
                .Where(o => o.Config.Group == null)
                .ToArray();
    }
}