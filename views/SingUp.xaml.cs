using BankTimeNET.DAO;
using BankTimeNET.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BankTimeNET.Views
{
    /// <summary>
    /// Lógica de interacción para SingUp.xaml
    /// </summary>
    public partial class SingUp : Page
    {
        public SingUp()
        {
            InitializeComponent();
        }

        private void singUpButton_Click(object sender, RoutedEventArgs e)
        {
            String dni = this.dniInput.Text;
            String name = this.nameInput.Text;
            String password = this.passwordInput.Password;

            User newUser = new(dni, name, password, 0, true, null);
            UserDAO userDAO = new UserDAO();
            if (userDAO.newUser(newUser) > 0)
            {
                userDAO.addUserXml(newUser);
                MessageBox.Show("Inserted successfully", "New User", MessageBoxButton.OK, MessageBoxImage.Information);
                singupFrame.Navigate(new Login());
            }
            else
            {
                MessageBox.Show("Error creating the user", "ERROR: New User", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            singupFrame.Navigate(new Login());
        }
    }
}
