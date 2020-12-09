using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model.Wrappers
{
    public class GameObject
    {
        public readonly TileObject TileObject;
        public float ChangeValue { get; set; }
        public int Price{ get; set; }

        public GameObject(TileObject tileObject, float changeValue, int price)
        {
            TileObject = tileObject;
            ChangeValue = changeValue;
            Price = price;
        }
    }
}