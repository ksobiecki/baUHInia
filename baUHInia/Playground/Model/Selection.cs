using System.Collections.Generic;
using System.Linq;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model
{
    public class Selection
    {
        public TileObject TileObject { get; set; }
        private LinkedList<Placer> ChangedPlacers { get; }
        public List<Element>[,] ElementsLayers { get; set; }

        public Selection(TileObject tileObject)
        {
            TileObject = tileObject;
            ChangedPlacers = new LinkedList<Placer>();
        }

        public void ApplyTiles()
        {
            foreach (Placer tile in ChangedPlacers) tile.AcceptChange();
        }

        public void RedoChanges()
        {
            foreach (Placer temporaryTile in ChangedPlacers) temporaryTile.RevertChange();
            ChangedPlacers.Clear();
        }

        public void UpdateChangedPlacerList(Tile hoveredTile, Tile[,] tileGrid)
        {
            for (int i = 0; i < TileObject.Sprite.Offsets.Length; i++)
            {
                (int x, int y) = GetCoords(hoveredTile, i);
                if (IsOutsideGrid(x, y, tileGrid)) continue;
                if (TileObject.Config.IsElement) UpdateChangedElementList((x, y), i);
                else UpdateChangedTileList(tileGrid[y, x], i);
            }
        }

        private void UpdateChangedTileList(Tile tileAtCoords, int index)
        {
            tileAtCoords.Change(TileObject[index], TileObject.Sprite.FullNameAtIndex(index));
            ChangedPlacers.AddLast(tileAtCoords);
        }

        private void UpdateChangedElementList((int x, int y) coords, int index)
        {
            Element element = ElementsLayers[coords.y, coords.x].Last();
            //TODO: check name
            element.Change(TileObject[index], TileObject.Config.Name);
            ChangedPlacers.AddLast(element);
        }

        private (int x, int y) GetCoords(Placer tileBeforeOffset, int offsetIndex)
        {
            (int x, int y) baseCoords = tileBeforeOffset.GetCoords();
            Offset offset = TileObject.Sprite.Offsets[offsetIndex];
            return (baseCoords.x + offset.X, baseCoords.y - offset.Y);
        }

        private static bool IsOutsideGrid(int x, int y, Tile[,] grid) =>
            x < 0 || x > grid.GetUpperBound(1) || y < 0 || y > grid.GetUpperBound(0);
    }
}