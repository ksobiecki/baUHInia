using System.Collections.Generic;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model
{
    public class Selection
    {
        public TileObject TileObject { get; set; }
        private LinkedList<Tile> ChangedElements { get; }

        public Selection(TileObject tileObject)
        {
            TileObject = tileObject;
            ChangedElements = new LinkedList<Tile>();
        }

        public void ApplyTiles()
        {
            foreach (Tile tile in ChangedElements) tile.AcceptChange();
        }

        public void RedoChanges()
        {
            foreach (Tile temporaryTile in ChangedElements) temporaryTile.RevertChange();
            ChangedElements.Clear();
        }

        public void UpdateChangedTileList(Tile hoveredTile, Tile[,] tileGrid)
        {
            for (int i = 0; i < TileObject.Sprite.Offsets.Length; i++)
            {
                (int x, int y) = GetCoords(hoveredTile, i);
                if (IsOutsideGrid(x, y, tileGrid)) return;
                AddChangedSpriteToList(tileGrid[y, x], i);
            }
        }

        private (int x, int y) GetCoords(Tile tileBeforeOffset, int offsetIndex)
        {
            (int x, int y) baseCoords = tileBeforeOffset.GetCoords();
            (byte _, sbyte xOffset, sbyte yOffset) = TileObject.Sprite.Offsets[offsetIndex];
            return (baseCoords.x + xOffset, baseCoords.y - yOffset);
        }

        private void AddChangedSpriteToList(Tile tileAtCoords, int index)
        {
            tileAtCoords.Change(TileObject[index], TileObject.Sprite.FullNameAtIndex(index));
            ChangedElements.AddLast(tileAtCoords);
        }

        private static bool IsOutsideGrid(int x, int y, Tile[,] grid) =>
            x < 0 || x > grid.GetUpperBound(1) || y < 0 || y > grid.GetUpperBound(0);
    }
}