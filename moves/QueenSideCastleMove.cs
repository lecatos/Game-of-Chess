using SplashKitSDK;
using System.ComponentModel;
using System;

namespace Chess
{
    public class QueenSideCastleMove : IMove
    {
        private Board _board;
        private Coordinate _coord;
        private Piece _piece;
        private SoundEffect _sound;
        private Piece _queenSideRook;
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
        public QueenSideCastleMove(Board board, Piece toCastle)
        {
            _board = board;
            _piece = toCastle;
            _moves = new List<NormalMove>();
            _sound = SplashKit.SoundEffectNamed("castle.wav");
            _pathIsSafe = true;
            if (toCastle.IsWhite)
            {
                _queenSideRook = _board.GetCell(0, 0).ChessPiece;
                _coord = new Coordinate(2, 0);
                _rookCoord = new Coordinate(3, 0);
                if (!_board.GetCell(1, 0).IsEmpty())
                {
                    _pathIsSafe = false;
                }
            }
            else
            {
                _queenSideRook = _board.GetCell(0, 7).ChessPiece;
                _coord = new Coordinate(2, 7);
                _rookCoord = new Coordinate(3, 7);
                if (!_board.GetCell(1, 7).IsEmpty())
                {
                    _pathIsSafe = false;
                }
            }
            _moves.Add(new NormalMove(_board, _coord, _piece, false));
            _moves.Add(new NormalMove(_board, _rookCoord, _queenSideRook, false));

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
                _queenSideRook != null &&
                _queenSideRook.Kind == PieceKind.Rook &&
                !_piece.IsMoved &&
                !_piece.IsEnemy(_queenSideRook) &&
                !_queenSideRook.IsMoved &&
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
