namespace baUHInia.Playground.Model
{
    public class Offset
    {
        public byte I { get; }
        public sbyte X { get; }
        public sbyte Y { get; }

        public Offset(byte i, sbyte x, sbyte y)
        {
            I = i;
            X = x;
            Y = y;
        }
    }
}