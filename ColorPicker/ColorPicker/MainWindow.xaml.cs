using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ColorPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsForegroundWindow
        {
            get
            {
                return WinApi.GetForegroundWindow() == new WindowInteropHelper(this).Handle;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.Top = App.StartUpPosition.Y;
            this.Left = App.StartUpPosition.X;

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Closing += MainWindow_Closing;
            this.KeyUp += MainWindow_KeyUp;

            this.Deactivated += MainWindow_Deactivated;

            new Timer(CheckWindowIsFallBehandTimer, null, 0, 500);

            btnFromScreen.Click += btnFromScreen_Click;
            btnPalette.Click += btnPalette_Click;
            panelCurrentColor.PreviewMouseMove += DragElement_PreviewMouseMove;
            labelCurrentColor.PreviewMouseMove += DragElement_PreviewMouseMove;

            ShowColor(App.StartUpColor);
        }

        void CheckWindowIsFallBehandTimer(object state)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (!this.IsForegroundWindow)
                {
                    MainWindow_Deactivated(null, null);
                }
            });
        }

        void MainWindow_Deactivated(object sender, EventArgs e)
        {
            if (_isPaletteShowing)
            {
                return;
            }
            this.Close();
        }

        void DragElement_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var element = sender as FrameworkElement;
            element.Cursor = Cursors.SizeAll;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                App.Current.MainWindow.DragMove();
            }
        }

        void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Enter:
                    Console.Write(labelCurrentColor.Text);
                    this.Close();
                    break;
                default:
                    return;
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (btnFromScreen.Tag != null)
            {
                var sampler = btnFromScreen.Tag as ScreenColorSampler;
                if (sampler.IsWorking)
                {
                    sampler.Stop();
                }
            }
        }

        void Picker_ColorPicked(int value)
        {
            this.Dispatcher.Invoke(() =>
            {
                ShowColor(value.ToColor());
            });
        }

        private void ShowColor(System.Windows.Media.Color color)
        {
            labelCurrentColor.Text = color.ToHexString();
            panelCurrentColor.Background = color.ToBrush();
        }

        private bool _isPaletteShowing = false;
        void btnPalette_Click(object sender, RoutedEventArgs e)
        {
            _isPaletteShowing = true;
            var dialog = new ColorDialog();
            dialog.FullOpen = true;
            dialog.Deactivated += (s, evt) => { this.Close(); };
            dialog.Color = System.Drawing.Color.FromArgb(Int32.Parse(labelCurrentColor.Text, System.Globalization.NumberStyles.HexNumber));
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Console.Write(dialog.Color.ToColor().ToHexString());
                this.Close();
            }
            _isPaletteShowing = false;
        }

        void btnFromScreen_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            ScreenColorSampler sampler;

            if (element.Tag == null)
            {
                sampler = new ScreenColorSampler();
                sampler.ColorPicked += Picker_ColorPicked;
                element.Tag = sampler;
            }
            else
            {
                sampler = element.Tag as ScreenColorSampler;
            }

            if (sampler.IsWorking)
            {
                sampler.Stop();
            }
            else
            {
                sampler.Start();
            }
        }
    }
}
