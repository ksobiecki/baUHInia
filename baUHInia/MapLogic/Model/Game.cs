using baUHInia.Playground.Model;

namespace baUHInia.MapLogic.Model
{
    public class Game
    {
        public int MapID { get; }
        public int GameID { get; }
        public int UserID { get; set; }
        public Placement[] PlacedObjects { get; }
        public Map Map { get; }

        public Game(int mapID, int gameID, int userID, Placement[] placedObjects, Map map)
        {
            MapID = mapID;
            GameID = gameID;
            UserID = userID;
            PlacedObjects = placedObjects;
            Map = map;
        }
    }
}
