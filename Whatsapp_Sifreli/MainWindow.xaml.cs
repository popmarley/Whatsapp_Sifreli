using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Whatsapp_Sifreli
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            Loaded += MainWindow_Loaded;
            MouseLeftButtonDown += (_, __) => DragMove();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await InitWebViewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ WebView başlatılamadı:\n" + ex.ToString());
            }
        }

        private async Task InitWebViewAsync()
        {
            try
            {
                string userDataFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Whatsapp_Sifreli", "WebView2Profile");

                var edgePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
    "Microsoft", "Edge", "Application");

                if (!Directory.Exists(edgePath))
                {
                    edgePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                        "Microsoft", "Edge", "Application");
                }

                var env = await CoreWebView2Environment.CreateAsync(
    browserExecutableFolder: null,
    userDataFolder: userDataFolder);


                await webView.EnsureCoreWebView2Async(env);


                webView.Source = new Uri("https://web.whatsapp.com/");
            }
            catch (Exception ex)
            {
                MessageBox.Show("WebView2 başlatılamadı: " + ex.Message,
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Close_Click(object sender, RoutedEventArgs e) => Close();

        // Bu alanları sınıf seviyesine ekle
        private double _restoreTop, _restoreLeft, _restoreWidth, _restoreHeight;
        private bool _isMaximized = false;

        private void MaxRestore_Click(object sender, RoutedEventArgs e)
        {
            if (!_isMaximized)
            {
                // Mevcut boyut ve konumu kaydet
                _restoreTop = Top;
                _restoreLeft = Left;
                _restoreWidth = Width;
                _restoreHeight = Height;

                // Görev çubuğunu kaplamadan büyüt
                var workArea = SystemParameters.WorkArea;
                Left = workArea.Left;
                Top = workArea.Top;
                Width = workArea.Width;
                Height = workArea.Height;

                _isMaximized = true;
                MaxRestoreButton.Content = "🗗"; // restore ikonu
            }
            else
            {
                // Önceki boyut ve konuma dön
                Left = _restoreLeft;
                Top = _restoreTop;
                Width = _restoreWidth;
                Height = _restoreHeight;

                _isMaximized = false;
                MaxRestoreButton.Content = "🗖"; // maximize ikonu
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle)?.AddHook(WindowProc);
        }

        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTCLIENT = 1;

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_NCHITTEST = 0x0084;
            const int RESIZE_HANDLE_SIZE = 8; // piksel hassasiyeti

            if (msg == WM_NCHITTEST)
            {
                var mousePos = PointFromScreen(new Point(
                    (short)((uint)lParam & 0xFFFF),
                    (short)(((uint)lParam >> 16) & 0xFFFF)));

                double width = ActualWidth;
                double height = ActualHeight;

                handled = true;

                if (mousePos.Y <= RESIZE_HANDLE_SIZE)
                {
                    if (mousePos.X <= RESIZE_HANDLE_SIZE) return (IntPtr)HTTOPLEFT;
                    if (mousePos.X >= width - RESIZE_HANDLE_SIZE) return (IntPtr)HTTOPRIGHT;
                    return (IntPtr)HTTOP;
                }
                else if (mousePos.Y >= height - RESIZE_HANDLE_SIZE)
                {
                    if (mousePos.X <= RESIZE_HANDLE_SIZE) return (IntPtr)HTBOTTOMLEFT;
                    if (mousePos.X >= width - RESIZE_HANDLE_SIZE) return (IntPtr)HTBOTTOMRIGHT;
                    return (IntPtr)HTBOTTOM;
                }
                else if (mousePos.X <= RESIZE_HANDLE_SIZE)
                {
                    return (IntPtr)HTLEFT;
                }
                else if (mousePos.X >= width - RESIZE_HANDLE_SIZE)
                {
                    return (IntPtr)HTRIGHT;
                }

                handled = false;
            }

            return IntPtr.Zero;
        }

    }
}
