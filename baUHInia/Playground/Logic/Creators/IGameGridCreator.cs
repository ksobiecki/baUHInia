using System.Collections.Generic;
using System.Windows.Controls;
using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Playground.Logic.Creators
{
    public interface IGameGridCreator
    {
        Grid CreateElementsInWindow(ITileBinder tileBinder, int boardDensity);
        List<GameObject> LoadMapIntoTheGameGrid(ITileBinder tileBinder, Map map);
        void LoadGameIntoTheGameGrid(ITileBinder tileBinder, Game game);
    }
}