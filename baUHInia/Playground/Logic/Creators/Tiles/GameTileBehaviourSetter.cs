using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class GameTileBehaviourSetter : GenericTileBehaviourSetter
    {
        public GameTileBehaviourSetter(Selection selection, Tile[,] tileGrid) : base(selection, tileGrid) { }

        public override void OnTileMouseClick(object sender, RoutedEventArgs routedEventArgs)=>
            Selection.ApplyTiles();

        public override void OnFieldMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            Button button = sender as Button;
            Tile tile = Placer.GetPlacerFromButton(TileGrid, button) as Tile;
            Selection.UpdateChangedPlacerList(tile, TileGrid);
        }
        
        public override void OnFieldMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            bool pressed = mouseEventArgs.RightButton == MouseButtonState.Pressed;
            if (pressed) Selection.ApplyTiles();
            else Selection.RedoChanges();
        }
    }
}