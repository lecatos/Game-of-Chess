using SplashKitSDK;
using System.Xml.Linq;

namespace Chess
{
    public class Pawn : Piece, IPromotable
    {
        public Pawn(bool isWhite) : base(PieceKind.Pawn, isWhite) { }
        public override List<IMove> GetPreMoves(Board board, Cell cell)
        {
            List<IMove> moves = new List<IMove>();
            int X = cell.Coord.X;
            int Y = cell.Coord.Y;
            Coordinate up;
            Coordinate upUp;
            Coordinate upLeft;
            Coordinate upRight;
            if (IsWhite)
            {
                up = new Coordinate(X, Y + 1);
                upUp = new Coordinate(X, Y + 2);
                upLeft = new Coordinate(X - 1, Y + 1);
                upRight = new Coordinate(X + 1, Y + 1);
            } else
            {
                up = new Coordinate(X, Y - 1);
                upUp = new Coordinate(X, Y - 2);
                upLeft = new Coordinate(X + 1, Y - 1);
                upRight = new Coordinate(X - 1, Y - 1);
            }
            //Check the above cell
            if (up.IsValid() && board.GetCell(up).IsEmpty())
            {
                if (!upUp.IsValid())
                {
                    moves.Add(new PromotionMove(board, up, this));
                } else
                {
                    moves.Add(new NormalMove(board, up, this));
                }
                DoublePushMove dpush = new DoublePushMove(board, cell, this);
                if (dpush.IsValid())
                {
                    moves.Add(dpush);
                }
            }
            //Check up left cell
            Cell tmp_cell = board.GetCell(upLeft);
            if (upLeft.IsValid() && !tmp_cell.IsEmpty() && IsEnemy(tmp_cell.ChessPiece))
            {
                if (!upUp.IsValid())
                {
                    moves.Add(new PromotionMove(board, upLeft, this));
                }
                else
                {
                    moves.Add(new NormalMove(board, upLeft, this));
                }
            }
            EnPassant ep = new EnPassant(board, upLeft, this);
            if (ep.IsValid())
            {
                moves.Add(ep);
            }
            //Check up right
            tmp_cell = board.GetCell(upRight);
            if (upRight.IsValid() && !tmp_cell.IsEmpty() && IsEnemy(tmp_cell.ChessPiece))
            {
                if (!upUp.IsValid())
                {
                    moves.Add(new PromotionMove(board, upRight, this));
                } 
                else
                {
                    moves.Add(new NormalMove(board, upRight, this));
                }
            }
            ep = new EnPassant(board, upRight, this);
            if (ep.IsValid())
            {
                moves.Add(ep);
            }
            return moves;
        }
        public PromotionPiece Promote()
        {
            return PromotionPiece.Queen;
        }
    }
}
