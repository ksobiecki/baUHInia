using baUHInia.Playground.Model;

namespace baUHInia.Playground.Logic.Creators
{
    public interface IGameGridCreator
    {
        //TODO: change
        void CreateGameGridInWindow(ITileBinder tileBinder, int boardDensity);
        void LoadMapIntoTheGameGrid(ITileBinder tileBinder, int boardDensity);
        void LoadGameIntoTheGameGrid(ITileBinder tileBinder, int boardDensity);
    }
}