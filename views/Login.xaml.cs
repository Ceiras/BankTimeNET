using BankTimeNET.Data;
using BankTimeNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankTimeNET.Views
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
            String dni = this.dniInput.Text;
            String password = this.passwordInput.Password;

            using (var db = new DatabaseContext())
            {
                User? res = db.Users.Include((User user) => user.Bank).Where((User user) => user.Dni.Equals(dni) && user.Password.Equals(password) && user.Active.Equals(true)).FirstOrDefault();
                if (res != null)
                {
                    AppStore.currentUser = res;
                    loginFrame.Navigate(new Home());
                }
                else
                {
                    MessageBox.Show("The login is wrong");
                }
            }
        }

        private void singupButton_Click(object sender, RoutedEventArgs e)
        {
            loginFrame.Navigate(new SingUp());
        }
    }
}
