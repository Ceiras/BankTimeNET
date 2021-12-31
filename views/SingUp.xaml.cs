using BankTimeNET.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BankTimeNET.views
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

            using (var db = new DatabaseContext())
            {
                try
                {
                    User newUser = new User(dni, name, password, 0, true);
                    db.Users.Add(newUser);
                    int res = db.SaveChanges();
                    if (res == 1)
                    {
                        MessageBox.Show("Inserted successfully");
                        singupFrame.Navigate(new Login());
                    }
                    else
                    {
                        MessageBox.Show("Error creating the new user");
                    }
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException);
                }
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            singupFrame.Navigate(new Login());
        }
    }
}
