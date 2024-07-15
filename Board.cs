using SplashKitSDK;
using System.Data;
using System.Xml;

namespace Chess
{
    public enum GameStatus
    {
        WhiteWins,
        BlackWins,
        Stalemate,
        InProgress,
        Checked,
        Draw
    }
    public class Board
    {
        private Grid _grid;
        private Cell _activeCell;
        private bool _isWhiteTurn;
        private GameStatus _status;
        public List<IMove> Moves;

        public GameStatus Status { get { return _status; } }
        public bool IsWhiteTurn
        {
            get { return _isWhiteTurn; }
        }
        public List<Piece> WhitePieces
        {
            get
            {
                List<Piece> pieces = new List<Piece>();
                foreach (Cell cell in _grid.Cells)
                {
                    if (!cell.IsEmpty() && cell.ChessPiece.IsWhite)
                    {
                        pieces.Add(cell.ChessPiece);
                    }
                }
                return pieces;
            }
        }
        public List<Piece> BlackPieces
        {
            get
            {
                List<Piece> pieces = new List<Piece>();
                foreach (Cell cell in _grid.Cells)
                {
                    if (!cell.IsEmpty() && !cell.ChessPiece.IsWhite)
                    {
                        pieces.Add(cell.ChessPiece);
                    }
                }
                return pieces;
            }
        }
        public King GetKing(bool isWhite)
        {
            List<Piece> pieces;
            if (isWhite)
            {
                pieces = WhitePieces;
            } else
            {
                pieces = BlackPieces;
            }
            foreach (Piece p in pieces)
            {
                if (p is King)
                {
                    return p as King;
                }
            }
            return null;
        }
        public Board()
        {
            _grid = new Grid();
            _activeCell = null;
            _isWhiteTurn = true;
            _status = GameStatus.InProgress;
            Moves = new List<IMove>();
            UpdateStatus();
        }
        public IMove GetLastMove()
        {
            if (Moves.Count > 0)
            {
                return Moves.Last();
            } else
            {
                return null;
            }
        }
        public Cell GetCell(Coordinate coord)
        {
            return _grid.GetCell(coord);
        }
        public Cell GetCell(int x, int y)
        {
            return _grid.GetCell(x, y);
        }
        public Cell GetCell(Piece chessPiece)
        {
            return _grid.GetCell(chessPiece);
        }
        public void Move(int x1, int y1, int x2, int y2, bool simulate = false)
        {
            Cell from = GetCell(x1, y1);
            Cell to = GetCell(x2, y2);
            if (from != null && to != null)
            {
                Move(from, to, simulate);
            }
        }
        private void Move(Cell from, Cell to, bool simulate = false)
        {
            if (simulate || (from.ChessPiece.IsWhite == _isWhiteTurn && from.IsValidMove(this, to)))
            {
                IMove move = from.GetMove(this, to, simulate);
                Moves.Add(move);
                move.Move(simulate);
                if (!simulate)
                {
                    Clear();
                    UpdateStatus(); 
                    _isWhiteTurn = !_isWhiteTurn;
                }
            }
        }
        public void Revert()
        {
            if (Moves.Count > 0)
            {
                IMove move = Moves.Last();
                move.Revert();
                Moves.RemoveAt(Moves.Count - 1);
            }
        }
        private void UpdateStatus()
        {
            King king = GetKing(!_isWhiteTurn);
            List<Piece> enemyPieces;
            List<Piece> allyPieces;
            bool checkmate = false;
            bool stalemate = true;
            if (king == null)
            {
                _status = GameStatus.InProgress;
                return;
            }
            if (_isWhiteTurn)
            {
                allyPieces = WhitePieces;
                enemyPieces = BlackPieces;
            } else
            {
                allyPieces = BlackPieces;
                enemyPieces = WhitePieces;
            }
            foreach (Piece piece in allyPieces)
            {
                if (IMove.ContainsMove(piece.GetMoves(this, GetCell(piece)), GetCell(king).Coord))
                {
                    checkmate = true;
                    _status = GameStatus.Checked;
                    GetCell(king).IsChecked = true;
                    SplashKit.FadeAllSoundEffectsOut(60);
                    SplashKit.SoundEffectNamed("check.wav").Play();
                    break;
                }
            }
            foreach (Piece piece in enemyPieces)
            {
                if (piece.GetMoves(this, GetCell(piece)).Count > 0)
                {
                    stalemate = false;
                    break;
                }
            }
            if (checkmate && stalemate)
            {
                if (_isWhiteTurn)
                {
                    _status = GameStatus.WhiteWins;
                } else
                {
                    _status = GameStatus.BlackWins;
                }
                
                SplashKit.SoundEffectNamed("notify.wav").Play();
            } else if (checkmate)
            {
                _status = GameStatus.Checked;
            }
                else if (stalemate)
            {
                _status = GameStatus.Stalemate;
                SplashKit.SoundEffectNamed("notify.wav").Play();
            } else
            {
                _status = GameStatus.InProgress;
            }
        }
        //Select or Unselect cell
        private void Toggle(Cell cell)
        {
            if (!cell.IsEmpty())
            {
                if (_activeCell != cell)
                {
                    Clear();
                }
                if (cell.ChessPiece.IsWhite != _isWhiteTurn)
                {
                    return;
                }
                cell.IsSelected = !cell.IsSelected;
                List<IMove> moves = cell.GetMoves(this);
                foreach (IMove move in moves)
                {
                    GetCell(move.Coord).IsDotted = cell.IsSelected;
                }
                if (cell.IsSelected)
                {
                    _activeCell = cell;
                }
                else
                {
                    _activeCell = null;
                }
            } else if (!cell.IsDotted)
            {
                Clear();
            }
        }
        private void Move(Cell to)
        {
            if (to.IsDotted)
            {
                ClearAll();
                Cell from = _activeCell;
                Move(_activeCell, to);
                from.IsHighLighted = true;
                to.IsHighLighted = true;
            }
        }
        public void ClickAt(Point2D pt)
        {
            Cell cell = _grid.SelectAt(pt);
            if (cell != null)
            {
                Move(cell);
                Toggle(cell);
            }
        }
        public void LoadFrom(string file)
        {
            if (Path.Exists(file))
            {
                StreamReader reader = new StreamReader(file);
                try
                {
                    _isWhiteTurn = reader.ReadBoolean();
                    _status = (GameStatus)Enum.Parse(typeof(GameStatus), reader.ReadLine());
                    _grid.LoadFrom(reader);
                } catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    NewGame();
                } finally
                {
                    SplashKit.SoundEffectNamed("notify.wav").Play();
                    reader.Close();
                }
            } else
            {
                NewGame();
            }
        }
        public void SaveTo(string file)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            StreamWriter writer = new StreamWriter(file);
            try
            {
                writer.WriteLine(_isWhiteTurn);
                writer.WriteLine(_status.ToString());
                _grid.SaveTo(writer);
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            } finally
            {
                writer.Close();
            }

        }
        public void NewGame()
        {
            _isWhiteTurn = true;
            SplashKit.SoundEffectNamed("notify.wav").Play();
            _grid.NewGame();
            UpdateStatus();
        }
        public void Draw()
        {
            _grid.Draw();
        }
        private void Clear()
        {
            _activeCell = null;
            _grid.Clear();
        }
        private void ClearAll()
        {
            _grid.ClearAll();
        }
    }
}
