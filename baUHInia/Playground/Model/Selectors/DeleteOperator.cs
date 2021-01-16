using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Playground.Model.Selectors
{
    public class DeleteOperator : IOperator
    {
        private readonly Selection _selection;
        
        public DeleteOperator(Selection selection) => _selection = selection;
        

        public void ApplyTiles(Button button, bool rmb)
        {
            if (!_selection.TileObject.Config.IsElement) return;
            Grid grid = button.Parent as Grid;
            Element changedPlacer = null;
            
            foreach (Placer placer in _selection.ChangedPlacers)
            {
                changedPlacer = (Element) placer;
                (int x, int y) = changedPlacer.GetCoords();
                List<Element> placers = _selection.ElementsLayers[y, x];
                placers.Remove(changedPlacer);
                grid.Children.Remove(changedPlacer.GetUiElement());
            }

            if (changedPlacer == null) return;
            (int, int) sourceCoords = changedPlacer.Root.Coords; 
            Placement existingPlacement = _selection.Binder.PlacedObjects.First(p => p.Position == sourceCoords);
            _selection.Binder.PlacedObjects.Remove(existingPlacement);
            _selection.ChangedPlacers.Clear();
            _selection.UpdateCost(false);
        }

        public void RedoChanges()
        {
            foreach (Placer temporaryTile in _selection.ChangedPlacers)
            {
                ((Element)temporaryTile).MarkAsRemovable(1.0);
            }
            _selection.ChangedPlacers.Clear();
        }

        public void UpdateChangedPlacerList(Tile hoveredTile, Tile[,] tileGrid, (sbyte x, sbyte y)? pos = null)
        {
            (int x, int y) = hoveredTile.GetCoords();
            List<Element> layers = _selection.ElementsLayers[y, x];
            if (layers.Count <= 1) return;
            Element topElement = layers[layers.Count - 2];
            if (topElement.TileObject != null)
            {
                MarkAsRemovable(topElement);
            }
        }

        public void SelectOperator() { }

        public void DeselectOperator() { }

        // ReSharper disable once CoVariantArrayConversion
        private void MarkAsRemovable(Placer element)
        {
            TileObject pointedObject = element.TileObject;
            _selection.TileObject = pointedObject;
            List<Element> elements = _selection.ElementsLayers[element.Root.Y, element.Root.X];
            Element parentElement = elements.First(e => e.Root.X == e.GetCoords().x && e.Root.Y == e.GetCoords().y);
            
            (int x, int y) parent = parentElement.GetCoords();
            for (int i = 0; i < parentElement.TileObject.Config.Offsets.Length; i++)
            {
                (int x, int y) = _selection.GetCoords(parentElement, i);
                if (Selection.IsOutsideGrid(x, y, _selection.ElementsLayers)) continue;
                List<Element> layers = _selection.ElementsLayers[y, x];
                Element childElement = layers.Find(e => e.Root.X == parent.x && e.Root.Y == parent.y);
                _selection.ChangedPlacers.AddLast(childElement);
                childElement.MarkAsRemovable(0.6);
            }
        }
    }
}