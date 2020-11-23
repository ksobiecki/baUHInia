using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Model.Utility;

namespace baUHInia.Playground.Model.Tiles
{
    public abstract class Placer
    {
        public Offset Root { get; set; }
        public TileObject TileObject { get; set; }
            
        private bool _changed;
        private string _previousTag;
        private string _currentTag;
        private BitmapImage _previousTexture;
        protected BitmapImage CurrentTexture;
        protected readonly FrameworkElement FrameworkElement;

        protected Placer(FrameworkElement frameworkElement, Offset root)
        {
            FrameworkElement = frameworkElement;
            Root = root;
        }

        public string GetName() => _currentTag;

        public FrameworkElement GetUIElement() => FrameworkElement;

        protected abstract void ApplyTexture();

        public void SetPositionInGrid(Grid grid, int xPos, int yPos)
        {
            Grid.SetRow(FrameworkElement, yPos);
            Grid.SetColumn(FrameworkElement, xPos);
            grid.Children.Add(FrameworkElement);
        }

        public void Change(BitmapImage newTexture, string newTag)
        {
            _changed = false;
            _previousTexture = CurrentTexture;
            CurrentTexture = newTexture;
            _previousTag = _currentTag;
            _currentTag = newTag;
            ApplyTexture();
        }

        public void AcceptChange() => _changed = true;

        public void RevertChange()
        {
            if (_changed) return;
            CurrentTexture = _previousTexture;
            _previousTexture = null;
            _currentTag = _previousTag;
            _previousTag = "";
            ApplyTexture();
        }

        public Grid GetParentGrid() => FrameworkElement.Parent as Grid;

        public (int x, int y) GetCoords() => (Grid.GetColumn(FrameworkElement), Grid.GetRow(FrameworkElement));

        public static Placer GetPlacerFromButton(Tile[,] tilesGrid, Button button)
        {
            (int x, int y) = (Grid.GetColumn(button), Grid.GetRow(button));
            return tilesGrid[y, x];
        }
    }
}