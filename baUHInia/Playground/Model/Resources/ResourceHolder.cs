using System.Collections.Generic;
using baUHInia.Playground.Logic.Loaders;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model.Resources
{
    public class ResourceHolder
    {
        public readonly List<TileCategory> Terrain;
        public readonly List<TileCategory> Structures;
        public readonly List<TileCategory> Foliage;

        private static ResourceHolder _instance;

        private ResourceHolder(List<TileCategory> terrain, List<TileCategory> structures, List<TileCategory> foliage)
        {
            Terrain = terrain;
            Structures = structures;
            Foliage = foliage;
        }

        public static ResourceHolder Get =>
            _instance ?? (_instance = new ResourceHolder(ResourceLoader.LoadTileCategories("terrain"), null, null));
    }
}