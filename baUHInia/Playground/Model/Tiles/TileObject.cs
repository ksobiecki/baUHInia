using System.Windows.Media.Imaging;
using baUHInia.Playground.Logic.Utils;

namespace baUHInia.Playground.Model.Tiles
{
    public class TileObject
    {
        public readonly (int category, int subCategory) Tag;
        
        public readonly string Name;
        
        public readonly Sprite Sprite;

        public readonly Config Config;
        

        public BitmapImage this[string str] => Sprite[str];
        public BitmapImage this[int i] => Sprite[i];
        
        public TileObject(string name, (int category, int subCategory) tag, Sprite sprite, Config config)
        {
            Name = name;
            Tag = tag;
            Sprite = sprite;
            Config = config;
        }
    }
}