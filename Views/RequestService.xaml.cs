﻿using BankTimeNET.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace BankTimeNET.Views
{
    /// <summary>
    /// Lógica de interacción para RequestService.xaml
    /// </summary>
    public partial class RequestService : Page
    {
        public RequestService()
        {
            InitializeComponent();

            for(int i = 0; i < 24; i++)
            {
                this.hourInput.Items.Add(i < 10 ? "0" + i : i.ToString());
            }

            for (int i = 0; i < 60; i++)
            {
                this.minutesInput.Items.Add(i < 10 ? "0" + i : i.ToString());
            }

            for (int i = 1; i < 25; i++)
            {
                this.requestTimeInput.Items.Add(i.ToString());
            }
        }

        private void requestInput_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = (DateTime) this.datePickerInput.SelectedDate;
            String timeHours = this.hourInput.Text;
            String timeMinutes = this.minutesInput.Text;
            String description = this.descriptionInput.Text;
            String timeRequest = this.requestTimeInput.Text;

            using (var db = new DatabaseContext())
            {
                try
                {
                    DateTime dateService = new(date.Year, date.Month, date.Day, Int32.Parse(timeHours), Int32.Parse(timeMinutes), 0);
                    Service newService = new(dateService, description, Int32.Parse(timeRequest), 0, ServiceState.Pending, AppStore.currentUser, null, AppStore.currentUser.Bank);
                    db.Entry(newService.RequestUser).State = EntityState.Unchanged;
                    db.Entry(newService.Bank).State = EntityState.Unchanged;
                    db.Services.Add(newService);
                    int res = db.SaveChanges();
                    if (res > 0)
                    {
                        addServiceXml(newService);
                        MessageBox.Show("Services created", "New Service", MessageBoxButton.OK, MessageBoxImage.Information);
                        requestServiceFrame.Navigate(new Home());
                    }
                    else
                    {
                        MessageBox.Show("Error creating the service", "ERROR: New Service", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: New Service", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void addServiceXml(Service service)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                DataRow newService = dataset.Tables["Services"].NewRow();
                newService["id"] = service.Id;
                newService["date"] = service.Date;
                newService["description"] = service.Description;
                newService["requestTime"] = service.RequestTime;
                newService["doneTime"] = service.DoneTime;
                newService["state"] = service.State;
                newService["requestUserId"] = service.RequestUser != null ? service.RequestUser.Id : "";
                newService["doneUserId"] = service.DoneUser != null ? service.DoneUser.Id : "";
                newService["bankId"] = service.Bank != null ? service.Bank.Id : "";
                dataset.Tables["Services"].Rows.Add(newService);

                DataXml.writeDataXml(dataset);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Add XML Service", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.requestServiceFrame.Navigate(new Home());
        }
    }
}
