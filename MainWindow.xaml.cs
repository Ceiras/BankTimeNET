using BankTimeNET.Views;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace BankTimeNET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (var context = new DatabaseContext())
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }

            mainFrame.Navigate(new Login());
        }
    }
}
