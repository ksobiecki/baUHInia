using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace baUHInia.Playground.Model.Tiles
{
    public class Tile
    {
        private readonly Button _button;
        private bool _changed;
        private string _currentTag;
        private string _previousTag;
        private BitmapImage _currentTexture;
        private BitmapImage _previousTexture;

        public Tile(Button button)
        {
            _button = button;
            _currentTexture = ((Image) _button.Content).Source as BitmapImage;
        }

        public string GetCategorySubcategoryAndName() => _currentTag;

        public void SetPositionInGrid(Grid grid, int xPos, int yPos)
        {
            Grid.SetRow(_button, yPos);
            Grid.SetColumn(_button, xPos);
            grid.Children.Add(_button);
        }

        public void Change(BitmapImage newTexture, string newTag)
        {
            _changed = false;
            _previousTexture = _currentTexture;
            _currentTexture = newTexture;
            _previousTag = _currentTag;
            _currentTag = newTag;
            ((Image) _button.Content).Source = _currentTexture;
        }

        public void AcceptChange() => _changed = true;

        public void RevertChange()
        {
            if (_changed) return;
            _currentTexture = _previousTexture;
            _previousTexture = null;
            _currentTag = _previousTag;
            _previousTag = "";
            ((Image) _button.Content).Source = _currentTexture;
        }

        public (int x, int y) GetCoords() => (Grid.GetColumn(_button), Grid.GetRow(_button));

        public static Tile GetTileFromButton(Tile[,] tilesGrid, Button button)
        {
            (int x, int y) = (Grid.GetColumn(button), Grid.GetRow(button));
            return tilesGrid[y, x];
        }
    }
}