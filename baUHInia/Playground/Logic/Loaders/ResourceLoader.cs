using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Logic.Utils;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Loaders
{
    public static class ResourceLoader
    {
        private const string ResourceDir = "pack://application:,,,/";

        public static List<TileCategory> LoadTileCategories(string package)
        {
            ResourceCollection resources = new ResourceCollection(GetResourceNames(), package);
            List<TileCategory> tileCategories = new List<TileCategory>();
            for (int i = 0; i < resources.Categories.Length; i++)
            {
                List<TileObject> tileObjects = LoadTileObjects(resources, i);
                tileCategories.Add(new TileCategory(resources.Categories[i], tileObjects));
            }

            return tileCategories;
        }

        private static List<TileObject> LoadTileObjects(ResourceCollection resources, int index)
        {
            List<TileObject> tileObjects = new List<TileObject>();
            string[] subCategories = resources.SubCategories(index);
            for (int i = 0; i < subCategories.Length; i++)
            {
                string subCategory = subCategories[i];
                string[] elementsOfSubCategory = resources.ElementsOfSubcategory(subCategory);
                string config = resources.GetPossibleConfig(subCategory);
                Offset[] offsets = LoadOffsets(config);

                Dictionary<string, BitmapImage> bitmaps = resources.LoadBitmaps(
                    resources.Categories[index], subCategory, ResourceDir
                );
                Sprite sprite = new Sprite(elementsOfSubCategory, offsets, bitmaps);
                TileObject tileObject = new TileObject(subCategory, (index, i), sprite);
                tileObjects.Add(tileObject);
            }

            return tileObjects;
        }

        private static Offset[] LoadOffsets(string config)
        {
            if (config == null) return null;
            TileConfigReader tileConfigReader = new TileConfigReader(config);
            return tileConfigReader.ReadTileIndexesWithOffsets();
        }
        
        private static string[] GetResourceNames()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            string resName = assembly.GetName().Name + ".g.resources";
            using (Stream stream = assembly.GetManifestResourceStream(resName))
            using (ResourceReader reader = new ResourceReader(stream ?? throw new NullReferenceException()))
            {
                return reader.Cast<DictionaryEntry>().Select(entry => (string) entry.Key).ToArray();
            }
        }
    }
}