using System.Security.Cryptography.X509Certificates;

namespace baUHInia.Playground.Model.Utility
{
    public class Offset
    {
        public int I { get; set; }
        public sbyte X { get; set; }
        public sbyte Y { get; set; }

        public Offset(int i, sbyte x, sbyte y)
        {
            I = i;
            X = x;
            Y = y;
        }
        
        public (int x, int y) Coords => (X, Y);
    }
}