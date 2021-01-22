using System.Collections.Generic;

namespace baUHInia.Playground.Model.Tiles
{
    public class TileCategory
    {
        public readonly string Name;
        public readonly List<TileObject> TileObjects;

        public TileCategory(string name, List<TileObject> tileObjects)
        {
            Name = name;
            TileObjects = tileObjects;
        }
    }
}