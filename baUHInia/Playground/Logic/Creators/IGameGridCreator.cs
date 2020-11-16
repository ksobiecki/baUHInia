using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;

namespace baUHInia.Playground.Logic.Creators
{
    public interface IGameGridCreator
    {
        void CreateGameGridInWindow(ITileBinder tileBinder, int boardDensity);
        void LoadMapIntoTheGameGrid(ITileBinder tileBinder, Map map);
        void LoadGameIntoTheGameGrid(ITileBinder tileBinder, Game game);
    }
}