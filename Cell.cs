using SplashKitSDK;
using Color = SplashKitSDK.Color;

namespace Chess
{
    public class Cell
    {
        private Coordinate _coord;
        private Color _cellColor;
        private Piece _chessPiece;
        private int _size;
        private int _x;
        private int _y;
        public bool IsHighLighted;
        public bool IsSelected;
        public bool IsDotted;
        public bool IsChecked;
        public Piece ChessPiece
        {
            get {  return _chessPiece;}
            set { _chessPiece = value; }
        }
        public Coordinate Coord
        {
            get { return _coord; }
        }
        public int X
        {
            get { return _x; }
        }
        public int Y
        {
            get { return _y; }
        }
        public int CX
        {
            get { return _x + _size / 2; }
        }
        public int CY
        {
            get { return _y + _size / 2; }
        }
        public bool IsEmpty()
        {
            return ChessPiece == null;
        }
        public void Move(Cell to, bool simulate)
        {
            if (!simulate)
            {
               _chessPiece.Move(to);
                if (!to.IsEmpty())
                {
                    SplashKit.SoundEffectNamed("capture.wav").Play();
                }
            }
            to.ChessPiece = _chessPiece;
            _chessPiece = null;
        }
        public Color CellColor {  get { return _cellColor; } }
        public Cell(Color cellColor, Coordinate coord) : this(cellColor, coord, null)
        {
        }
        public Cell(Color cellColor, Coordinate coord, Piece chessPiece)
        {
            _cellColor = cellColor;
            _coord = coord;
            _chessPiece = chessPiece;
            _size = 100;
            (int new_x, int new_y) = coord.GetTransform();
            _x = new_x * _size;
            _y = new_y * _size;
            IsSelected = false;
            IsChecked = false;
            IsHighLighted = false;
        }
        public bool IsValidMove(Board board, Cell to)
        {
            if (GetMove(board, to) != null)
            {
                return true;
            }
            return false;
        }
        public List<IMove> GetMoves(Board board)
        {
            if (IsEmpty())
            {
                return null;
            } else
            {
                return _chessPiece.GetMoves(board, this);
            }
        }
        public IMove GetMove(Board board, Cell to, bool simulate = false)
        {
            if (IsEmpty())
            {
                return null;
            }
            else
            {
                if (!simulate)
                {
                    return _chessPiece.GetMove(board, this, to);
                } else
                {
                    return _chessPiece.GetPreMove(board, this, to);
                }
            }
        }
        public bool IsAt(Point2D pt)
        {
            return (_x <= pt.X & _x + _size >= pt.X & _y <= pt.Y & _y + _size >= pt.Y);
        }
        public void Draw()
        {
            
            Color hightLightColor = Color.RGBAColor(255, 255, 0, 127);
            Color dotColor = Color.RGBAColor(127, 127, 127, 127);
            Color outlineColor = Color.DeepSkyBlue;
            int X = _x + 2;
            int Y = _y - 2;

            SplashKit.FillRectangle(outlineColor, _x, _y, _size, _size);
            if (IsSelected || IsHighLighted)
            {
                SplashKit.FillRectangle(_cellColor, X, Y, _size, _size);
                SplashKit.FillRectangle(hightLightColor, X, Y, _size, _size);
            }
            else if (IsChecked)
            {
                SplashKit.FillRectangle(Color.SwinburneRed, X, Y, _size, _size);
            }
            else
            {
                SplashKit.FillRectangle(_cellColor, X, Y, _size, _size);
            }
            if (IsDotted)
            {
                if (IsEmpty())
                {
                    SplashKit.FillCircle(dotColor, CX, CY, 10);
                } else
                {
                    SplashKit.FillCircle(Color.SwinburneRed, CX, CY, 50);
                    if (IsSelected || IsHighLighted)
                    {
                        SplashKit.FillCircle(_cellColor, CX, CY, 40);
                        SplashKit.FillCircle(hightLightColor, CX, CY, 40);
                    } else if (IsChecked)
                    {
                        SplashKit.FillCircle(Color.SwinburneRed, CX, CY, 40);
                    } else
                    {
                        SplashKit.FillCircle(_cellColor, CX, CY, 40);
                    }

                }
            }
        }
    }

}
