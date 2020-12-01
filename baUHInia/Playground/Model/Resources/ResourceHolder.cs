using System.Collections.Generic;
using System.Linq;
using baUHInia.Playground.Logic.Loaders;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model.Resources
{
    public class ResourceHolder
    {
        public readonly List<TileCategory> Terrain;
        public readonly List<TileCategory> Structures;
        public readonly List<TileCategory> Foliage;

        private ResourceType CurrentType { get; set; }

        private static ResourceHolder _instance;

        private ResourceHolder(List<TileCategory> terrain, List<TileCategory> structures, List<TileCategory> foliage)
        {
            Terrain = terrain;
            Structures = structures;
            Foliage = foliage;
        }

        public static ResourceHolder Get =>
            _instance ?? (_instance = new ResourceHolder(ResourceLoader.LoadTileCategories("terrain"), null, null));

        public void ChangeResourceType(ResourceType type) => CurrentType = type;

        public TileCategory GetCategoryByName(string name)
        {
            switch (CurrentType)
            {
                case ResourceType.Terrain: return GetSpecificCategory(Terrain, name);
                case ResourceType.Structure: return GetSpecificCategory(Structures, name); 
                default: return GetSpecificCategory(Foliage, name);
            }
        }
        
        public IEnumerable<string> GetCategoryName()
        {
            switch (CurrentType)
            {
                case ResourceType.Terrain: return GetCategoryNames(Get.Terrain);
                case ResourceType.Structure: return GetCategoryNames(Get.Structures);
                default: return GetCategoryNames(Get.Foliage);
            }
        }

        public TileCategory GetFirstTileCategory()
        {
            TileCategory nullPlaceholder = new TileCategory("", new List<TileObject>());
            switch (CurrentType)
            {
                case ResourceType.Terrain: return Get.Terrain?[0] ?? nullPlaceholder;
                case ResourceType.Structure: return Get.Structures?[0] ?? nullPlaceholder;
                default: return Get.Foliage?[0] ?? nullPlaceholder;
            }
        }

        public TileObject GetTerrainTileObject(string name) => FindInGroup(Terrain, name);

        public TileObject GetPlaceableTileObject(string name)
        {
            return FindInGroup(Structures, name) ?? FindInGroup(Foliage, name);
        }

        private static TileCategory GetSpecificCategory(IEnumerable<TileCategory> categories, string name) =>
            categories?.First(c => c.Name == name.ToLower()) ?? new TileCategory("", new List<TileObject>());

        private static IEnumerable<string> GetCategoryNames(IEnumerable<TileCategory> categories) =>
            categories?.Select(c => char.ToUpper(c.Name[0]) + c.Name.Substring(1));

        private static TileObject FindInGroup(IEnumerable<TileCategory> pack, string name) => (
            from category in pack
            from tile in category.TileObjects
            where tile.Name == name
            select tile).FirstOrDefault();
    }
}

// WYWOŁANIE    ResourceHolder.Get.GetPlaceableTileObject(name);