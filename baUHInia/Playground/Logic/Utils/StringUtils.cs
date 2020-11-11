using System.Linq;

namespace baUHInia.Playground.Logic.Utils
{
    public static class StringUtils
    {
        public static string[] GetConfigs(string[] resources) =>
            resources.Where(str => str.EndsWith(".txt")).ToArray();

        public static string[] GetImageNames(string[] resources) => resources
            .Where(str => str.EndsWith(".png"))
            .OrderBy(str => str)
            .Select(str => str.Substring(str.IndexOf('/') + 1))
            .ToArray();

        public static string[] GetCategories(string[] imageNames) => imageNames
            .Select(str => str.Substring(0, str.IndexOf('/')))
            .Distinct()
            .ToArray();
    }
}