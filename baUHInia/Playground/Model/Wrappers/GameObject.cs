using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Model.Wrappers
{
    public class GameObject
    {
        public readonly TileObject TileObject;
        public readonly float ChangeValue;
        public readonly int Price;

        public GameObject(TileObject tileObject, float changeValue, int price)
        {
            TileObject = tileObject;
            ChangeValue = changeValue;
            Price = price;
        }
    }
}