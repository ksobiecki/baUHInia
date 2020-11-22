using System.Windows.Controls;

namespace baUHInia.Playground.Model.Tiles
{
    public class Element : Placer
    {
        public Element(Image image) : base(image) { }

        protected override void ApplyTexture()
        {
            ((Image) FrameworkElement).Source = CurrentTexture;
        }
    }
}