using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace baUHInia.Playground.Model.Tiles
{
    public class Sprite
    {
        public readonly string[] Names;

        public readonly Offset[] Offsets;

        public readonly Dictionary<string, BitmapImage> Bitmaps;

        public BitmapImage this[string str] => Bitmaps[str];

        public BitmapImage this[int i] => Bitmaps[Names[Offsets[i].I]];

        public Sprite(string[] names, Offset[] offsets, Dictionary<string, BitmapImage> bitmaps)
        {
            Names = names;
            Offsets = offsets ?? new[] {new Offset(0, 0, 0)};
            Bitmaps = bitmaps;
        }

        public (sbyte x, sbyte y) SpriteMinCoordinates() => (Offsets.Min(val => val.X), Offsets.Min(val => val.Y));

        public (byte x, byte y) SpriteWidthHeight() => (
            (byte) (Offsets.Max(val => val.X) - Offsets.Min(val => val.X)),
            (byte) (Offsets.Max(val => val.Y) - Offsets.Min(val => val.Y)));
    }
}