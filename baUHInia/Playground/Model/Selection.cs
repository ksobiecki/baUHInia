using System.Collections.Generic;
using System.Windows.Controls;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model
{
    public class Selection
    {
        public TileObject TileObject { get; set; }
        private LinkedList<Tile> ChangedTiles { get; }
        
        //TODO
        private List<Tile>[,] ChangedElements { get; }
        
        private Grid GameGrid { get; }

        public Selection(TileObject tileObject)
        {
            TileObject = tileObject;
            ChangedTiles = new LinkedList<Tile>();
        }

        public void ApplyTiles()
        {
            foreach (Tile tile in ChangedTiles) tile.AcceptChange();
        }

        public void ApplyElements()
        {
            Offset[] offsets = TileObject.Sprite.Offsets;
            foreach (Tile changedElement in ChangedTiles)
            {
                //TODO
                (int x, int y) = changedElement.GetCoords();
                //GameGrid.
            }
        }

        public void RedoChanges()
        {
            foreach (Tile temporaryTile in ChangedTiles) temporaryTile.RevertChange();
            ChangedTiles.Clear();
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
            Offset offset = TileObject.Sprite.Offsets[offsetIndex];
            return (baseCoords.x + offset.X, baseCoords.y - offset.Y);
        }

        private void AddChangedSpriteToList(Tile tileAtCoords, int index)
        {
            tileAtCoords.Change(TileObject[index], TileObject.Sprite.FullNameAtIndex(index));
            ChangedTiles.AddLast(tileAtCoords);
        }

        //TODO: fix faulty
        private static bool IsOutsideGrid(int x, int y, Tile[,] grid) =>
            x < 0 || x > grid.GetUpperBound(1) || y < 0 || y > grid.GetUpperBound(0);
    }
}