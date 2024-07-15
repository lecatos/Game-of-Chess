using SplashKitSDK;

namespace Chess
{
    public class NormalMove : IMove
    {
        protected Board _board;
        protected Coordinate _coord;
        protected Cell _oldCell;
        protected Piece _piece;
        protected Piece _captured;
        protected SoundEffect _sound;
        protected bool _playSound;
        public Piece ChessPiece
        {
            get { return _piece; }
        }
        public Coordinate Coord
        {
            get { return _coord; }
        }

        public NormalMove(Board board, int x, int y, Piece piece) :
            this(board, new Coordinate(x, y), piece)
        { }
        public NormalMove(Board board, Coordinate coord, Piece piece, bool playSound=true)
        {
            _board = board;
            _piece = piece;
            _oldCell = board.GetCell(_piece);
            _coord = coord;
            _captured = null;
            _sound = SplashKit.SoundEffectNamed("move.wav");
            _playSound = playSound;
        }
        public virtual void Move(bool simulate)
        {
            Cell to = _board.GetCell(_coord);
            _captured = to.ChessPiece;
            _board.GetCell(_piece).Move(to, simulate);
            if (!simulate && _playSound && _captured == null)
            {
                _sound.Play();
            }
        }
        public virtual void Revert()
        {
            Cell from = _board.GetCell(_piece);
            from.Move(_oldCell, true);
            from.ChessPiece = _captured;
        }
        public bool IsValid(bool simulate = false)
        {
            return _coord.IsValid();
        }
    }
}
