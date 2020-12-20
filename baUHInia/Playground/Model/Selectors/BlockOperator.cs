using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model.Selectors
{
    public class BlockOperator : IOperator
    {
        private const double Full = 1.0;
        private const double Element = 0.5;
        private const double Tile = 0.1;
        private const double Hovered = 0.0;
        
        private readonly Selection _selection;
        private Tile Hover { get; set; }
        
        public BlockOperator(Selection selection) => _selection = selection;

        public void ApplyTiles(Button button, bool rmb)
        {
            Hover.Placeable = !rmb;
            MarkTileAsUnHovered();
        }

        public void RedoChanges() => MarkTileAsUnHovered();

        public void UpdateChangedPlacerList(Tile hoveredTile, Tile[,] tileGrid, (sbyte x, sbyte y)? pos = null)
        {
            Hover = hoveredTile;
            MarkTileAsHovered();
        }

        public void SelectOperator() => ChangeAppearance(Element, Tile);

        public void DeselectOperator() => ChangeAppearance(Full, Full);

        private void ChangeAppearance(double elements, double tiles)
        {
            foreach (List<Element> list in _selection.ElementsLayers)
            foreach (Element element in list)
                element.MarkAsRemovable(elements);

            foreach (Tile tile in _selection.Binder.TileGrid)
                tile.ShowIfAvailable(tiles, Brushes.RoyalBlue, Brushes.Firebrick);
        }

        private void MarkTileAsHovered() =>
            Hover.ShowIfAvailable(Hovered, Brushes.Navy, Brushes.Maroon);
        
        private void MarkTileAsUnHovered() =>
            Hover.ShowIfAvailable(Tile, Brushes.RoyalBlue, Brushes.Firebrick);
    }
}