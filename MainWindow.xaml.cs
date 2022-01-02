using BankTimeNET.DAO;
using BankTimeNET.Data;
using BankTimeNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

            UserDAO userDAO = new UserDAO();
            BankDAO bankDAO = new BankDAO();
            ServiceDAO serviceDAO = new ServiceDAO();

            User newUser = new User("12345678P", "Alberto Álvarez", "Password", 0, true, null);
            userDAO.newUser(newUser);
            userDAO.addUserXml(newUser);

            AppStore.currentUser = newUser;

            Bank newBank = new Bank("A Place");
            bankDAO.addBank(newBank);
            bankDAO.addBankXml(newBank);

            bankDAO.associateBank(newBank.Place);
            bankDAO.associateBankXml();

            AppStore.currentUser.Bank = newBank;

            DateTime dateService = new(2022, 1, 5, 10, 0, 0);
            Service newService = new(dateService, "A Task", 1, 0, ServiceState.Pending, AppStore.currentUser, null, AppStore.currentUser.Bank);
            serviceDAO.newService(newService);
            serviceDAO.addServiceXml(newService);

            serviceDAO.removeService(newService);
            serviceDAO.removeServiceXml(newService);

            User otherUser = new User("12111678P", "Other User", "Password", 0, true, null);
            userDAO.newUser(otherUser);
            userDAO.addUserXml(otherUser);

            AppStore.currentUser = otherUser;
            AppStore.currentUser.Bank = newBank;

            DateTime otherDateService = new(2022, 1, 5, 10, 0, 0);
            Service otherService = new(otherDateService, "Other Task", 1, 0, ServiceState.Pending, AppStore.currentUser, null, AppStore.currentUser.Bank);
            serviceDAO.newService(otherService);
            serviceDAO.addServiceXml(otherService);

            AppStore.currentUser = newUser;
            AppStore.currentUser.Bank = newBank;

            serviceDAO.acceptService(otherService);
            serviceDAO.acceptServiceXml(otherService);

            serviceDAO.confirmService(otherService, 1);
            serviceDAO.confirmServiceXml(otherService, 1);
        }
    }
}
