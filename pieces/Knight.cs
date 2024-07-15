namespace Chess
{
    public class Knight : Piece
    {
        public Knight(bool isWhite) : base(PieceKind.Knight, isWhite) { }
        public override List<IMove> GetPreMoves(Board board, Cell cell)
        {
            List<IMove> moves = new List<IMove>();
            int X = cell.Coord.X;
            int Y = cell.Coord.Y;
            //Candidate Coordinates
            List<Coordinate> coords = new List<Coordinate>()
            {
                new Coordinate(X-2, Y-1),
                new Coordinate(X-2, Y+1),
                new Coordinate(X-1, Y+2),
                new Coordinate(X-1, Y-2),
                new Coordinate(X+2, Y-1),
                new Coordinate(X+2, Y+1),
                new Coordinate(X+1, Y-2),
                new Coordinate(X+1, Y+2),
            };
            //Check if Listed coordinate can be moved to
            foreach (Coordinate coord in coords)
            {
                if (!coord.IsValid())
                {
                    continue;
                }
                Piece piece = board.GetCell(coord).ChessPiece;
                if (piece == null || piece != null && IsEnemy(piece))
                {
                    moves.Add(new NormalMove(board, coord, this));
                }
            }
            return moves;
        }
    }
}
