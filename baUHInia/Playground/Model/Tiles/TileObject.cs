using System.Windows.Media.Imaging;
using baUHInia.Playground.Logic.Utils;
using baUHInia.Playground.Model.Utility;

namespace baUHInia.Playground.Model.Tiles
{
    public class TileObject
    {
        public readonly (string category, string subCategory) Tag;
        public readonly Sprite Sprite;
        public readonly Config Config;

        public BitmapImage this[string str] => Sprite[str];
        public BitmapImage this[int i] => Sprite[i];
        public string Name => Config.Name;

        public TileObject((string category, string subCategory) tag, Sprite sprite, Config config)
        {
            Tag = tag;
            Sprite = sprite;
            Config = config;
        }
    }
}