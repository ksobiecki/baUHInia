using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class TileCreator
    {
        public Grid GameGrid { get; set; }
        private readonly TileBehaviourSetter _behaviourSetter;

        public TileCreator(Selection selection, Tile[,] tileGrid)
        {
            //TODO: check
            _behaviourSetter = new TileBehaviourSetter(selection, tileGrid);
        }

        //============================ COMPLEX TILE CREATION ============================//

        public Tile CreateBehavioralTileInGameGrid(int xPos, int yPos, BitmapImage image)
        {
            //TODO: Change
            TileObject to = ResourceHolder.Get.Terrain.First(c => c.Name == "terrain")["Plain Grass"];
            Button button = CreateButton(to.Name, to["grass.png"]);//image);//
            ApplyBehaviourToTile(button);
            Tile tile = new Tile(button);
            tile.SetPositionInGrid(GameGrid, xPos, yPos);
            return tile;
        }

        //============================ TILE CREATION ============================//

        private static Button CreateButton(string elementName, BitmapImage image) => new Button
        {
            Content = new Image {Source = image},
            Background = Brushes.Transparent,
            BorderBrush = Brushes.Transparent,
            Margin = new Thickness(-1, 0, -1, 0),
            Padding = new Thickness(-1.2),
            Tag = elementName
            //BorderThickness = new Thickness(0.0),
        };

        private void ApplyBehaviourToTile(Button button)
        {
            button.MouseEnter += _behaviourSetter.OnFieldMouseEnter;
            button.MouseLeave += _behaviourSetter.OnFieldMouseLeave;
            button.MouseDown += _behaviourSetter.OnTileMouseClick;
            button.MouseUp += _behaviourSetter.OnTileMouseClick;
            button.Click += _behaviourSetter.OnTileMouseClick;
        }
    }
}