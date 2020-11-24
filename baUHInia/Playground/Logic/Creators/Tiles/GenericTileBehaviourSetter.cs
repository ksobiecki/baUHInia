using System.Windows.Input;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public abstract class GenericTileBehaviourSetter : ITileBehaviourSetter
    {
        protected readonly Selection Selection;
        protected readonly Tile[,] TileGrid;

        protected GenericTileBehaviourSetter(Selection selection, Tile[,] tileGrid)
        {
            Selection = selection;
            TileGrid = tileGrid;
        }

        public abstract void OnTileMouseClick(object sender, MouseEventArgs mouseEventArgs);
        public abstract void OnFieldMouseEnter(object sender, MouseEventArgs mouseEventArgs);
        public abstract void OnFieldMouseLeave(object sender, MouseEventArgs mouseEventArgs);
    }
}