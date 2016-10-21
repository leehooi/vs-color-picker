using System;
using System.Windows;

namespace ColorPicker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        public static Point StartUpPosition = new Point();
        public static System.Windows.Media.Color StartUpColor = System.Windows.Media.Colors.White;

        void App_Startup(object sender, StartupEventArgs e)
        {
            //var rect = new WinApi.Rect();
            //WinApi.GetWindowRect(WinApi.GetForegroundWindow(), ref rect);
            //StartUpPosition.X = rect.Left;
            //StartUpPosition.Y = rect.Top;
            //if (e.Args.Length == 2)
            //{
            //    StartUpPosition.X += Int32.Parse(e.Args[0]);
            //    StartUpPosition.Y += Int32.Parse(e.Args[1]);
            //}
            try
            {
                StartUpColor = e.Args[0].ToColor();
            }
            catch { }
            StartUpPosition.X = System.Windows.Forms.Cursor.Position.X;
            StartUpPosition.Y = System.Windows.Forms.Cursor.Position.Y;
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
