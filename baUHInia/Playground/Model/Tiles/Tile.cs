using System.Windows.Controls;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Model.Utility;

namespace baUHInia.Playground.Model.Tiles
{
    public class Tile : Placer
    {
        public Tile(Button button, Offset root) : base(button, root)
        {
            CurrentTexture = ((Image) ((Button) FrameworkElement).Content).Source as BitmapImage;
        }


        protected override void ApplyTexture()
        {
            ((Image) ((Button) FrameworkElement).Content).Source = CurrentTexture;
        }
    }
}