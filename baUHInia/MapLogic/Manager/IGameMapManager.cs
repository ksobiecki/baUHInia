using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using System.Windows.Controls;

namespace baUHInia.MapLogic.Manager
{
    public interface IGameMapManager
    {
        Grid GetMapLoadGrid();
        Grid GetGameLoadGrid();
        Grid GetMapSaveGrid();
        Grid GetGameSaveGrid();
        Map LoadMap(string name);
        Game LoadGame(string name);
        bool SaveMap(ITileBinder tileBinder);
        bool SaveGame(ITileBinder tileBinder, Map map);
    }
}