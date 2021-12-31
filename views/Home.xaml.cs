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
            this.userLabel.Content = Store.currentUser;
        }
    }
}
