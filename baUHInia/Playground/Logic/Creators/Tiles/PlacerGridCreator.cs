using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class PlacerGridCreator : IGameGridCreator
    {
        private static readonly (int x, int y) BoardResolution = (2000, 2000);
        private readonly int _boardDensity;

        private readonly TileCreator _tileCreator;
        private readonly TileObject _tileObject;


        public PlacerGridCreator(ITileBinder binder, int boardDensity, TileObject tileObject)
        {
            _tileCreator = new TileCreator(binder.Selection, binder.TileGrid);
            _boardDensity = boardDensity;
            _tileObject = tileObject;
        }

        //============================= IMPLEMENTATIONS ================================//

        public void CreateGameGridInWindow(ITileBinder tileBinder, int boardDensity)
        {
            Grid gameGrid = new Grid {Width = BoardResolution.x, Height = BoardResolution.y};
            tileBinder.GameViewer.Content = gameGrid;

            _tileCreator.GameGrid = gameGrid;
            for (int i = 0; i < _boardDensity; i++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                gameGrid.RowDefinitions.Add(new RowDefinition());
            }

            FillGameGridWithTiles(tileBinder.TileGrid);
            InitializeElementsLayer(gameGrid, tileBinder.Selection, boardDensity);
        }

        public List<GameObject> LoadMapIntoTheGameGrid(ITileBinder tileBinder, Map map)
        {
            
            Grid gameGrid = new Grid {Width = BoardResolution.x, Height = BoardResolution.y};
            tileBinder.GameViewer.Content = gameGrid;

            _tileCreator.GameGrid = gameGrid;
            for (int i = 0; i < _boardDensity; i++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                gameGrid.RowDefinitions.Add(new RowDefinition());
            }

            tileBinder.AvailableFounds = map.AvailableMoney;
            FillGameGridWithTiles(tileBinder, map);

            tileBinder.Selection.ChangeState(State.Place);
            Placement[] placers = map.PlacedObjects ??  new Placement[0];
            foreach (Placement placement in placers)
            {
                tileBinder.Selection.UpdateChangedPlacerList(null, tileBinder.TileGrid, placement.Position);
                Button btn = tileBinder.TileGrid[placement.Position.y, placement.Position.x].GetUiElement() as Button;
                tileBinder.Selection.ApplyTiles(btn, false);
            }
            return map.AvailableTiles?.ToList() ?? new List<GameObject>();
        }

        public void LoadGameIntoTheGameGrid(ITileBinder tileBinder, Game game)
        {
            throw new System.NotImplementedException();
        }

        //============================= GAME GRID ================================//
        
        public void CreateGameGridInWindow(Tile[,] tileFields, ScrollViewer window) { }

        private void FillGameGridWithTiles(Tile[,] tileFields)
        {
            for (int i = 0; i < _boardDensity; i++)
            {
                for (int j = 0; j < _boardDensity; j++)
                {
                    tileFields[i, j] = _tileCreator.CreateBehavioralTileInGameGrid(j, i, _tileObject);
                }
            }
        }

        private void FillGameGridWithTiles(ITileBinder tileBinder, Map map)
        {
            for (int i = 0; i < _boardDensity; i++)
            {
                for (int j = 0; j < _boardDensity; j++)
                {
                    string imagePath = map.Indexer[map.TileGrid[i, j]];
                    (TileObject to, BitmapImage bi) = ResourceHolder.Get.GetTerrainPair(imagePath);
                    Tile tile = _tileCreator.CreateBehavioralTileInGameGrid(j, i, to);
                    tile.SwapTexture(bi);
                    tile.Placeable = map.PlacableGrid[i, j];
                    tileBinder.TileGrid[i, j] = tile;
                }
            }
        }

        private void FillGameGridWithTiles(ITileBinder tileBinder, Game game)
        {
            //TODO: implement
        }

        private void PlaceTile(Tile tileField, string tileName, bool placeable)
        {
            //TODO: implement
        }

        private void InitializeElementsLayer(Grid gameGrid, Selection selection, int boardDensity)
        {
            //TODO: change
            List<Element>[,] elementsLayers = new List<Element>[boardDensity, boardDensity];
            for (int i = 0; i < boardDensity; i++)
            {
                for (int j = 0; j < boardDensity; j++)
                {
                    Image image = new Image {IsHitTestVisible = false};
                    Grid.SetRow(image, i);
                    Grid.SetColumn(image, j);
                    gameGrid.Children.Add(image);
                    elementsLayers[i, j] = new List<Element> {new Element(image, null)};
                }
            }

            selection.ElementsLayers = elementsLayers;
        }
    }
}