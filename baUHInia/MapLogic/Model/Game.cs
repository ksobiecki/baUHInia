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
    }
}
