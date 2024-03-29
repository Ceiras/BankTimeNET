﻿using BankTimeNET.Data;
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
            InitializeTables();
        }

        private void InitializeTables()
        {
            InitializeAceptedServicesTable();
            InitializeRequestedServicesTable();
            InitializeBankServicesTable();
        }

        private void InitializeUserData()
        {
            using (var db = new DatabaseContext())
            {
                User? res = db.Users.Include((User user) => user.Bank).Where((User user) => user.Dni.Equals(AppStore.currentUser.Dni)).FirstOrDefault();
                if (res != null)
                {
                    AppStore.currentUser = res;
                }
            }

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

        private void InitializeAceptedServicesTable()
        {
            this.acceptedServicesListView.ItemsSource = new List<Service>();

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
                        .Where((Service service) => !service.State.Equals(ServiceState.Done))
                        .ToList();
                    this.acceptedServicesListView.ItemsSource = resServices;
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: Initialize Accepted Services Table", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void InitializeRequestedServicesTable()
        {
            this.requestedServicesListView.ItemsSource = new List<Service>();

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
                        .Where((Service service) => service.State.Equals(ServiceState.Pending))
                        .ToList();
                    this.requestedServicesListView.ItemsSource = resServices;
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: Initialize Request Services Table", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void InitializeBankServicesTable()
        {
            this.bankServicesListView.ItemsSource = new List<Service>();

            using (var db = new DatabaseContext())
            {
                try
                {
                    List<Service> resServices = db.Services
                        .Include((Service service) => service.RequestUser)
                        .Include((Service service) => service.DoneUser)
                        .Include((Service service) => service.Bank)
                        .Where((Service service) => service.Bank.Equals(AppStore.currentUser.Bank))
                        .Where((Service service) => !service.RequestUser.Equals(AppStore.currentUser))
                        .Where((Service service) => service.State.Equals(ServiceState.Pending))
                        .ToList();
                    this.bankServicesListView.ItemsSource = resServices;
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: Initialize Bank Services Table", MessageBoxButton.OK, MessageBoxImage.Error);
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

                        if (res > 0)
                        {
                            removeUserXml(resUser);
                            AppStore.currentUser = null;
                            homeFrame.Navigate(new Login());
                            MessageBox.Show("Removed user successfully", "Remove User", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Error removing user", "EROR: Remove User", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The user selected does not exists", "ERROR: Remove User", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: Remove User", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Remove XML User", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void revokeService_Click(object sender, RoutedEventArgs e)
        {
            Service? selectedItem = this.requestedServicesListView.SelectedIndex > -1 ? (Service)this.requestedServicesListView.Items[this.requestedServicesListView.SelectedIndex] : null;

            if (selectedItem != null)
            {
                using (var db = new DatabaseContext())
                {
                    db.Services.Remove(selectedItem);
                    int res = db.SaveChanges();
                    if (res > 0)
                    {
                        removeServiceXml(selectedItem);
                        this.InitializeTables();
                        MessageBox.Show("Service removed successfully", "Remove Service", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("It had been impossible remove the service because fails the connection to database", "ERROR: Remove Service", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void removeServiceXml(Service service)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                for (int i = 0; i < dataset.Tables["Services"].Rows.Count; i++)
                {
                    if (dataset.Tables["Services"].Rows[i].ItemArray[0].Equals(service.Id.ToString()))
                    {
                        dataset.Tables["Services"].Rows[i].Delete();

                        DataXml.writeDataXml(dataset);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Remove Service", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void acceptService_Click(object sender, RoutedEventArgs e)
        {
            Service? selectedItem = this.bankServicesListView.SelectedIndex > -1 ? (Service)this.bankServicesListView.Items[this.bankServicesListView.SelectedIndex] : null;

            if (selectedItem != null)
            {
                using (var db = new DatabaseContext())
                {
                    Service? resService = db.Services.Where((Service service) => service.Id.Equals(selectedItem.Id)).FirstOrDefault();
                    if (resService != null)
                    {
                        resService.DoneUser = AppStore.currentUser;
                        resService.State = ServiceState.Accepted;
                        int res = db.SaveChanges();
                        if (res > 0)
                        {
                            acceptServiceXml(resService);
                            this.InitializeTables();
                            MessageBox.Show("Service accepted", "Accept Service", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("It had been impossible accept the service because fails the connection to database", "ERROR: Accept Service", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void acceptServiceXml(Service service)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                DataRow serviceRow = null;
                foreach (DataRow row in dataset.Tables["Services"].Rows)
                {
                    if (row.ItemArray[0].Equals(service.Id.ToString()))
                    {
                        serviceRow = row;
                        break;
                    }
                }

                if (serviceRow != null)
                {
                    serviceRow.BeginEdit();
                    serviceRow["doneUserId"] = AppStore.currentUser.Id;
                    serviceRow["state"] = ServiceState.Accepted;
                    serviceRow.EndEdit();

                    DataXml.writeDataXml(dataset);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Accept Service", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void confirmService_Click(object sender, RoutedEventArgs e)
        {
            Service? selectedItem = this.acceptedServicesListView.SelectedIndex > -1 ? (Service)this.acceptedServicesListView.Items[this.acceptedServicesListView.SelectedIndex] : null;

            if (selectedItem != null)
            {
                homeFrame.Navigate(new ConfirmService(selectedItem));
            }
        }
    }
}
