using BankTimeNET.db;
using BankTimeNET.models;
using System;
using System.Data.SqlClient;
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

            SqlConnection sqlConnection = Db.connect();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT TOP(1) * FROM [Users] WHERE username=@username AND password=@password";
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Connection = sqlConnection;

            SqlDataReader sqlReader = cmd.ExecuteReader();

            if (sqlReader.HasRows)
            {
                while (sqlReader.Read())
                {
                    Store.currentUser = new User();
                    Store.currentUser.Dni = sqlReader["dni"].ToString();
                    Store.currentUser.Name = sqlReader["name"].ToString();
                    Store.currentUser.Username = sqlReader["username"].ToString();
                    Store.currentUser.Amount = Int32.Parse(sqlReader["amount"].ToString());
                    Store.currentUser.Active = bool.Parse(sqlReader["active"].ToString());
                }

                loginFrame.Navigate(new Home());
            }
            else
            {
                MessageBox.Show("Login error");
            }

        }

        private void singupButton_Click(object sender, RoutedEventArgs e)
        {
            loginFrame.Navigate(new SingUp());
        }
    }
}
