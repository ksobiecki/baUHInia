using System.Windows.Controls;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model.Selectors
{
    public interface IOperator
    {
        void ApplyTiles(Button button, bool rmb);

        void RedoChanges();

        void UpdateChangedPlacerList(Tile hoveredTile, Tile[,] tileGrid, (int x, int y)? pos = null);

        void SelectOperator();

        void DeselectOperator();
    }
}