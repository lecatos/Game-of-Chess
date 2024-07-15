using SplashKitSDK;
using System.Drawing;

namespace Chess
{
    public enum PieceKind
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        King,
        Queen
    }
    public abstract class Piece
    {
        protected PieceKind _kind;
        private bool _isMoved;
        private bool _isWhite;
        protected MySprite _sprite;
        public bool IsMoved
        {
            get { return _isMoved; }
            set
            {
                if (value)
                {
                    _isMoved = true;
                }
            }
        }
        public bool IsWhite
        {
            get { return _isWhite; }
        }
        public PieceKind Kind
        {
            get { return _kind; }
        }
        public Piece(PieceKind kind, bool isWhite)
        {
            _kind = kind;
            _isWhite = isWhite;
            _isMoved = false;
            string name;
            if (_isWhite)
            {
                name = "w" + _kind.ToString() + ".png";
            } else
            {
                name = "b" + _kind.ToString() + ".png";
            }
            Bitmap bitmap = SplashKit.BitmapNamed(name);
            _sprite = new MySprite(bitmap);

        }
        public bool IsEnemy(Piece other)
        {
            return IsWhite != other.IsWhite;
        }
        public void Move(Cell to)
        {
            _isMoved = true;
            _sprite.MyAnimateMove(to.CX, to.CY);
        }
        public List<Piece> GetEnemies(Board board)
        {
            if (IsWhite)
            {
                return board.BlackPieces;
            }
            else
            {
                return board.WhitePieces;
            }
        }
        public IMove GetMove(Board board, Cell from, Cell to)
        {
            if (from.ChessPiece != this)
            {
                return null;
            }
            List<IMove> moves = GetMoves(board, from);
            return IMove.GetMove(moves, to.Coord);
        }
        //Filter for specific premove
        public IMove GetPreMove(Board board, Cell from, Cell to)
        {
            if (from.ChessPiece != this)
            {
                return null;
            }
            List<IMove> moves = GetPreMoves(board, from);
            return IMove.GetMove(moves, to.Coord);
        }
        //Filter for no checkmate
        public List<IMove> GetMoves(Board board, Cell cell)
        {
            List<IMove> moves = GetPreMoves(board, cell);

            //Cell from = cell;
            foreach (IMove move in moves.ToList())
            {
                board.Move(cell.Coord.X, cell.Coord.Y, move.Coord.X, move.Coord.Y, true);
                List<Piece> enemyPieces = GetEnemies(board);
                foreach (Piece piece in enemyPieces)
                {
                    List<IMove> enemyPreMoves = piece.GetPreMoves(board, board.GetCell(piece));
                    if (_kind == PieceKind.King && IMove.ContainsMove(enemyPreMoves, move.Coord))
                    {
                        moves.Remove(move);
                        break;
                    } else if (_kind != PieceKind.King && IMove.ContainsMove(enemyPreMoves, board.GetCell(board.GetKing(IsWhite)).Coord)){
                        moves.Remove(move);
                        break;
                    }
                }
                board.Revert();
            }
            return moves;
        }
        //GetPreMoves searches for moves without considering that the king will get Checked.
        public abstract List<IMove> GetPreMoves(Board board, Cell cell);
        public void Draw(Cell cell)
        {
            if (cell.ChessPiece == this)
            {
                if (!IsMoved)
                {
                    _sprite.MyMoveTo(cell.CX, cell.CY);
                }
                _sprite.MyUpdateMove();
                SplashKit.DrawSprite(_sprite);
            }
           
        }
    }
}
