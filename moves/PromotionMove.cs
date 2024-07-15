using SplashKitSDK;

namespace Chess
{
    public enum PromotionPiece
    {
        Rook,
        Knight,
        Bishop,
        Queen
    }
    public class PromotionMove : IMove
    {
        private Board _board;
        private Coordinate _coord;
        private Coordinate _oldCoord;
        private IPromotable _piece;
        private NormalMove _move;
        private SoundEffect _sound;
        public Piece ChessPiece
        {
            get { return (Piece)_piece; }
        }
        public Coordinate Coord
        {
            get { return _coord; }
        }
        public PromotionMove(Board board, Coordinate coord, IPromotable piece)
        {
            _board = board;
            _piece = piece;
            _coord = coord;
            _oldCoord = board.GetCell(ChessPiece).Coord;
            _move = new NormalMove(_board, _coord, ChessPiece);
            _sound = SplashKit.SoundEffectNamed("promote.wav");

        }
        public void Move(bool simulate)
        {
            if (!simulate)
            {
                Piece newPiece = PieceFactory.GetInstance().Operate(_piece.Promote(), ChessPiece.IsWhite);
                _board.GetCell(_oldCoord).ChessPiece = newPiece;
                _board.Draw();
                _move = new NormalMove(_board, _coord, newPiece, false);
                _sound.Play();
            }
            _move.Move(simulate);
        }
        public void Revert()
        {
            _move.Revert();
        }
        public bool IsValid(bool simulate = false)
        {
            return _move.IsValid();
        }
    }
}
