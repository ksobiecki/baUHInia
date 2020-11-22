using System.Windows;
using System.Windows.Input;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class MapTileBehaviourSetter : GenericTileBehaviourSetter
    {
        public MapTileBehaviourSetter(Selection selection, Tile[,] tileGrid) : base(selection, tileGrid) { }
        
        public override void OnTileMouseClick(object sender, RoutedEventArgs routedEventArgs)
        { 
            Selection.ApplyTiles();
        }

        public override void OnFieldMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            throw new System.NotImplementedException();
        }

        public override void OnFieldMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            throw new System.NotImplementedException();
        }
    }
}