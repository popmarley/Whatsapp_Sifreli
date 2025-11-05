using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Web.WebView2.Core;

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
    }
}
