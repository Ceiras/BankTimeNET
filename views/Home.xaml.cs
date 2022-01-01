using BankTimeNET.Data;
using BankTimeNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankTimeNET.Views
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            InitializeUserData();
            InitializeActionButtons();
            InitializeAceptedServicesTable();
            InitializeRequestedServicesTable();
            InitializeBankServicesTable();
        }

        private void InitializeUserData()
        {
            this.nameLabel.Content = AppStore.currentUser.Name;
            this.dniLabel.Content = AppStore.currentUser.Dni;
            this.amountLabel.Content = AppStore.currentUser.Amount + " h";
            this.bankLabel.Content = AppStore.currentUser.Bank != null ? AppStore.currentUser.Bank.Place : '-';
        }

        private void InitializeActionButtons()
        {
            if (AppStore.currentUser.Bank == null)
            {
                this.associateBankButton.Content = "Associate Bank";
                this.requestServiceButton.IsEnabled = false;
            }
            else
            {
                this.associateBankButton.Content = "Change Bank";
                this.requestServiceButton.IsEnabled = true;
            }
        }

        private void InitializeRequestedServicesTable()
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    List<Service> resServices = db.Services
                        .Include((Service service) => service.RequestUser)
                        .Include((Service service) => service.DoneUser)
                        .Include((Service service) => service.Bank)
                        .Where((Service service) => service.Bank.Equals(AppStore.currentUser.Bank))
                        .Where((Service service) => service.RequestUser.Equals(AppStore.currentUser))
                        .ToList();
                    this.requestedServicesListView.ItemsSource = resServices;
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException);
                }
            }
        }

        private void InitializeAceptedServicesTable()
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    List<Service> resServices = db.Services
                        .Include((Service service) => service.RequestUser)
                        .Include((Service service) => service.DoneUser)
                        .Include((Service service) => service.Bank)
                        .Where((Service service) => service.Bank.Equals(AppStore.currentUser.Bank))
                        .Where((Service service) => service.DoneUser.Equals(AppStore.currentUser))
                        .ToList();
                    this.acceptedServicesListView.ItemsSource = resServices;
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException);
                }
            }
        }

        private void InitializeBankServicesTable()
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    List<Service> resServices = db.Services
                        .Include((Service service) => service.RequestUser)
                        .Include((Service service) => service.DoneUser)
                        .Include((Service service) => service.Bank)
                        .Where((Service service) => service.Bank.Equals(AppStore.currentUser.Bank))
                        .ToList();
                    this.bankServicesListView.ItemsSource = resServices;
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException);
                }
            }
        }

        private void associateBankButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            homeFrame.Navigate(new ChooseBank());
        }

        private void requestServiceButton_Click(object sender, RoutedEventArgs e)
        {
            homeFrame.Navigate(new RequestService());
        }

        private void unregisterButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    User? resUser = db.Users.Where((User user) => user.Id.Equals(AppStore.currentUser.Id)).FirstOrDefault();
                    if (resUser != null)
                    {
                        resUser.Active = false;
                        int res = db.SaveChanges();

                        if (res == 1)
                        {
                            removeUserXml(resUser);
                            AppStore.currentUser = null;
                            homeFrame.Navigate(new Login());
                            MessageBox.Show("Removed successfully");
                        }
                        else
                        {
                            MessageBox.Show("Error creating the new bank");
                        }
                    }
                    else
                    {
                        MessageBox.Show("The user selected does not exists");
                    }
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException);
                }
            }
        }

        private void removeUserXml(User user)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                DataRow userRow = null;
                foreach (DataRow row in dataset.Tables["Users"].Rows)
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
                    userRow["active"] = user.Active;
                    userRow.EndEdit();

                    DataXml.writeDataXml(dataset);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: {0}", e.ToString());
            }
        }
    }
}
