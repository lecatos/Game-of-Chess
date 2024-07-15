using System.ComponentModel;

namespace Chess
{
    class PieceFactory
    {
        private static PieceFactory _instance;
        //Singleton PieceFactory object
        public static PieceFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PieceFactory();
            }
            return _instance;
        }
        private PieceFactory()
        {
        }
        public Piece Operate(PieceKind piece, bool isWhite)
        {
            return Operate(piece.ToString(), isWhite);
        }
        public Piece Operate(PromotionPiece piece, bool isWhite)
        {
            return Operate(piece.ToString(), isWhite);
        }
        public Piece Operate(string pieceKind, bool isWhite)
        {
            Piece piece;
            if (PieceKind.King.ToString() == pieceKind)
            {
                piece = new King(isWhite);
            }
            else if (PieceKind.Queen.ToString() == pieceKind)
            {
                piece = new Queen(isWhite);
            }
            else if (PieceKind.Bishop.ToString() == pieceKind)
            {
                piece = new Bishop(isWhite);
            }
            else if (PieceKind.Rook.ToString() == pieceKind)
            {
                piece = new Rook(isWhite);
            }
            else if (PieceKind.Knight.ToString() == pieceKind)
            {
                piece = new Knight(isWhite);
            }
            else if (PieceKind.Pawn.ToString() == pieceKind)
            {
                piece = new Pawn(isWhite);
            }
            else
            {
                piece = null;
            }
            return piece;
        }
    }
    
}
