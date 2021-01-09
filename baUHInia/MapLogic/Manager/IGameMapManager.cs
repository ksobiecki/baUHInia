using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using System.Windows.Controls;

namespace baUHInia.MapLogic.Manager
{
    public interface IGameMapManager
    {
        Grid GetMapLoadGrid(int userID);
        Grid GetGameLoadGrid(int userID);
        Grid GetMapSaveGrid(int userID);
        Grid GetGameSaveGrid(int userID);
        Map LoadMap(out int mapID);
        Game LoadGame();
        bool SaveMap(ITileBinder tileBinder);
        bool SaveGame(ITileBinder tileBinder, int mapID);
    }
}