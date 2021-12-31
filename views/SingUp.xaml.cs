using BankTimeNET.Data;
using BankTimeNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
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

            using (var db = new DatabaseContext())
            {
                try
                {
                    User newUser = new(dni, name, password, 0, true, null);
                    db.Users.Add(newUser);
                    int res = db.SaveChanges();
                    if (res == 1)
                    {
                        addUserXml(newUser);
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

        private void addUserXml(User user)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                DataRow newUser = dataset.Tables["Users"].NewRow();
                newUser["dni"] = user.Dni;
                newUser["name"] = user.Name;
                newUser["password"] = user.Password;
                newUser["amount"] = user.Amount;
                newUser["active"] = user.Active;
                newUser["bankId"] = user.Bank != null ? user.Bank.Id : "";
                dataset.Tables["Users"].Rows.Add(newUser);

                DataXml.writeDataXml(dataset);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: {0}", e.ToString());
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            singupFrame.Navigate(new Login());
        }
    }
}
