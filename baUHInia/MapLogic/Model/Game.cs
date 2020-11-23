using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.MapLogic.Model
{
    public class Game
    {
        public int MapID { get; }
        public int GameID { get; }
        public int UserID { get; set; }
        public string Name { get; }
        public Placement[] PlacedObjects { get; }
        public Map Map { get; }

        public Game(int mapID, int gameID, int userID, string name, Placement[] placedObjects, Map map)
        {
            MapID = mapID;
            GameID = gameID;
            UserID = userID;
            Name = name;
            PlacedObjects = placedObjects;
            Map = map;
        }

        public Game(string name)
        {
            Name = name;
        }
    }
}
