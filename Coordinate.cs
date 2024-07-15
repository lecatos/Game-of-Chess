namespace Chess
{
    public struct Coordinate
    {
        private int _x;
        private int _y;
        public static bool operator==(Coordinate A, Coordinate B)
        {
            return A.X == B.X && A.Y == B.Y;
        }
        public static bool operator!=(Coordinate A, Coordinate B)
        {
            return A == B;
        }
        public (int x, int y) GetTransform()
        {
            return (_x, 7-_y);
        }
        public int X
        {
            get { return _x; }
        }
        public int Y
        {
            get { return _y; }
        }
        public Coordinate(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public static bool IsValid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < 8 && y < 8;
        }
        public bool IsValid()
        {
            return IsValid(_x, _y);
        }
    }
}
