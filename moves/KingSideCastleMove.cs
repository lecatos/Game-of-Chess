using SplashKitSDK;

namespace Chess
{
    public class KingSideCastleMove : IMove
    {
        private Board _board;
        private Coordinate _coord;
        private Piece _piece;
        private SoundEffect _sound;
        private Piece _kingSideRook;
        private Coordinate _rookCoord;
        private List<NormalMove> _moves;
        private bool _pathIsSafe;
        public Piece ChessPiece
        {
            get { return _piece; }
        }
        public Coordinate Coord
        {
            get { return _coord; }
        }
        public KingSideCastleMove(Board board, Piece toCastle)
        {
            _board = board;
            _piece = toCastle;
            _sound = SplashKit.SoundEffectNamed("castle.wav");
            _moves = new List<NormalMove>();
            _pathIsSafe = true;
            if (toCastle.IsWhite)
            {
                _kingSideRook = _board.GetCell(7, 0).ChessPiece;
                _coord = new Coordinate(6, 0);
                _rookCoord = new Coordinate(5, 0);
            }
            else
            {
                _kingSideRook = _board.GetCell(7, 7).ChessPiece;
                _coord = new Coordinate(6, 7);
                _rookCoord = new Coordinate(5, 7);
            }
            _moves.Add(new NormalMove(_board, _coord, _piece, false));
            _moves.Add(new NormalMove(_board, _rookCoord, _kingSideRook, false));
        }
        public bool IsValid(bool simulate = false)
        {
            foreach (Piece enemy in _piece.GetEnemies(_board).ToList())
            {
                if (enemy.Kind == PieceKind.King)
                {
                    continue;
                }
                if (enemy.GetPreMove(_board, _board.GetCell(enemy), _board.GetCell(_rookCoord)) != null)
                {
                    _pathIsSafe = false;
                    break;
                }
            }
            return (
                _kingSideRook != null &&
                _kingSideRook.Kind == PieceKind.Rook &&
                !_piece.IsMoved &&
                !_piece.IsEnemy(_kingSideRook) &&
                !_kingSideRook.IsMoved &&
                _board.GetCell(_rookCoord).IsEmpty() &&
                _board.GetCell(_coord).IsEmpty() &&
                _board.Status != GameStatus.Checked &&
                _pathIsSafe
                );
        }
        public void Move(bool simulate)
        {
            foreach (NormalMove move in _moves)
            {
                move.Move(simulate);
            }
            if (!simulate)
            {
                _sound.Play();
            }
        }
        public void Revert()
        {
            foreach (NormalMove move in _moves)
            {
                move.Revert();
            }
        }
    }
}
