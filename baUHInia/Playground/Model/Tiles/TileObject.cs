using System.Windows.Media.Imaging;

namespace baUHInia.Playground.Model.Tiles
{
    public class TileObject
    {
        public readonly (int category, int subCategory) Tag;

        public readonly string Name;

        public readonly string Group;

        public readonly Sprite Sprite;

        public BitmapImage this[string str] => Sprite[str];
        public BitmapImage this[int i] => Sprite[i];
        
        public TileObject(string name, (int category, int subCategory) tag, Sprite sprite)
        {
            Name = name;
            Tag = tag;
            Sprite = sprite;
        }
    }
}