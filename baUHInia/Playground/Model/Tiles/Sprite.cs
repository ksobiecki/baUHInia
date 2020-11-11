using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace baUHInia.Playground.Model.Tiles
{
    public class Sprite
    {
        public const string Grass = "terrain/grass/grass.png";

        public readonly string[] Names;

        public readonly (byte i, sbyte x, sbyte y)[] Offsets;

        public readonly Dictionary<string, BitmapImage> Bitmaps;

        public BitmapImage this[string str] => Bitmaps[str];

        public BitmapImage this[int i] => Bitmaps[Names[Offsets[i].i]];

        public Sprite(string[] names, (byte i, sbyte x, sbyte y)[] offsets, Dictionary<string, BitmapImage> bitmaps)
        {
            Names = names;
            Offsets = offsets ?? new[] {((byte) 0, (sbyte) 0, (sbyte) 0)};
            Bitmaps = bitmaps;
        }

        public (sbyte x, sbyte y) SpriteMinCoordinates() => (Offsets.Min(val => val.x), Offsets.Min(val => val.y));

        public (byte x, byte y) SpriteWidthHeight() => (
            (byte) (Offsets.Max(val => val.x) - Offsets.Min(val => val.x)),
            (byte) (Offsets.Max(val => val.y) - Offsets.Min(val => val.y)));
        
        public string FullNameAtIndex(int i)
        {
            string path = this[i].ToString();
            return path.Substring(path.IndexOf("resources/", StringComparison.Ordinal) + 10);
        } 
    }
}