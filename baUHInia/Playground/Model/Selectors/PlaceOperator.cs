﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using baUHInia.Playground.Logic.Utils;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Utility;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Playground.Model.Selectors
{
    public class PlaceOperator : IOperator
    {
        private readonly Selection _selection;

        public PlaceOperator(Selection selection) => _selection = selection;

        public void ApplyTiles(Button button, bool rmb)
        {
            (int x, int y) position = (Grid.GetColumn(button), Grid.GetRow(button));
            bool located = _selection.Binder.PlacedObjects.FirstOrDefault(e => e.Position == position) != null;
            bool isElement = _selection.TileObject.Config.IsElement;
            if (isElement && located)
            {
                Console.WriteLine("CANNOT PLACE THERE!");
                return;
            }

            foreach (Placer tile in _selection.ChangedPlacers)
            {
                tile.AcceptChange();
                if (!isElement) continue;

                (int x, int y) = tile.GetCoords();
                List<Element> elements = _selection.ElementsLayers[y, x];
                Image image = new Image {IsHitTestVisible = false};
                Grid grid = tile.GetParentGrid();
                Grid.SetColumn(image, x);
                Grid.SetRow(image, y);
                grid.Children.Add(image);
                tile.Root = new Offset(
                    ((Grid) button.Parent).Children.Count - 1 - _selection.TileObject.Config.Offsets.Length,
                    (sbyte) position.x,
                    (sbyte) position.y
                );
                elements.Add(new Element(image, null));
                tile.TileObject = _selection.TileObject;
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
        public void UpdateChangedPlacerList(Tile hoveredTile, Tile[,] tileGrid)
        {
            Config config = _selection.TileObject.Config;
            bool isElement = config.IsElement;
            for (int i = 0; i < config.Offsets.Length; i++)
            {
                (int x, int y) = _selection.GetCoords(hoveredTile, i);
                if (Selection.IsOutsideGrid(x, y, tileGrid)) continue;
                if (isElement) UpdateChangedElementList(x, y, i);
                else UpdateChangedTileList(tileGrid[y, x], i);
            }
        }

        public void SelectOperator()
        {
            
        }

        public void DeselectOperator()
        {
            
        }

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
    }
}