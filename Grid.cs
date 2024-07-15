using SplashKitSDK;

namespace Chess
{
    public class Grid
    {
        private Cell[,] _grid;
        public Cell[,] Cells
        {
            get { return _grid; }
        }
        public Grid()
        {
            _grid = new Cell[8, 8];
            ClearCells();
        }
        public Cell GetCell(int x, int y)
        {
            if (Coordinate.IsValid(x, y))
            {
                return _grid[x, y];
            }
            else
            {
                return null;
            }
        }
        public Cell GetCell(Coordinate coord)
        {
            return GetCell(coord.X, coord.Y);
        }
        public Cell GetCell(Piece chessPiece)
        {
            foreach (Cell cell in _grid)
            {
                if (cell.ChessPiece == chessPiece)
                {
                    return cell;
                }
            }
            return null;
        }
        public void SetCell(int x, int y, Piece piece)
        {
            GetCell(x, y).ChessPiece = piece;
        }
        public void LoadFrom(StreamReader reader)
        {
            for (int i = 0; i < _grid.Length; i++)
            {
                Cell cell = reader.ReadCell();
                _grid[cell.Coord.X, cell.Coord.Y] = cell;
            }
        }
        public void SaveTo(StreamWriter writer)
        {
            foreach (Cell cell in _grid)
            {
                writer.WriteCell(cell);
            }
        }
        public void Draw()
        {
            foreach (Cell cell in _grid)
            {
                cell.Draw();
            }
            foreach (Cell cell in _grid)
            {
                if (!cell.IsEmpty())
                {
                    cell.ChessPiece.Draw(cell);
                }
            }
        }
        //Clear selected and dotted cells
        public void Clear()
        {
            foreach (Cell cell in _grid)
            {
                cell.IsSelected = false;
                cell.IsDotted = false;
            }
        }
        //Clear all cells properties
        public void ClearAll()
        {
            foreach (Cell cell in _grid)
            {
                cell.IsSelected = false;
                cell.IsDotted = false;
                cell.IsHighLighted = false;
                cell.IsChecked = false;
            }
        }
        //Initialize new cells
        private void ClearCells()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0))
                    {
                        _grid[i, j] = new Cell(Color.LightGreen, new Coordinate(i, j));
                    }
                    else
                    {
                        _grid[i, j] = new Cell(Color.LightGoldenrodYellow, new Coordinate(i, j));
                    }
                }
            }
        }

        public Cell SelectAt(Point2D pt)
        {
            foreach(Cell cell in _grid)
            {
                if (cell.IsAt(pt))
                {
                    return cell;
                }
            };
            return null;
        }
        //Setup new game
        public void NewGame()
        {
            ClearCells();
            SetCell(0, 0, new Rook(true));
            SetCell(1, 0, new Knight(true));
            SetCell(2, 0, new Bishop(true));
            SetCell(3, 0, new Queen(true));
            SetCell(4, 0, new King(true));
            SetCell(5, 0, new Bishop(true));
            SetCell(6, 0, new Knight(true));
            SetCell(7, 0, new Rook(true));
            SetCell(0, 1, new Pawn(true));
            SetCell(1, 1, new Pawn(true));
            SetCell(2, 1, new Pawn(true));
            SetCell(3, 1, new Pawn(true));
            SetCell(4, 1, new Pawn(true));
            SetCell(5, 1, new Pawn(true));
            SetCell(6, 1, new Pawn(true));
            SetCell(7, 1, new Pawn(true));

            SetCell(0, 7, new Rook(false));
            SetCell(1, 7, new Knight(false));
            SetCell(2, 7, new Bishop(false));
            SetCell(3, 7, new Queen(false));
            SetCell(4, 7, new King(false));
            SetCell(5, 7, new Bishop(false));
            SetCell(6, 7, new Knight(false));
            SetCell(7, 7, new Rook(false));
            SetCell(0, 6, new Pawn(false));
            SetCell(1, 6, new Pawn(false));
            SetCell(2, 6, new Pawn(false));
            SetCell(3, 6, new Pawn(false));
            SetCell(4, 6, new Pawn(false));
            SetCell(5, 6, new Pawn(false));
            SetCell(6, 6, new Pawn(false));
            SetCell(7, 6, new Pawn(false));
        }
    }
}
