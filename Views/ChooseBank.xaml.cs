using BankTimeNET.Data;
using BankTimeNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankTimeNET.Views
{
    /// <summary>
    /// Lógica de interacción para ChooseBank.xaml
    /// </summary>
    public partial class ChooseBank : Page
    {
        public ChooseBank()
        {
            InitializeComponent();
            populateBankListView();
        }

        private void populateBankListView()
        {
            using (var db = new DatabaseContext())
            {
                this.bankListBox.Items.Clear();
                try
                {
                    ListView bankListView = new ListView();
                    foreach(Bank bank in db.Banks)
                    {
                        this.bankListBox.Items.Add(bank.Place);
                    }
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: Choose Bank", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void newBankButon_Click(object sender, RoutedEventArgs e)
        {
            String bankPlace = this.newBankInput.Text;

            if (bankPlace.Length > 0)
            {
                using (var db = new DatabaseContext())
                {
                    try
                    {
                        Bank newBank = new Bank(bankPlace);
                        db.Banks.Add(newBank);
                        int res = db.SaveChanges();

                        if (res > 0)
                        {
                            addBankXml(newBank);
                            this.newBankInput.Text = "";
                            this.populateBankListView();
                            MessageBox.Show("Created successfully", "New Bank", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Error creating the new bank", "ERROR: New Bank", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (DbUpdateException sqlException)
                    {
                        MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: New Bank", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void addBankXml(Bank bank)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                DataRow newBank = dataset.Tables["Banks"].NewRow();
                newBank["place"] = bank.Place;
                dataset.Tables["Banks"].Rows.Add(bank.Place);

                DataXml.writeDataXml(dataset);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Add Bank", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void associateBankButton_Click(object sender, RoutedEventArgs e)
        {
            String selectedItem = this.bankListBox.Items[this.bankListBox.SelectedIndex].ToString();

            using (var db = new DatabaseContext())
            {
                Bank? resBank = db.Banks.Where((Bank bank) => bank.Place.Equals(selectedItem)).FirstOrDefault();
                if (resBank != null)
                {
                    User? resUser = db.Users.Where((User user) => user.Dni.Equals(AppStore.currentUser.Dni)).FirstOrDefault();
                    if (resUser != null)
                    {
                        resUser.Bank = resBank;
                        int res = db.SaveChanges();
                        if (res > 0)
                        {
                            updateBankToUserXml(resUser, resBank);
                            AppStore.currentUser = resUser;
                            MessageBox.Show("Bank associated successfully", "Choose Bank", MessageBoxButton.OK, MessageBoxImage.Information);
                            chooseBankFrame.Navigate(new Home());
                        }
                        else
                        {
                            MessageBox.Show("It had been impossible associate the bank to the user because fails the connection to database", "ERROR: Choose Bank", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("It had been impossible associate the bank to the user", "ERROR: Choose Bank", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void updateBankToUserXml(User user, Bank bank)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                DataRow userRow = null;
                foreach(DataRow row in dataset.Tables["Users"].Rows)
                {
                    if (row.ItemArray[0].Equals(user.Dni))
                    {
                        userRow = row;
                        break;
                    }
                }

                if (userRow != null)
                {
                    userRow.BeginEdit();
                    userRow["bankId"] = bank.Id;
                    userRow.EndEdit();

                    DataXml.writeDataXml(dataset);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Choose Bank", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            chooseBankFrame.Navigate(new Home());
        }
    }
}
