using Chess.Properties;
using SplashKitSDK;

namespace Chess
{
    public class Game
    {
        private Window _window;
        private Board _board;
        private GameStatus _status;
        private string _filename;
        private string _path;

        private static Game _instance;
        //Singleton Game object
        public static Game GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Game();
            }
            return _instance;
        }
        private Game()
        {
            //Resource Path
            _path = "resources";
            _filename = "save.txt";
            Setup();
            _window = new Window("Game of Chess", 1000, 800);
            _board = new Board();
            _status = _board.Status;
            NewGame();
            
        }
        private void Setup()
        {            
            string[] resources = { "wKing.png", "wQueen.png", "wKnight.png", "wBishop.png", "wRook.png", "wPawn.png",
            "bKing.png", "bQueen.png", "bKnight.png", "bBishop.png", "bRook.png", "bPawn.png",
            "notify.wav", "move.wav", "capture.wav", "castle.wav", "check.wav", "promote.wav"
            };
            Directory.CreateDirectory(_path);
            //Writing resources to disk
            foreach (string resourceName in resources)
            {
                Stream output = File.Create(Path.Combine(_path, resourceName));
                //Console.WriteLine(resourceName);
                Object res = Resources.ResourceManager.GetObject(resourceName);
                Stream source;
                if ((res.GetType().Name == "Byte[]"))
                {
                    source = new MemoryStream((byte[])res);
                } else
                {
                    source = (MemoryStream)res;
                }

                source.CopyTo(output);
                source.Close();
                output.Close();
                //Loading Resources
                if (Path.GetExtension(resourceName) == ".wav")
                {
                    SplashKit.LoadSoundEffect(resourceName, Path.Combine(_path, resourceName));
                } else if (Path.GetExtension(resourceName) == ".png")
                {
                    SplashKit.LoadBitmap(resourceName, Path.Combine(_path, resourceName));
                }
            }
            SplashKit.LoadFont("BAUHS", "BAUHS93.ttf");
        }
        public void Run()
        {
            do
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen(Color.LightSeaGreen);
                if (SplashKit.MouseClicked(MouseButton.LeftButton))
                {
                    Point2D pos = SplashKit.MousePosition();
                    _board.ClickAt(pos);
                    ClickAt(pos);
                    Console.WriteLine(_board.Status.ToString());
                }
                _board.Draw();
                _status = _board.Status;
                Draw();
                switch (_status)
                {
                    case GameStatus.WhiteWins:
                        DrawMessage("White Wins!!!");
                        break;
                    case GameStatus.BlackWins:
                        DrawMessage("Black Wins!!!");
                        break;
                    case GameStatus.Stalemate:
                        DrawMessage("Stale MATE !!!");
                        break;
                    case GameStatus.Draw:
                        DrawMessage("DRAW !!!");
                        break;
                    default:
                        break;
                }
                SplashKit.RefreshScreen();
            } while (!_window.CloseRequested);
        }
        private void ClickAt(Point2D point)
        {
            if (point.X > 810 && point.X < 990 && point.Y > 650 && point.Y < 700)
            {
                NewGame();
            }
            if (point.X > 810 && point.X < 990 && point.Y > 730 && point.Y < 780)
            {
                SaveAndQuit();
            }
            if (point.X > 810 && point.X < 990 && point.Y > 570 && point.Y < 620)
            {
                LoadFromFile();
            }
        }
        private void Draw()
        {
            SplashKit.FillRectangle(Color.AliceBlue, 810, 650, 180, 50);
            SplashKit.DrawText("New Game", Color.DarkRed, "BAUHS", 20, 820, 660);
            SplashKit.FillRectangle(Color.AliceBlue, 810, 730, 180, 50);
            SplashKit.DrawText("Save & Quit", Color.DarkRed, "BAUHS", 20, 820, 740);
            SplashKit.FillRectangle(Color.AliceBlue, 810, 570, 180, 50);
            SplashKit.DrawText("Load Game", Color.DarkRed, "BAUHS", 20, 820, 580);
            string turn;
            if (_board.IsWhiteTurn)
            {
                turn = "White's Turn!";
            } else
            {
                turn = "Black's Turn!";
            }
            SplashKit.DrawText(turn, Color.GhostWhite, "BAUHS", 30, 820, 100);
        }
        private void DrawMessage(string text)
        {
            SplashKit.FillRectangle(Color.RGBAColor(127, 0, 0, 190), 50, 150, 900, 500);
            SplashKit.DrawText(text, Color.AntiqueWhite, "BAUHS", 50, 210, 350);

        }
        private void LoadFromFile()
        {
            _board.LoadFrom(Path.Combine(_path, _filename));
        }
        private void NewGame()
        {
            _board.NewGame();
        }
        private void SaveAndQuit()
        {
            _board.SaveTo(Path.Combine(_path, _filename));
            Environment.Exit(0);
        }

    }
}
