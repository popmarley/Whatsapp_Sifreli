using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Whatsapp_Sifreli
{
    public partial class LoginWindow : Window
    {
        private const string DemoPassword = "123"; // burayı kendin değiştirebilirsin

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Pwd.Password == "123")
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Parola yanlış!", "Hata",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                Pwd.Clear();
                Pwd.Focus();
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        

    }
}
