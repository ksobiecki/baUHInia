using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Utility;
using baUHInia.Playground.View;

namespace baUHInia.Playground.Model.Selectors
{
    public class Selection
    {
        private const string PlacementString = "STAWIANIE/LMB: POJEDYŃCZO/RMB: MALOWANIE/NIEDOSTĘPNE DLA OBIEKTÓW";
        private const string RemoveString = "USUWANIE/LMB: USUWA OBIEKT/PO NAJECHANIU, POLA/ZMIENIAJĄ PRZEZROCZYSTOŚĆ";
        private const string BlockString = "BLOKOWANIE/LMB: ODBLOKOWUJE/RMB: MALUJE/LCTRL + LMB\\RMB: BLOKUJE";

        public ITileBinder Binder { get; }
        public TileObject TileObject { get; set; }
        public List<Element>[,] ElementsLayers { get; set; }
        public LinkedList<Placer> ChangedPlacers { get; private set; }
        public State SelectionState { get; private set; }

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
            if (IsUserWindow()) ((UserGameWindow) Binder).CurrentCash.Foreground = Brushes.Azure;
        }

        public void Reset()
        {
            SelectionState = State.Place;
            ChangedPlacers = new LinkedList<Placer>();
            ChangeState(State.Place);
        }

        public void ChangeState(State state)
        {
            SelectionState = state;
            CurrentOperator.DeselectOperator();
            switch (SelectionState)
            {
                case State.Place:
                    Binder.ChangeInteractionMode(PlacementString, Brushes.SeaGreen);
                    CurrentOperator = Operators[0];
                    break;
                case State.Remove:
                    Binder.ChangeInteractionMode(RemoveString, Brushes.Brown);
                    CurrentOperator = Operators[1];
                    break;
                case State.Block:
                    Binder.ChangeInteractionMode(BlockString, Brushes.Peru);
                    CurrentOperator = Operators[2];
                    break;
            }

            CurrentOperator.SelectOperator();
        }

        public void ApplyTiles(Button button, bool rmb) => CurrentOperator.ApplyTiles(button, rmb);

        public void RedoChanges() => CurrentOperator.RedoChanges();


        public void UpdateChangedPlacerList(Tile hoveredTile, Tile[,] tileGrid, (sbyte x, sbyte y)? pos = null) =>
            CurrentOperator.UpdateChangedPlacerList(hoveredTile, tileGrid, pos);

        public (int x, int y) GetCoords(Placer tileBeforeOffset, int offsetIndex)
        {
            (int x, int y) baseCoords = tileBeforeOffset.GetCoords();
            Offset offset = TileObject.Config.Offsets[offsetIndex];
            return (baseCoords.x + offset.X, baseCoords.y - offset.Y);
        }

        public bool IsUserWindow() => Binder.GetType() == typeof(UserGameWindow);

        public bool UpdateCost(bool buy)
        {
            if (!IsUserWindow()) return true;
            TextBlock currentCash = ((UserGameWindow) Binder).CurrentCash;
            int cost = Binder.AvailableObjects?.FirstOrDefault(a => a.TileObject == TileObject).Price ?? 0;
            int money = int.Parse(currentCash.Text) - (buy ? cost : -cost);

            if (money < 0)
            {
                currentCash.Foreground = Brushes.Crimson;
                return false;
            }

            currentCash.Foreground = Brushes.Azure;
            currentCash.Text = money.ToString();
            return true;
        }

        public void AssignSelection(TileObject tileObject)
        {
            TileObject = tileObject;
            if (!IsUserWindow()) return;
            UserGameWindow window = Binder as UserGameWindow;
            window.UpdateSelectionWindow(tileObject);
        }

        public static bool IsOutsideGrid(int x, int y, object[,] grid) =>
            x < 0 || x > grid.GetUpperBound(1) || y < 0 || y > grid.GetUpperBound(0);
    }
}