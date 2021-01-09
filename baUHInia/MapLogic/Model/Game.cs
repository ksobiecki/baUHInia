using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.MapLogic.Model
{
    public class Game
    {
        public int MapID { get; }
        public string Name { get; }
        public Placement[] PlacedObjects { get; }
        public Map Map { get; }

        public Game(int mapID, string name, Placement[] placedObjects, Map map)
        {
            MapID = mapID;
            Name = name;
            PlacedObjects = placedObjects;
            Map = map;
        }
    }
}
