using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using baUHInia.Playground.Logic.Utils;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Utility;

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

            HasNameDuplicates(tileCategories);
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
                if (config == null) throw new FileNotFoundException($"All tiles must have json file: {subCategory}");
                Config configInstance = LoadConfig(config);
                Dictionary<string, BitmapImage> bitmaps = resources.LoadBitmaps(
                    resources.Categories[index], subCategory, ResourceDir
                );
                Sprite sprite = new Sprite(elementsOfSubCategory, configInstance?.Offsets, bitmaps);
                TileObject tileObject =
                    new TileObject((resources.Categories[index], subCategory), sprite, configInstance);
                tileObjects.Add(tileObject);
            }

            return tileObjects;
        }

        private static Config LoadConfig(string config)
        {
            //TODO: refactor extract to other class
            string uri = ResourceDir + "resources/" + config;
            StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri(uri));
            using (StreamReader reader = new StreamReader(resourceStream.Stream))
            {
                string file = reader.ReadToEnd();
                Config configInstance = JsonSerializer.Deserialize<Config>(file);
                return configInstance;
            }
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

        private static void HasNameDuplicates(IEnumerable<TileCategory> tileCategories)
        {
            List<string> configs = (
                from tileCategory in tileCategories
                from tileObject in tileCategory.TileObjects
                select tileObject.Config.Name).ToList();

            var groups = configs.GroupBy(s => s).Where(g => g.Count() > 1).ToArray();
            if (groups.Length != 0)
            {
                throw new DuplicateNameException($"There can't be 2 elements with same name: {groups[0].Key}");
            }
        }
    }
}