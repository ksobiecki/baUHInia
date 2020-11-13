using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace baUHInia.Playground.Logic.Utils
{
    public class ResourceCollection
    {
        public readonly string[] Categories;
        private readonly string[] _configs;
        private readonly string[] _imageNames;

        private string[] _elementsOfCategory;
        private string _package;

        public ResourceCollection(string[] resources, string package)
        {
            _package = package;
            resources = resources
                .Where(s => s.StartsWith("resources/" + _package))
                .Select(s => s.Substring(s.IndexOf('/') + 1))
                .ToArray();
            _configs = StringUtils.GetConfigs(resources);
            _imageNames = StringUtils.GetImageNames(resources);
            Categories = StringUtils.GetCategories(_imageNames);
        }

        public string[] SubCategories(int categoryIndex)
        {
            UpdateElementsOfCategory(categoryIndex);
            return _elementsOfCategory
                .Select(str => str.Substring(0, str.IndexOf('/')))
                .Distinct()
                .ToArray();
        }

        public string[] ElementsOfSubcategory(string subCategory) => _elementsOfCategory
            .Where(str => str.StartsWith(subCategory))
            .Select(str => str.Remove(0, subCategory.Length + 1))
            .ToArray();

        public string GetPossibleConfig(string subCategory) =>
            _configs.FirstOrDefault(str => str.Contains(subCategory));

        public Dictionary<string, BitmapImage> LoadBitmaps(string category, string subCategory, string resourceDir) =>
            _elementsOfCategory
                .Where(str => str.StartsWith(subCategory + '/'))
                .ToDictionary(
                    path => path.Substring(path.IndexOf('/') + 1),
                    path => new BitmapImage(new Uri(resourceDir + "resources/" + _package + '/' + category + '/' + path))
                );

        private void UpdateElementsOfCategory(int categoryIndex)
        {
            string category = Categories[categoryIndex];
            _elementsOfCategory = _imageNames
                .Where(str => str.StartsWith(category))
                .Select(str => str.Remove(0, category.Length + 1))
                .ToArray();
        }
    }
}