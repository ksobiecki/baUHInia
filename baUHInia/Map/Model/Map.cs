using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baUHInia.Map
{
    class Map
    {
        public int MapID { get; }
        public int AuthorID { get; }
        public int[,] TileGrid { get; }
        public bool[,] PlacableGrid { get; }
        public Dictionary<int, string> Indexer { get; }
        //public GameObject[] AvailableTiles { get; }
        public int AvailableMoney { get; set; }
        //public Placement[] PlacedObjects { get; }

        public Map(int mapID, int authorID, int[,] tileGrid, bool[,] placableGrid, Dictionary<int, string> indexer, int availableMoney)
        {
            MapID = mapID;
            AuthorID = authorID;
            TileGrid = tileGrid;
            PlacableGrid = placableGrid;
            Indexer = indexer;
            // Available
            AvailableMoney = availableMoney;
            // Placed

        }
    }
}
