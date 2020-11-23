using System.Windows.Controls;
using baUHInia.Playground.Model.Utility;

namespace baUHInia.Playground.Model.Tiles
{
    public class Element : Placer
    {
        public Element(Image image, Offset root) : base(image, root) { }

        public void MarkAsRemovable()
        {
            ((Image) FrameworkElement).Opacity = 0.6;
        }

        public void UnmarkAsRemovable()
        {
            ((Image) FrameworkElement).Opacity = 1.0;
        }
        
        protected override void ApplyTexture()
        {
            ((Image) FrameworkElement).Source = CurrentTexture;
        }
    }
}