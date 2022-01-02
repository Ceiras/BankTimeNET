using BankTimeNET.Data;
using BankTimeNET.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankTimeNET.Views
{
    /// <summary>
    /// Lógica de interacción para ConfirmService.xaml
    /// </summary>
    public partial class ConfirmService : Page
    {
        private Service service;

        public ConfirmService(Service service)
        {
            InitializeComponent();
            this.service = service;
            InitializeServiceData();
        }

        private void InitializeServiceData()
        {
            this.dateLabel.Content = this.service.Date.ToString("dd/MM/yyyy HH:mm");
            this.descriptionLabel.Content = this.service.Description.ToString();
            this.requestTimeLabel.Content = this.service.RequestTime.ToString() + " h";
            this.requestUserLabel.Content = this.service.RequestUser.Name.ToString();
            this.bankLabel.Content = this.service.Bank.Place.ToString();
            this.doneTimeInput.SelectedValue = this.service.RequestTime.ToString();

            for (int i = 1; i < 25; i++)
            {
                this.doneTimeInput.Items.Add(i.ToString());
            }
        }

        private void confirmServiceInput_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int timeSpentSelected = Int32.Parse(this.doneTimeInput.Text.ToString());

            if (timeSpentSelected != null)
            {
                using (var db = new DatabaseContext())
                {
                    Service? resService = db.Services.Where((Service service) => service.Id.Equals(this.service.Id)).FirstOrDefault();
                    User? resRequestUser = db.Users.Where((User user) => user.Id.Equals(this.service.RequestUser.Id)).FirstOrDefault();
                    User? resDoneUser = db.Users.Where((User user) => user.Id.Equals(this.service.DoneUser.Id)).FirstOrDefault();
                    if (resService != null && resRequestUser != null && resDoneUser != null)
                    {
                        resService.DoneTime = timeSpentSelected;
                        resService.State = ServiceState.Done;
                        resRequestUser.Amount -= timeSpentSelected;
                        resDoneUser.Amount += timeSpentSelected;
                        int res = db.SaveChanges();
                        if (res > 0)
                        {
                            confirmServiceXml(resService, timeSpentSelected, resRequestUser, resDoneUser);
                            MessageBox.Show("Service confirmed", "Confirm Service", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.confirmServiceFrame.Navigate(new Home());
                        }
                        else
                        {
                            MessageBox.Show("It had been impossible confirm the service because fails the connection to database", "ERROR: Confirm Service", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void confirmServiceXml(Service service, int timeSpent, User requestUser, User doneUser)
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

                DataRow requestUserRow = null;
                foreach (DataRow row in dataset.Tables["Users"].Rows)
                {
                    if (row.ItemArray[0].Equals(requestUser.Dni.ToString()))
                    {
                        requestUserRow = row;
                        break;
                    }
                }

                DataRow doneUserRow = null;
                foreach (DataRow row in dataset.Tables["Users"].Rows)
                {
                    if (row.ItemArray[0].Equals(doneUser.Dni.ToString()))
                    {
                        doneUserRow = row;
                        break;
                    }
                }

                if (serviceRow != null && requestUserRow != null && doneUserRow != null)
                {
                    serviceRow.BeginEdit();
                    serviceRow["doneTime"] = timeSpent;
                    serviceRow["state"] = ServiceState.Done;
                    serviceRow.EndEdit();

                    requestUserRow.BeginEdit();
                    requestUserRow["amount"] = Int32.Parse(requestUserRow["amount"].ToString()) - timeSpent;
                    requestUserRow.EndEdit();

                    doneUserRow.BeginEdit();
                    doneUserRow["amount"] = Int32.Parse(doneUserRow["amount"].ToString()) + timeSpent;
                    doneUserRow.EndEdit();

                    DataXml.writeDataXml(dataset);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Accept Service", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.confirmServiceFrame.Navigate(new Home());
        }
    }
}
