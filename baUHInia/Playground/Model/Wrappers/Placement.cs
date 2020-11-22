namespace baUHInia.Playground.Model.Wrappers
{
    public class Placement
    {
        public readonly GameObject GameObject;
        public (int x, int y) Position { get; set; }

        public Placement(GameObject gameObject, (int x, int y) position)
        {
            GameObject = gameObject;
            Position = position;
        }
    }
}