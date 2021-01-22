namespace baUHInia.Playground.Model.Utility
{
    public class Offset
    {
        public int I { get; }
        public sbyte X { get; }
        public sbyte Y { get; }

        public Offset(int i, sbyte x, sbyte y)
        {
            I = i;
            X = x;
            Y = y;
        }
        
        public (int x, int y) Coords => (X, Y);
    }
}