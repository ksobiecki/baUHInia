﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class TileCreator
    {
        public Grid GameGrid { get; set; }
        private readonly ITileBehaviourSetter _behaviourSetter;

        public TileCreator(Selection selection, Tile[,] tileGrid) =>
            _behaviourSetter = new PlacerTileBehaviourSetter(selection, tileGrid);

        //============================ COMPLEX TILE CREATION ============================//

        public Tile CreateBehavioralTileInGameGrid(int xPos, int yPos, TileObject tileObject)
        {
            Button button = CreateButton(tileObject.Name, tileObject[0]);
            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.VerticalAlignment = VerticalAlignment.Stretch;
            button.Style = Application.Current.FindResource("MenuButton") as Style;
            ApplyBehaviourToTile(button);
            Tile tile = new Tile(button, null);
            tile.SetPositionInGrid(GameGrid, xPos, yPos);
            return tile;
        }

        //============================ TILE CREATION ============================//

        private static Button CreateButton(string elementName, ImageSource image)
        {
            Image imageWrapper = new Image {Source = image};
            RenderOptions.SetBitmapScalingMode(imageWrapper, BitmapScalingMode.NearestNeighbor);
            
            return new Button
            {
                Content = imageWrapper,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = new Thickness(-0.59, 0, -0.59, 0),
                Padding = new Thickness(-1.2),
                Tag = elementName
            };
        }

        private void ApplyBehaviourToTile(UIElement button)
        {
            button.MouseEnter += _behaviourSetter.OnFieldMouseEnter;
            button.MouseLeave += _behaviourSetter.OnFieldMouseLeave;
            button.PreviewMouseUp += _behaviourSetter.OnTileMouseClick;
            button.PreviewMouseDown += _behaviourSetter.OnTileMouseClick;
        }
    }
}