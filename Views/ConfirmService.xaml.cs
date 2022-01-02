using BankTimeNET.DAO;
using System;
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
                ServiceDAO serviceDAO = new ServiceDAO();
                if (serviceDAO.confirmService(this.service, timeSpentSelected) > 0)
                {
                    serviceDAO.confirmServiceXml(this.service, timeSpentSelected);
                    MessageBox.Show("Service confirmed", "Confirm Service", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.confirmServiceFrame.Navigate(new Home());
                }
                else
                {
                    MessageBox.Show("It had been impossible confirm the service because fails the connection to database", "ERROR: Confirm Service", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void backButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.confirmServiceFrame.Navigate(new Home());
        }
    }
}
