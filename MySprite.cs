using SplashKitSDK;
using Timer = SplashKitSDK.Timer;

namespace Chess
{
    public class MySprite : Sprite
    {
        private Point2D _dest;
        private Point2D _prev;
        private Timer _timer;
        private bool _isMoved;
        private static int _counter = 0;
        public MySprite(Bitmap layer) : base(layer)
        {
            _dest = new Point2D();
            _prev = new Point2D();
            _isMoved = false;
            _timer = new Timer(_counter++.ToString());
        }
        public void MyAnimateMove(int x, int y)
        {
            _dest.X = x - Width / 2;
            _dest.Y = y - Width / 2;
            if (!_isMoved)
            {
                _prev.X = _dest.X;
                _prev.Y = _dest.Y;
            } else
            {
                _prev.X = X;
                _prev.Y = Y;
            }
            _timer.Start();
        }
        public void MyMoveTo(double x, double y)
        {
            MoveTo(x - Width/2, y - Height/2);
            _isMoved = true;
        }
        public void MyUpdateMove()
        {
            if (_timer.IsStarted)
            {
                uint time = 100;
                //Console.WriteLine(_dest.X.ToString() + ". " + ((_dest.X - _prev.X) * _timer.Ticks / time + _prev.X).ToString() + " " + ((_dest.Y - _prev.Y) * _timer.Ticks / time + _prev.Y).ToString());
                uint tick = _timer.Ticks;
                if (tick >= time)
                {
                    tick = time;
                    _timer.Stop();
                }
                MoveTo((_dest.X - _prev.X) * tick / time + _prev.X, (_dest.Y - _prev.Y) * tick / time + _prev.Y);
            }
        }
    }
}
