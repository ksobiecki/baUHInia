using System.Windows.Controls;
using System.Windows.Input;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class GameTileBehaviourSetter : GenericTileBehaviourSetter
    {
        public GameTileBehaviourSetter(Selection selection, Tile[,] tileGrid) : base(selection, tileGrid) { }

        public override void OnTileMouseClick(object sender, MouseEventArgs mouseEventArgs) =>
            Selection.ApplyTiles(sender as Button,Keyboard.Modifiers == ModifierKeys.Control);
        

        public override void OnFieldMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            Button button = sender as Button;
            Tile tile = Placer.GetPlacerFromButton(TileGrid, button) as Tile;
            Selection.UpdateChangedPlacerList(tile, TileGrid);
        }

        public override void OnFieldMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            bool pressed = mouseEventArgs.RightButton == MouseButtonState.Pressed;
            bool control = Keyboard.Modifiers == ModifierKeys.Control;
            bool canDrag = !Selection.TileObject.Config.IsElement || Selection.SelectionState == State.Block;
            if (pressed && canDrag) Selection.ApplyTiles(sender as Button, control);
            else Selection.RedoChanges();
        }
    }
}