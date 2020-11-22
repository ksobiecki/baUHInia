using System.Windows.Media.Imaging;
using baUHInia.Playground.Logic.Utils;

namespace baUHInia.Playground.Model.Tiles
{
    public class TileObject
    {
        public readonly (int category, int subCategory) Tag;
        public readonly Sprite Sprite;
        public readonly Config Config;

        public BitmapImage this[string str] => Sprite[str];
        public BitmapImage this[int i] => Sprite[i];
        public string Name => Config.Name;

        public TileObject((int category, int subCategory) tag, Sprite sprite, Config config)
        {
            Tag = tag;
            Sprite = sprite;
            Config = config;
        }
    }
}