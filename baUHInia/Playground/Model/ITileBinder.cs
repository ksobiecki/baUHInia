using System.Collections.Generic;
using System.Windows.Controls;
using baUHInia.Authorisation;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model
{
    public interface ITileBinder
    {
        Selection Selection { get; }
        
        Tile[,] TileGrid { get; }
        
        List<Placement> PlacedObjects { get; }
        
        ScrollViewer GameViewer { get; }
        
        Grid SelectorGrid { get; }
        
        List<GameObject> AvailableObjects { get; }
        
        LoginData Credentials { get; }
        
        int AvailableFounds { get; }
    }
}