namespace Chess
{
    public class Bishop : Piece
    {
        public Bishop(bool isWhite) : base(PieceKind.Bishop, isWhite) { }
        public override List<IMove> GetPreMoves(Board board, Cell cell)
        {
            List<IMove> moves = new List<IMove>();
            int X = cell.Coord.X;
            int Y = cell.Coord.Y;

            bool NW = true;
            bool NE = true;
            bool SW = true;
            bool SE = true;
            for (int i = 1; i < 8; i++)
            {
                NW = ProcessDirection(board, moves, NW, new Coordinate(X - i, Y + i));
                SW = ProcessDirection(board, moves, SW, new Coordinate(X - i, Y - i));
                NE = ProcessDirection(board, moves, NE, new Coordinate(X + i, Y + i));
                SE = ProcessDirection(board, moves, SE, new Coordinate(X + i, Y - i));
            }
            return moves;
        }
        private bool ProcessDirection(Board board, List<IMove> moves, bool direction, Coordinate coord)
        {
            Cell c = board.GetCell(coord);
            if (direction && coord.IsValid() && (c.IsEmpty() || !c.IsEmpty() && IsEnemy(c.ChessPiece)))
            {
                moves.Add(new NormalMove(board, coord, this));
                return c.IsEmpty();
            }
            else
            {
                return false;
            }
        }
    }
}
