using System;
using System.Threading;

namespace ColorPicker
{
    public class ScreenColorSampler
    {
        private Thread _thread;
        private bool _isDetecting;
        public event Action<int> ColorPicked;
        public ScreenColorSampler()
        {
            _isDetecting = false;   
        }

        public bool IsWorking { get { return _isDetecting; } }

        public void Start()
        {
            if (_thread != null)
            {
                _thread.Abort();
            }
            _isDetecting = true;
            _thread = new Thread(DetectMouseMoving);
            _thread.Start();
        }

        private void DetectMouseMoving(object obj)
        {
            var position = System.Windows.Forms.Cursor.Position;
            bool isCaptured = false;
            while (_isDetecting)
            {
                if (position.Equals(System.Windows.Forms.Cursor.Position))
                {
                    if (!isCaptured)
                    {
                        var color = GetScreenPixelColor(position.X, position.Y);
                        if (ColorPicked != null)
                        {
                            ColorPicked(color);
                        }
                        isCaptured = true;
                    }
                }
                else
                {
                    position = System.Windows.Forms.Cursor.Position;
                    isCaptured = false;
                }
                Thread.Sleep(100);
            }
        }

        private int GetScreenPixelColor(int x, int y)
        {
            var dc = WinApi.GetDC(0);
            return WinApi.GetPixel(dc, x, y);
        }

        public void Stop()
        {
            _isDetecting = false;
        }
    }
}
