﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Selector
{
    public class SelectorGridFiller
    {
        private readonly Panel _subGrid;
        private readonly SelectorCreator _selectorCreator;

        public SelectorGridFiller(Panel subGrid, SelectorCreator selectorCreator)
        {
            _subGrid = subGrid;
            _selectorCreator = selectorCreator;
        }

        public void CreateTilesInsideSubGrid(TileObject[] tileObjects)
        {
            int j = 0;
            int i = 0;
            foreach (TileObject tileObject in tileObjects)
            {
                BitmapImage bitmap = tileObject[tileObject.Sprite.Names[0]];
                if (j != 0 && j % 3 == 0) i++;
                Button button = _selectorCreator.CreateSelectorTile(tileObject.Name, bitmap, tileObject.Tag);
                Grid.SetRow(button, i);
                Grid.SetColumn(button, j % 3);
                _subGrid.Children.Add(button);
                j++;
            }
        }

        public void CreateGameObjectInsideSubGrid(TileObject tileObject)
        {
            (sbyte x, sbyte y) = tileObject.Sprite.SpriteMinCoordinates();
            foreach (Offset offset in tileObject.Sprite.Offsets)
            {
                string element = tileObject.Sprite.Names[offset.I];
                //TODO: change
                Button button =
                    _selectorCreator.CreateSelectorTile(element, tileObject.Sprite.Bitmaps[element], tileObject.Tag);
                button.Margin = new Thickness(-1);
                button.Padding = new Thickness(-1.2);
                Grid.SetColumn(button, offset.X - x);
                Grid.SetRow(button, offset.Y - y);
                _subGrid.Children.Add(button);
            }
        }
    }
}