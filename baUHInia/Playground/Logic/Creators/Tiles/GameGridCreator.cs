using System.Windows.Controls;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Tiles
{
    public class GameGridCreator
    {
        private static readonly (int x, int y) BoardResolution = (2000, 2000);
        private readonly int _boardDensity;

        private readonly TileCreator _tileCreator;
        public Grid _gameGrid;


        public GameGridCreator(ITileBinder binder, int boardDensity)
        {
            _tileCreator = new TileCreator(binder.Selection, binder.TileGrid);
            _boardDensity = boardDensity;
        }

        //============================= GAME GRID ================================//

        //public void LoadGameGrid(Map map)
        public void CreateGameGridInWindow(Tile[,] tileFields, ScrollViewer window)
        {
            _gameGrid = new Grid {Width = BoardResolution.x, Height = BoardResolution.y};
            _tileCreator.GameGrid = _gameGrid;
            for (int i = 0; i < _boardDensity; i++)
            {
                _gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                _gameGrid.RowDefinitions.Add(new RowDefinition());
            }

            FillGameGridWithTiles(tileFields);
            window.Content = _gameGrid;
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
    }
}