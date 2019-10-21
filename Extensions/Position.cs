namespace Ciphers.Extensions
{
    internal class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int y, int x)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
