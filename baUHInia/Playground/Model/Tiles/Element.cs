using System.Windows;
using System.Windows.Controls;
using baUHInia.Playground.Model.Utility;

namespace baUHInia.Playground.Model.Tiles
{
    public class Element : Placer
    {
        public Element(FrameworkElement image, Offset root) : base(image, root) { }

        public void MarkAsRemovable(double opacity) => ((Image) FrameworkElement).Opacity = opacity;

        protected override void ApplyTexture() => ((Image) FrameworkElement).Source = CurrentTexture;
    }
}