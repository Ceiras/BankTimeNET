using BankTimeNET.Data;
using BankTimeNET.Views;
using System.Windows;
using System.Windows.Controls;

namespace BankTimeNET.views
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            this.nameLabel.Content = AppStore.currentUser.Name;
            this.dniLabel.Content = AppStore.currentUser.Dni;
            this.amountLabel.Content = AppStore.currentUser.Amount + " h";
            this.bankLabel.Content = AppStore.currentUser.Bank != null ? AppStore.currentUser.Bank.Place : '-';

            if (AppStore.currentUser.Bank == null)
            {
                this.associateBankButton.Visibility = Visibility.Visible;
            } else
            {
                this.associateBankButton.Visibility = Visibility.Hidden;
            }
        }

        private void associateBankButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            homeFrame.Navigate(new ChooseBank());
        }
    }
}
