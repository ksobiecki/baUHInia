using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Authorisation;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Playground.Model
{
    public interface ITileBinder
    {
        Selection Selection { get; }
        
        Tile[,] TileGrid { get; }
        
        List<Placement> PlacedObjects { get; }

        Grid SelectorGrid { get; }
        
        List<GameObject> AvailableObjects { get; }
        
        LoginData Credentials { get; }

        int AvailableFounds { get; set; }

        void ChangeInteractionMode(string text, Brush color);
    }
}