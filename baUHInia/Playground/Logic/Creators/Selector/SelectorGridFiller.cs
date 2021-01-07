using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Utility;

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
                button.Margin = new Thickness(0, 0, 2, 2);
                Grid.SetRow(button, i);
                Grid.SetColumn(button, j % 3);
                _subGrid.Children.Add(button);
                j++;
            }
        }

        public void CreateGameObjectInsideSubGrid(TileObject tileObject)
        {
            (sbyte x, sbyte y) = tileObject.Sprite.SpriteMinCoordinates();
            (_, byte my) = tileObject.Sprite.SpriteWidthHeight();
            foreach (Offset offset in tileObject.Sprite.Offsets)
            {
                string element = tileObject.Sprite.Names[offset.I];
                Button button = _selectorCreator.CreateSelectorTile(element, tileObject[element], tileObject.Tag);
                Grid.SetColumn(button, offset.X - x);
                Grid.SetRow(button, my - offset.Y + y);
                _subGrid.Children.Add(button);
            }
        }
    }
}