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
        Map LoadMap(out int mapID);
        Game LoadGame();
        bool SaveMap(ITileBinder tileBinder);
        bool SaveGame(ITileBinder tileBinder, int mapID);
        void PopulateSaveMapListGrid();
        void PopulateEditLoadMapListGrid();
        void PopulatePlayLoadMapListGrid();
        void PopulateUserLoadGameListGrid();
        void PopulateObserverLoadGameListGrid();
        void PopulateSaveGameListGrid();
    }
}