﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Model.Utility;

namespace baUHInia.Playground.Model.Tiles
{
    public class Tile : Placer
    {
        public Tile(FrameworkElement button, Offset root) : base(button, root) =>
            CurrentTexture = ((Image) ((Button) FrameworkElement).Content).Source as BitmapImage;
        
        
        public void ShowIfAvailable(double opacity, Brush yes, Brush no)
        {
            if (!(FrameworkElement is Button button)) return;
            ((Image) button.Content).Opacity = opacity;
            button.Background = Placeable ? yes : no;
        }

        protected override void ApplyTexture() =>
            ((Image) ((Button) FrameworkElement).Content).Source = CurrentTexture;
    }
}