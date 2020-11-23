using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Utility;

namespace baUHInia.Playground.Model.Wrappers
{
    public class Selection
    {
        public State SelectionState { get; private set; } = State.Place;
        public TileObject TileObject { get; set; }
        public List<Element>[,] ElementsLayers { get; set; }
        public List<Placement> PlacedElements { get; }
        private LinkedList<Placer> ChangedPlacers { get; }
        
        private ITileBinder Binder { get; }


        public Selection(TileObject tileObject, ITileBinder binder)
        {
            Binder = binder;
            TileObject = tileObject;
            PlacedElements = new List<Placement>();
            ChangedPlacers = new LinkedList<Placer>();
        }

        public void ChangeState(State state)
        {
            SelectionState = state;
            switch (SelectionState)
            {
                case State.Place: Binder.ChangeMode("STAWIANIE", Brushes.Coral);
                    break;
                case State.Remove: Binder.ChangeMode("USUWANIE", Brushes.Brown);
                    break;
                case State.MarkAsPlaceable:
                    break;
                case State.MarkAsNotPlaceable:
                    break;
            }
        }

        public void ApplyTiles(Button button)
        {
            (int x, int y) position = (Grid.GetColumn(button), Grid.GetRow(button));
            bool isElement = TileObject.Config.IsElement;
            bool located = PlacedElements.FirstOrDefault(e => e.Position == position) != null;


            if (isElement && SelectionState == State.Remove)
            {
                Grid grid = button.Parent as Grid;
                Element changedPlacer = null;
                foreach (Placer placer in ChangedPlacers)
                {
                    changedPlacer = (Element) placer;
                    (int x, int y) = changedPlacer.GetCoords();
                    List<Element> placers = ElementsLayers[y, x];
                    placers.Remove(changedPlacer);
                    grid.Children.Remove(changedPlacer.GetUIElement());
                }
                if (changedPlacer != null)
                {
                    (int, int) sourceCoords = changedPlacer.Root.Coords; 
                    Placement existingPlacement = PlacedElements.First(p => p.Position == sourceCoords);
                    PlacedElements.Remove(existingPlacement);
                    ChangedPlacers.Clear();
                    return; 
                }
            }
            
            
            if (isElement && located)
            {
                Console.WriteLine("CANNOT PLACE THERE!");
                return;
            }
            
            foreach (Placer tile in ChangedPlacers)
            {
                tile.AcceptChange();
                if (!isElement) continue;
                
                var coords = tile.GetCoords();
                List<Element> elements = ElementsLayers[coords.y, coords.x];
                Image image = new Image {IsHitTestVisible = false};
                Grid grid = tile.GetParentGrid();
                Grid.SetColumn(image, coords.x);
                Grid.SetRow(image, coords.y);
                grid.Children.Add(image);
                tile.Root = new Offset(
                    ((Grid) button.Parent).Children.Count - 1 - TileObject.Config.Offsets.Length,
                    (sbyte) position.x,
                    (sbyte) position.y
                );
                elements.Add(new Element(image, null));
                tile.TileObject = TileObject;
            }

            if (!isElement) return;
            GameObject gameObject = new GameObject(TileObject, 0, 0);
            Placement placement = new Placement(gameObject, position);
            PlacedElements.Add(placement);
        }

        public void RedoChanges()
        {
            foreach (Placer temporaryTile in ChangedPlacers)
            {
                if (SelectionState == State.Remove) ((Element)temporaryTile).UnmarkAsRemovable();
                else temporaryTile.RevertChange();
            }
            ChangedPlacers.Clear();
        }

        public void UpdateChangedPlacerList(Tile hoveredTile, Tile[,] tileGrid)
        {
            (int x, int y) parent = hoveredTile.GetCoords();
            if (SelectionState == State.Remove)
            {
                List<Element> layers = ElementsLayers[parent.y, parent.x];
                if (layers.Count <= 1) return;
                Element topElement = layers[layers.Count - 2];
                if (topElement.TileObject != null)
                {
                    MarkAsRemovable(topElement);
                }
                return;
            }

            for (int i = 0; i < TileObject.Sprite.Offsets.Length; i++)
            {
                (int x, int y) = GetCoords(hoveredTile, i);
                if (IsOutsideGrid(x, y, tileGrid)) continue;
                switch (SelectionState)
                {
                    case State.Place when TileObject.Config.IsElement:
                        UpdateChangedElementList((x, y), i);
                        break;
                    case State.Place:
                        UpdateChangedTileList(tileGrid[y, x], i);
                        break;
                    case State.MarkAsPlaceable: break;
                    case State.MarkAsNotPlaceable: break;
                }
            }
        }

        private void UpdateChangedTileList(Tile tileAtCoords, int index)
        {
            tileAtCoords.Change(TileObject[index], TileObject.Config.Name);
            ChangedPlacers.AddLast(tileAtCoords);
        }

        private void UpdateChangedElementList((int x, int y) coords, int index)
        {
            List<Element> elements = ElementsLayers[coords.y, coords.x];
            Element element = elements.Last();
            element.Change(TileObject[index], TileObject.Config.Name);
            ChangedPlacers.AddLast(element);
        }

        private void MarkAsRemovable(Element element)
        {
            TileObject pointedObject = element.TileObject;
            TileObject = pointedObject;
            Element parentElement = ElementsLayers[element.Root.Y, element.Root.X].First(e => e.Root.X == e.GetCoords().x && e.Root.Y == e.GetCoords().y);
            (int x, int y) parent = parentElement.GetCoords();
            foreach (Offset offset in pointedObject.Config.Offsets)
            {
                (int x, int y) = GetCoords(parentElement, offset.I);
                if (IsOutsideGrid(x, y, ElementsLayers)) continue;
                Element childElement = ElementsLayers[y, x].Find(e => e.Root.X == parent.x && e.Root.Y == parent.y);
                ChangedPlacers.AddLast(childElement);
                childElement.MarkAsRemovable();
            }
        }

        private (int x, int y) GetCoords(Placer tileBeforeOffset, int offsetIndex)
        {
            (int x, int y) baseCoords = tileBeforeOffset.GetCoords();
            Offset offset = TileObject.Config.Offsets[offsetIndex];
            return (baseCoords.x + offset.X, baseCoords.y - offset.Y);
        }

        private static bool IsOutsideGrid(int x, int y, object[,] grid) =>
            x < 0 || x > grid.GetUpperBound(1) || y < 0 || y > grid.GetUpperBound(0);
    }
}