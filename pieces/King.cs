
namespace Chess
{
    public class King : Piece
    {
        public King(bool isWhite) : base(PieceKind.King, isWhite) { }
        public override List<IMove> GetPreMoves(Board board, Cell cell)
        {
            List<IMove> moves = new List<IMove>();
            int X = cell.Coord.X;
            int Y = cell.Coord.Y;
            //Check for surrounding of the king
            for (int i = X-1; i <= X+1; i++)
            {
                for (int j = Y - 1; j <= Y + 1; j++)
                {
                    if (i == X && j == Y || !Coordinate.IsValid(i, j)) { continue; }
                    Piece piece = board.GetCell(i, j).ChessPiece;
                    if (piece == null || IsEnemy(piece))
                    {
                        moves.Add(new NormalMove(board, i, j, this));
                    }
                }
            }
            //Check for Castles
            QueenSideCastleMove QCMove = new QueenSideCastleMove(board, this);
            KingSideCastleMove KCMove = new KingSideCastleMove(board, this);
            if (QCMove.IsValid())
            {
                moves.Add(QCMove);
            }
            if (KCMove.IsValid())
            {
                moves.Add(KCMove);
            }
            return moves;
        }
    }
}
