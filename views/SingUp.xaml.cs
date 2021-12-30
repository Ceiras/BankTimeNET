using BankTimeNET.db;
using System;
using System.Data.SqlClient;
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
            String username = this.usernameInput.Text;
            String password = this.passwordInput.Password;

            SqlConnection sqlConnection = Db.connect();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "INSERT INTO [Users] (dni, name, username, password, amount, active) VALUES (@dni, @name, @username, @password, 0, 1)";
            cmd.Parameters.AddWithValue("@dni", dni);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Connection = sqlConnection;

            try
            {
                int res = cmd.ExecuteNonQuery();
                if (res == 1)
                {
                    MessageBox.Show("Inserted successfully");
                    singupFrame.Navigate(new Login());
                }
                else
                {
                    MessageBox.Show("Error creating the new user");
                }
            } catch (SqlException sqlException)
            {
                MessageBox.Show("ERROR: " + sqlException.Message);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            singupFrame.Navigate(new Login());
        }
    }
}
