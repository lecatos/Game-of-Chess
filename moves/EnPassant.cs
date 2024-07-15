using SplashKitSDK;

namespace Chess
{
    public class EnPassant : IMove
    {
        private Board _board;
        private Coordinate _coord;
        private Cell _oldCell;
        private Piece _piece;
        private Piece _captured;
        private bool _playSound;
        private Coordinate _upLeft;
        private Coordinate _upRight;
        private Coordinate _left;
        private Coordinate _right;
        private IMove _lastMove;
        private Cell _from;
        private Cell _capturedCell;

        public Piece ChessPiece
        {
            get { return _piece; }
        }
        public Coordinate Coord
        {
            get { return _coord; }
        }
        public EnPassant(Board board, Coordinate coord, Piece piece, bool playSound = true)
        {
            _board = board;
            _piece = piece;
            _oldCell = board.GetCell(_piece);
            _coord = coord;
            _captured = null;
            _playSound = playSound;
            _from = board.GetCell(piece);
            _lastMove = _board.GetLastMove();
            int X = _from.Coord.X;
            int Y = _from.Coord.Y;
            if (_piece.IsWhite)
            {
                _upLeft = new Coordinate(X - 1, Y + 1);
                _upRight = new Coordinate(X + 1, Y + 1);
                _left = new Coordinate(X - 1, Y );
                _right = new Coordinate(X + 1, Y);
            }
            else
            {
                _upLeft = new Coordinate(X + 1, Y - 1);
                _upRight = new Coordinate(X - 1, Y - 1);
                _left = new Coordinate(X + 1, Y);
                _right = new Coordinate(X - 1, Y);
            }
            if (_coord == _upLeft)
            {
                _capturedCell = _board.GetCell(_left);
            }
            else
            {
                _capturedCell = _board.GetCell(_right);
            }
        }
        public bool IsValid(bool simulate = false)
        {

            return _lastMove != null && _lastMove is DoublePushMove &&
                _capturedCell != null &&
                _lastMove.ChessPiece == _capturedCell.ChessPiece &&
                ((_left.IsValid() && _coord == _upLeft) ||
                (_right.IsValid() && _coord == _upRight)) &&
                _board.GetCell(_coord).IsEmpty() &&
                _piece.IsEnemy(_capturedCell.ChessPiece);

        }
        public void Move(bool simulate)
        {

            _from.Move(_board.GetCell(_coord), simulate);
            if (!simulate)
            {
                _captured = _capturedCell.ChessPiece;
                _capturedCell.ChessPiece = null;
                if (_playSound)
                {
                    SplashKit.SoundEffectNamed("capture.wav").Play();
                }
            }
        }
        public void Revert()
        {
            Cell from = _board.GetCell(_piece);
            from.Move(_oldCell, true);
        }
    }
}
