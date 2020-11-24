using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Utility;

namespace baUHInia.Playground.Model.Selectors
{
    public class Selection
    {
        public ITileBinder Binder { get; }
        public TileObject TileObject { get; set; }
        public List<Element>[,] ElementsLayers { get; set; }
        public LinkedList<Placer> ChangedPlacers { get; }
        private State SelectionState { get; set; }


        private IOperator[] Operators { get; }
        private IOperator CurrentOperator { get; set; }


        public Selection(TileObject tileObject, ITileBinder binder)
        {
            Binder = binder;
            TileObject = tileObject;
            SelectionState = State.Place;
            ChangedPlacers = new LinkedList<Placer>();
            Operators = new IOperator[] {new PlaceOperator(this), new DeleteOperator(this), new BlockOperator(this)};
            CurrentOperator = Operators[0];
        }

        public void ChangeState(State state)
        {
            SelectionState = state;
            CurrentOperator.DeselectOperator();
            switch (SelectionState)
            {
                case State.Place:
                    Binder.ChangeMode("STAWIANIE", Brushes.SeaGreen);
                    CurrentOperator = Operators[0];
                    break;
                case State.Remove:
                    Binder.ChangeMode("USUWANIE", Brushes.Brown);
                    CurrentOperator = Operators[1];
                    break;
                case State.Block:
                    Binder.ChangeMode("LCTRL", Brushes.Peru);
                    CurrentOperator = Operators[2];
                    break;
            }

            CurrentOperator.SelectOperator();
        }

        public void ApplyTiles(Button button, bool rmb) => CurrentOperator.ApplyTiles(button, rmb);

        public void RedoChanges() => CurrentOperator.RedoChanges();


        public void UpdateChangedPlacerList(Tile hoveredTile, Tile[,] tileGrid) =>
            CurrentOperator.UpdateChangedPlacerList(hoveredTile, tileGrid);

        public (int x, int y) GetCoords(Placer tileBeforeOffset, int offsetIndex)
        {
            (int x, int y) baseCoords = tileBeforeOffset.GetCoords();
            Offset offset = TileObject.Config.Offsets[offsetIndex];
            return (baseCoords.x + offset.X, baseCoords.y - offset.Y);
        }

        public static bool IsOutsideGrid(int x, int y, object[,] grid) =>
            x < 0 || x > grid.GetUpperBound(1) || y < 0 || y > grid.GetUpperBound(0);
    }
}