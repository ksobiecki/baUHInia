using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace baUHInia.Playground.Model.Tiles
{
    public class Tile : Placer
    {
        public Tile(Button button) : base(button)
        {
            CurrentTexture = ((Image) ((Button) FrameworkElement).Content).Source as BitmapImage;
        }


        protected override void ApplyTexture()
        {
            ((Image) ((Button) FrameworkElement).Content).Source = CurrentTexture;
        }
    }
}