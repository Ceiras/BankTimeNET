using BankTimeNET.db;
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
            this.nameLabel.Content = Store.currentUser.Name;
            this.dniLabel.Content = Store.currentUser.Dni;
            this.amountLabel.Content = Store.currentUser.Amount;
        }

        private void homeFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
