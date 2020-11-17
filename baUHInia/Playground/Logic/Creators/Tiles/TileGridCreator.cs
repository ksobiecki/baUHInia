using System.Windows.Controls;
using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class TileGridCreator : IGameGridCreator
    {
        private static readonly (int x, int y) BoardResolution = (2000, 2000);
        private readonly int _boardDensity;

        private readonly TileCreator _tileCreator;


        public TileGridCreator(ITileBinder binder, int boardDensity)
        {
            _tileCreator = new TileCreator(binder.Selection, binder.TileGrid);
            _boardDensity = boardDensity;
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
        }

        public void LoadMapIntoTheGameGrid(ITileBinder tileBinder, Map map)
        {
            throw new System.NotImplementedException();
        }

        public void LoadGameIntoTheGameGrid(ITileBinder tileBinder, Game game)
        {
            throw new System.NotImplementedException();
        }

        //============================= GAME GRID ================================//

        //public void LoadGameGrid(Map map)
        
        public void CreateGameGridInWindow(Tile[,] tileFields, ScrollViewer window)
        {
            
        }

        private void FillGameGridWithTiles(Tile[,] tileFields)
        {
            for (int i = 0; i < _boardDensity; i++)
            {
                for (int j = 0; j < _boardDensity; j++)
                {
                    tileFields[i, j] = _tileCreator.CreateBehavioralTileInGameGrid(j, i, null);
                }
            }
        }

        private void FillGameGridWithTiles(ITileBinder tileBinder, Map map)
        {
            //TODO: implement            
        }

        private void FillGameGridWithTiles(ITileBinder tileBinder, Game game)
        {
            //TODO: implement
        }

        private void PlaceTile(Tile tileField, string tileName, bool placeable)
        {
            //TODO: implement
        }
    }
}