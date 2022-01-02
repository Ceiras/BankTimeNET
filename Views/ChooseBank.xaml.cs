using BankTimeNET.DAO;
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
                    foreach (Bank bank in db.Banks)
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
                Bank newBank = new Bank(bankPlace);
                BankDAO bankDAO = new BankDAO();
                if (bankDAO.addBank(newBank) > 0)
                {
                    bankDAO.addBankXml(newBank);
                    this.newBankInput.Text = "";
                    this.populateBankListView();
                    MessageBox.Show("Created successfully", "New Bank", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error creating the new bank", "ERROR: New Bank", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void associateBankButton_Click(object sender, RoutedEventArgs e)
        {
            String selectedItem = this.bankListBox.Items[this.bankListBox.SelectedIndex].ToString();

            BankDAO bankDAO = new BankDAO();
            if (bankDAO.associateBank(selectedItem) > 0)
            {
                bankDAO.updateBankToUserXml();
                MessageBox.Show("Bank associated successfully", "Choose Bank", MessageBoxButton.OK, MessageBoxImage.Information);
                this.chooseBankFrame.Navigate(new Home());
            }
            else
            {
                MessageBox.Show("It had been impossible associate the bank to the user because fails the connection to database", "ERROR: Choose Bank", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            chooseBankFrame.Navigate(new Home());
        }
    }
}
