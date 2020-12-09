using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace baUHInia.MapLogic.Helper
{
    public class SerializationHelper
    {
        // Serialization
        public static void JsonAddBasicData(JObject jsonMap, ITileBinder tileBinder)
        {
            // TODO how do I get all the IDs?;
            jsonMap["Name"] = "Placeholder name";
            jsonMap["Size"] = tileBinder.TileGrid.GetLength(0);
            jsonMap["AvailableMoney"] = tileBinder.AvailableFounds;
        }

        public static void JsonAddTileGridAndDictionary(JObject jsonMap, Tile[,] tileGrid)
        {
            Dictionary<int, string> indexer = new Dictionary<int, string>();
            int[] intTileGrid = new int[tileGrid.GetLength(0) * tileGrid.GetLength(1)];
            JArray jsonIntTileGrid = new JArray();
            JArray jsonTileGrid = new JArray();
            int key = 0;

            for (int i = 0; i < tileGrid.GetLength(0); i++)
            {
                for (int j = 0; j < tileGrid.GetLength(1); j++)
                {
                    if (!indexer.ContainsValue(tileGrid[i, j].GetTextureName()))
                    {
                        indexer.Add(key, tileGrid[i, j].GetTextureName());

                        JArray jsonTile = new JArray {key, tileGrid[i, j].Placeable ? 1 : 0};
                        jsonTileGrid.Add(jsonTile);
                        jsonIntTileGrid.Add(key);
                        key++;
                    }
                    else
                    {
                        jsonIntTileGrid.Add(indexer.FirstOrDefault(x => x.Value == tileGrid[i, j].GetTextureName()).Key);
                        JArray jsonTile = new JArray
                        {
                            indexer.FirstOrDefault(x => x.Value == tileGrid[i, j].GetTextureName()).Key,
                            tileGrid[i, j].Placeable ? 1 : 0
                        };
                        jsonTileGrid.Add(jsonTile);
                    }
                }
            }

            jsonMap["TileGrid"] = jsonTileGrid;
            jsonMap["Indexer"] = DictionaryToJson(indexer);
        }

        private static JArray DictionaryToJson(Dictionary<int, string> indexer)
        {
            JArray a = new JArray();

            for (int i = 0; i < indexer.Count; i++)
            {
                JObject o = new JObject {["Key"] = i, ["Value"] = indexer[i]};
                a.Add(o);
            }

            return a;
        }

        public static Dictionary<int, string> GenerateDictionary(Tile[,] tileGrid)
        {
            Dictionary<int, string> indexer = new Dictionary<int, string>();
            int key = 0;

            for (int i = 0; i < tileGrid.GetLength(0); i++)
            {
                for (int j = 0; j < tileGrid.GetLength(1); j++)
                {
                    if (!indexer.ContainsValue(tileGrid[i, j].GetTextureName()))
                    {
                        indexer.Add(key, tileGrid[i, j].GetTextureName());
                        key++;
                    }
                }
            }

            return indexer;
        }

        public static void JsonAddPlacements(JObject jsonMap, List<Placement> placedObjects)
        {

            JArray jsonPlacements = new JArray();

            foreach (Placement placement in placedObjects)
            {
                JObject jsonPlacement = new JObject
                {
                    ["Name"] = placement.GameObject.TileObject.Name,
                    ["X"] = placement.Position.x,
                    ["Y"] = placement.Position.y
                };
                jsonPlacements.Add(jsonPlacement);
            }

            jsonMap["PlacedObjects"] = jsonPlacements;
        }

        public static void JsonAddAvailableTiles(JObject jsonMap, List<GameObject> availableTiles)
        {
            JArray jsonAvailableObjects = new JArray();
            foreach (GameObject gameObject in availableTiles)
            {
                JObject jsonTile = new JObject
                {
                    ["Name"] = gameObject.TileObject.Name,
                    ["Price"] = gameObject.Price,
                    ["ChangeValue"] = gameObject.ChangeValue
                };
                jsonAvailableObjects.Add(jsonTile);
            }

            jsonMap["AvailableTiles"] = jsonAvailableObjects;
        }

        // Deserialization
        public static void JsonGetBasicData(JObject jsonMap, ref string name, ref int size, ref int availableMoney)
        {
            name = (string)jsonMap["Name"];
            size = (int)jsonMap["Size"];
            availableMoney = (int)jsonMap["AvailableMoney"];
        }

        public static void JsonGetTileGridAndDictionary(JObject jsonMap, Dictionary<int,string> indexer, int[,] tileGrid, bool[,] placeableGrid)
        {
            int width = tileGrid.GetLength(0);
            int index = 0;

            JArray jsonTileGrid = (JArray)jsonMap["TileGrid"];
            JArray jsonIndexer = (JArray)jsonMap["Indexer"];

            foreach (JArray jsonTile in jsonTileGrid)
            {
                tileGrid[index / width, index % width] = (int)jsonTile[0];
                placeableGrid[index / width, index % width] = ((int)jsonTile[1] == 1);
                index++;
            }

            foreach (JObject association in jsonIndexer)
            {
                indexer.Add((int)association["Key"], (string)association["Value"]);
            }
        }

        public static void JsonGetAvailableTiles(JObject jsonMap, GameObject[] availableTiles)
        {
            JArray jsonAvailableTiles = (JArray)jsonMap["AvailableTiles"];
            availableTiles = new GameObject[jsonAvailableTiles.Count];

            int index = 0;

            foreach (JObject jsonTile in jsonAvailableTiles)
            {
                string name = (string)jsonTile["Name"];
                int price = (int)jsonTile["Price"];
                float change = (float)jsonTile["ChangeValue"];
                availableTiles[index] = new GameObject(ResourceHolder.Get.GetPlaceableTileObject(name), change, price);
                index++;
            }
        }

        public static void JsonGetPlacedObjects(JObject jsonMap, Placement[] placedObjects)
        {
            JArray jsonPlacedObjects = (JArray)jsonMap["PlacedObjects"];
            placedObjects = new Placement[jsonPlacedObjects.Count];

            int index = 0;

            foreach (JObject jsonPlacement in jsonPlacedObjects)
            {
                string name = (string)jsonPlacement["Name"];
                int x = (int)jsonPlacement["X"];
                int y = (int)jsonPlacement["Y"];
                GameObject gameObject = new GameObject(ResourceHolder.Get.GetPlaceableTileObject(name), 0, 0);
                placedObjects[index] = new Placement(gameObject, (x, y));
                index++;
            }
        }
    }
}
