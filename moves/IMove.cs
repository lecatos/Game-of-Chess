namespace Chess
{
    public interface IMove
    {
        public Piece ChessPiece
        {
            get;
        }
        public Coordinate Coord
        {
            get;
        }
        public void Move(bool simulate = false);
        public bool IsValid(bool simulate = false);
        public void Revert();

        //Helper Static functions
        public static bool ContainsMove(List<IMove> moves, Coordinate coord)
        {
            return GetMove(moves, coord) != null;
        }
        public static IMove GetMove(List<IMove> moves, Coordinate coord)
        {
            foreach (IMove move in moves)
            {
                if (move.Coord == coord)
                {
                    return move;
                }
            };
            return null;
        }
    }
}
