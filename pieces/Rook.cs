namespace Chess
{
    public class Rook : Piece
    {
        public Rook(bool isWhite) : base(PieceKind.Rook, isWhite) { }
        public override List<IMove> GetPreMoves(Board board, Cell cell)
        {
            List<IMove> moves = new List<IMove>();
            int X = cell.Coord.X;
            int Y = cell.Coord.Y;
            bool N = true;
            bool E = true;
            bool S = true;
            bool W = true;
            for (int i = 1; i < 8; i++)
            {
                N = ProcessDirection(board, moves, N, new Coordinate(X, Y + i));
                E = ProcessDirection(board, moves, E, new Coordinate(X + i, Y));
                S = ProcessDirection(board, moves, S, new Coordinate(X, Y - i));
                W = ProcessDirection(board, moves, W, new Coordinate(X - i, Y));
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
