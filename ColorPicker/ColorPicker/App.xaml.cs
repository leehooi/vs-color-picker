using System;
using System.Diagnostics;
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
            CloseExistsProcess();
            this.Startup += App_Startup;
        }

        void CloseExistsProcess()
        {
            Process current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (process.MainModule.FileName == current.MainModule.FileName)
                    {
                        process.Kill();
                    }
                }
            }
        }

        public static Point StartUpPosition = new Point();
        public static System.Windows.Media.Color StartUpColor = System.Windows.Media.Colors.White;

        void App_Startup(object sender, StartupEventArgs e)
        {
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
