using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Logic.Loaders;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model.Resources
{
    public class ResourceHolder
    {
        public readonly List<TileCategory> Terrain;
        private List<TileCategory> Structures { get; }
        private List<TileCategory> Foliage { get; }

        private static ResourceHolder _instance;
        public ResourceType CurrentType { get; private set; } = ResourceType.Terrain;

        private ResourceHolder(List<TileCategory> terrain, List<TileCategory> structures, List<TileCategory> foliage)
        {
            Terrain = terrain;
            Structures = structures;
            Foliage = foliage;
        }

        public static ResourceHolder Get => _instance ?? InitializeInstance();
        
        public void ChangeResourceType(ResourceType type) => CurrentType = type;

        public List<TileCategory> GetSelectedCategories()
        {
            switch (CurrentType)
            {
                case ResourceType.Terrain: return Terrain;
                case ResourceType.Structure: return Structures; 
                default: return Foliage;
            }
        }

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
                case ResourceType.Terrain: return Get.Terrain.ElementAtOrDefault(0) ?? nullPlaceholder;
                case ResourceType.Structure: return Get.Structures.ElementAtOrDefault(0) ?? nullPlaceholder;
                default: return Get.Foliage.ElementAtOrDefault(0) ?? nullPlaceholder;
            }
        }

        public TileObject GetTerrainTileObject(string name) => FindInGroup(Terrain, name);

        public (TileObject, BitmapImage) GetTerrainPair(string imagePath)
        {
            TileObject tileObject = (from category in Terrain
                from tile in category.TileObjects
                where tile.Sprite.Bitmaps.ContainsKey(imagePath)
                select tile).FirstOrDefault();
            BitmapImage bitmapImage = tileObject[imagePath];
            return (tileObject, bitmapImage);
        }

        public TileObject GetPlaceableTileObject(string name)
        {
            return FindInGroup(Structures, name) ?? FindInGroup(Foliage, name);
        }

        private static ResourceHolder InitializeInstance()
        {
            List<TileCategory> terrain = ResourceLoader.LoadTileCategories("terrain");
            List<TileCategory> structures = ResourceLoader.LoadTileCategories("structures");
            List<TileCategory> foliage = ResourceLoader.LoadTileCategories("foliage");
            _instance = new ResourceHolder(terrain, structures, foliage);
            return _instance;
        }

        private static TileCategory GetSpecificCategory(IEnumerable<TileCategory> categories, string name)
        {
            return categories.FirstOrDefault(c => c.Name == name.ToLower()) ??
                   new TileCategory("", new List<TileObject>());
        }

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