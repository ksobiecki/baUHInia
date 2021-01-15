using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Logic.Utils;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Utility;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Playground.Model.Selectors
{
    public class PlaceOperator : IOperator
    {
        private readonly Selection _selection;
        private Tile _currentTile;

        public PlaceOperator(Selection selection) => _selection = selection;

        public void ApplyTiles(Button button, bool rmb)
        {
            (sbyte x, sbyte y) position = ((sbyte) Grid.GetColumn(button), (sbyte) Grid.GetRow(button));
            bool isElement = _selection.TileObject.Config.IsElement;

            _currentTile?.ShowIfAvailable(1.0f, Brushes.Navy, Brushes.Maroon);
            bool same = _currentTile == _selection.Binder.TileGrid[position.y, position.x];
            _currentTile = _selection.Binder.TileGrid[position.y, position.x];

            if (!CheckAllConditions(same, position, isElement)) return;

            foreach (Placer tile in _selection.ChangedPlacers)
            {
                tile.AcceptChange();
                if (!isElement) continue;
                AttachElementAtCoords(tile, button, position);
            }

            if (!isElement) return;
            GameObject gameObject = new GameObject(_selection.TileObject, 0, 0);
            Placement placement = new Placement(gameObject, position);
            _selection.Binder.PlacedObjects.Add(placement);
        }

        public void RedoChanges()
        {
            foreach (Placer temporaryTile in _selection.ChangedPlacers)
            {
                temporaryTile.RevertChange();
            }

            _selection.ChangedPlacers.Clear();
        }

        // ReSharper disable once CoVariantArrayConversion
        public void UpdateChangedPlacerList(Tile hoveredTile, Tile[,] tileGrid, (sbyte x, sbyte y)? pos = null)
        {
            Config config = _selection.TileObject.Config;
            bool isElement = config.IsElement;
            for (int i = 0; i < config.Offsets.Length; i++)
            {
                //TODO: all stacked up
                (int x, int y) = pos != null
                    ? (pos.Value.x + config.Offsets[i].X, pos.Value.y - config.Offsets[i].Y)
                    : _selection.GetCoords(hoveredTile, i);

                if (Selection.IsOutsideGrid(x, y, tileGrid)) continue;
                if (isElement) UpdateChangedElementList(x, y, i);
                else UpdateChangedTileList(tileGrid[y, x], i);
            }
        }

        public void SelectOperator() { }

        public void DeselectOperator() => _currentTile = null;

        private void UpdateChangedTileList(Placer tileAtCoords, int index)
        {
            TileObject tileObject = _selection.TileObject;
            tileAtCoords.Change(tileObject[index], tileObject.Config.Name);
            _selection.ChangedPlacers.AddLast(tileAtCoords);
        }

        private void UpdateChangedElementList(int x, int y, int index)
        {
            List<Element> elements = _selection.ElementsLayers[y, x];
            TileObject tileObject = _selection.TileObject;
            Element element = elements.Last();
            element.Change(tileObject[index], tileObject.Config.Name);
            _selection.ChangedPlacers.AddLast(element);
        }

        private bool CheckAllConditions(bool sameElement, (int, int) position, bool isElement)
        {
            List<Placement> placedObjects = _selection.Binder.PlacedObjects;

            bool located = placedObjects.FirstOrDefault(e => e.Position == position) != null;
            bool isNotPlaceable = !_currentTile.Placeable && _selection.IsUserWindow();
            bool isOccupied = isElement && located;

            // ReSharper disable once InvertIf
            if (isNotPlaceable || isOccupied || !_selection.UpdateCost(true))
            {
                if (sameElement) _currentTile = null;
                else _currentTile.ShowIfAvailable(0.25f, Brushes.Maroon, Brushes.Maroon);
                return false;
            }

            return true;
        }

        private void AttachElementAtCoords(Placer tile, FrameworkElement button, (sbyte x, sbyte y) position)
        {
            (int x, int y) = tile.GetCoords();
            List<Element> elements = _selection.ElementsLayers[y, x];

            Image image = new Image {IsHitTestVisible = false};
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            Grid grid = tile.GetParentGrid();
            Grid.SetColumn(image, x);
            Grid.SetRow(image, y);
            grid.Children.Add(image);

            (sbyte rootX, sbyte rootY) = position;
            tile.Root = new Offset(CalculateRootIndex(button), rootX, rootY);
            elements.Add(new Element(image, null));
            tile.TileObject = _selection.TileObject;
        }

        private int CalculateRootIndex(FrameworkElement button) =>
            ((Grid) button.Parent).Children.Count - 1 - _selection.TileObject.Config.Offsets.Length;
    }
}