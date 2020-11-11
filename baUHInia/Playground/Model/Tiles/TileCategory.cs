using System.Collections.Generic;
using System.Linq;

namespace baUHInia.Playground.Model.Tiles
{
    public class TileCategory
    {
        public readonly string Name;
        public readonly List<TileObject> TileObjects;

        public TileObject this[string str] => TileObjects.First(o => o.Name == str);
        
        public TileCategory(string name, List<TileObject> tileObjects)
        {
            Name = name;
            TileObjects = tileObjects;
        }
    }
}