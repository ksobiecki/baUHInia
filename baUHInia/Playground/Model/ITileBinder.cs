using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model
{
    public interface ITileBinder
    {
        Selection Selection { get; }
        
        Tile[,] TileGrid { get; }
    }
}