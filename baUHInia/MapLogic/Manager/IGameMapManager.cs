using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using System.Windows.Controls;

namespace baUHInia.MapLogic.Manager
{
    public interface IGameMapManager
    {
        Grid GetMapLoadGrid();
        Grid GetGameLoadGrid();
        bool GetMapSaveGrid();
        Map LoadMap(string name);
        Game LoadGame(string name);
        bool SaveMap(ITileBinder tileBinder);
        bool SaveGame(ITileBinder tileBinder);

    }
}
