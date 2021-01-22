using System.Windows.Controls;
using System.Windows.Input;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class PlacerTileBehaviourSetter : ITileBehaviourSetter
    {
        private readonly Selection _selection;
        private readonly Tile[,] _tileGrid;

        public PlacerTileBehaviourSetter(Selection selection, Tile[,] tileGrid)
        {
            _selection = selection;
            _tileGrid = tileGrid;
        }

        public void OnTileMouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.RightButton == MouseButtonState.Pressed && _selection.TileObject != null) return;
            _selection.ApplyTiles(sender as Button, Keyboard.Modifiers == ModifierKeys.Control);
        }

        public void OnFieldMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            Button button = sender as Button;
            Tile tile = Placer.GetPlacerFromButton(_tileGrid, button) as Tile;
            _selection.UpdateChangedPlacerList(tile, _tileGrid);
        }

        public void OnFieldMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            bool canDrag = !_selection.TileObject.Config.IsElement || _selection.SelectionState == State.Block;
            bool pressed = mouseEventArgs.RightButton == MouseButtonState.Pressed;
            bool control = Keyboard.Modifiers == ModifierKeys.Control;
            if (pressed && canDrag) _selection.ApplyTiles(sender as Button, control);
            else _selection.RedoChanges();
        }
    }
}