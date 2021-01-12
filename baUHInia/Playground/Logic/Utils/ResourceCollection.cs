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
            .Where(str => str.StartsWith(subCategory + '/'))
            .Select(str => str.Remove(0, subCategory.Length + 1))
            .OrderBy(str => str, Comparer<string>.Create(
                (x, y) =>
                {
                    try
                    {
                        int valX = int.Parse(x.Substring(0, x.IndexOf('_')));
                        int valY = int.Parse(y.Substring(0, y.IndexOf('_')));
                        return valX > valY ? 1 : valX == valY ? 0 : -1;
                    }
                    catch (Exception)
                    {
                        return string.CompareOrdinal(x, y);
                    }
                }))
            .ToArray();

        public string GetPossibleConfig(string subCategory) =>
            _configs.FirstOrDefault(str => str.EndsWith('/' + subCategory + ".json"));

        public Dictionary<string, BitmapImage> LoadBitmaps(string ctg, string subCtg, string resDir) =>
            _elementsOfCategory
                .Where(str => str.StartsWith(subCtg + '/'))
                .ToDictionary(
                    path => path.Substring(path.IndexOf('/') + 1),
                    path => new BitmapImage(new Uri(resDir + "resources/" + _package + '/' + ctg + '/' + path))
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