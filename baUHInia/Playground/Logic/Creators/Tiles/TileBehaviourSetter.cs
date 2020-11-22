using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class TileBehaviourSetter
    {
        private readonly Selection _selection;
        private readonly Tile[,] _tileGrid;

        public TileBehaviourSetter(Selection selection, Tile[,] tileGrid)
        {
            _selection = selection;
            _tileGrid = tileGrid;
        }

        //============================ TILE BEHAVIOUR ============================//

        public void OnTileMouseClick(object sender, RoutedEventArgs routedEventArgs) =>
            _selection.ApplyTiles();
        
        public void OnFieldMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            Button button = sender as Button;
            Tile tile = Placer.GetPlacerFromButton(_tileGrid, button) as Tile;
            _selection.UpdateChangedPlacerList(tile, _tileGrid);
        }
        
        public void OnFieldMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            bool pressed = mouseEventArgs.RightButton == MouseButtonState.Pressed;
            if (pressed) _selection.ApplyTiles();
            else _selection.RedoChanges();
        }
    }
}