using System;
using System.Windows;

namespace Whatsapp_Sifreli
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var login = new LoginWindow();
            var ok = login.ShowDialog() == true;

            if (!ok)
            {
                Shutdown();
                return;
            }

            // LoginWindow kapanınca artık MainWindow'u aç
            var main = new MainWindow();
            MainWindow = main;
            main.Show();

            // Artık uygulamayı kapatmak için ana pencere yeterli
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
    }
}
