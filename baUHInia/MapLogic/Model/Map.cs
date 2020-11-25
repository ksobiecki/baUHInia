using baUHInia.Playground.Model;
using System.Collections.Generic;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.MapLogic.Model
{
    public class Map
    {
        public int MapID { get; }
        public int AuthorID { get; }
        public string Name { get; }
        public int[,] TileGrid { get; }
        public bool[,] PlacableGrid { get; }
        public Dictionary<int, string> Indexer { get; }
        public GameObject[] AvailableTiles { get; }
        public int AvailableMoney { get; set; }
        public Placement[] PlacedObjects { get; }

        public Map(int mapID, int authorID, string name, int[,] tileGrid, bool[,] placableGrid, Dictionary<int, string> indexer, GameObject[] availableTiles, int availableMoney, Placement[] placedObjects)
        {
            MapID = mapID;
            AuthorID = authorID;
            Name = name;
            TileGrid = tileGrid;
            PlacableGrid = placableGrid;
            Indexer = indexer;
            AvailableTiles = availableTiles;
            AvailableMoney = availableMoney;
            PlacedObjects = placedObjects;
        }

        public Map(string name) { // Debug constructor for mockup data.
            Name = name;
        }
    }
}
