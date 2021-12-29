using System;
using System.Windows;
using System.Windows.Controls;

namespace BankTimeNET.views
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            String username = this.usernameInput.Text;
            String password = this.passwordInput.Password;

            MessageBox.Show($"Username: {username} - {password}");
        }

        private void singupButton_Click(object sender, RoutedEventArgs e)
        {
            loginFrame.Navigate(new SingUp());
        }
    }
}
