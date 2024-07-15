namespace Chess
{
    public class DoublePushMove : IMove
    {
        private Board _board;
        private Coordinate _coord;
        private Piece _piece;
        private NormalMove _move;
        private Cell _from;
        public Piece ChessPiece
        {
            get { return _piece; }
        }
        public Coordinate Coord
        {
            get { return _coord; }
        }
        public DoublePushMove(Board board, Cell from, Piece piece)
        {
            _board = board;
            _piece = piece;
            _from = from;
            if (_piece.IsWhite)
            {
                _coord = new Coordinate(from.Coord.X, from.Coord.Y + 2);
            }
            else
            {
                _coord = new Coordinate(from.Coord.X, from.Coord.Y - 2);
            }
            _move = new NormalMove(_board, _coord, _piece);

        }
        public void Move(bool simulate)
        {
            _move.Move(simulate);
        }
        public void Revert()
        {
            _move.Revert();
        }
        public bool IsValid(bool simulate = false)
        {
            return _move.IsValid() && !_piece.IsMoved && _from.ChessPiece == _piece && _board.GetCell(_coord).IsEmpty();
        }
    }
}
