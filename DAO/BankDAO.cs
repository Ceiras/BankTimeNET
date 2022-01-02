using BankTimeNET.Data;
using BankTimeNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Data;
using System.Windows;

namespace BankTimeNET.DAO
{
    public class BankDAO
    {

        public int addBank(Bank bank)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    db.Banks.Add(bank);
                    int res = db.SaveChanges();

                    return res;
                }
                catch (DbUpdateException sqlException)
                {
                    return -1;
                }
            }
        }

        public void addBankXml(Bank bank)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                DataRow newBank = dataset.Tables["Banks"].NewRow();
                newBank["place"] = bank.Place;
                dataset.Tables["Banks"].Rows.Add(bank.Place);

                DataXml.writeDataXml(dataset);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Add Bank", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int associateBank(String item)
        {
            using (var db = new DatabaseContext())
            {
                Bank? resBank = db.Banks.Where((Bank bank) => bank.Place.Equals(item)).FirstOrDefault();
                User? resUser = db.Users.Where((User user) => user.Dni.Equals(AppStore.currentUser.Dni)).FirstOrDefault();
                if (resUser != null && resBank != null)
                {
                    resUser.Bank = resBank;
                    int res = db.SaveChanges();
                    if (res > 0)
                    {
                        AppStore.currentUser = resUser;
                    }
                    else
                    {
                        MessageBox.Show("It had been impossible associate the bank to the user because fails the connection to database", "ERROR: Choose Bank", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    return res;
                }
                else
                {
                    return -1;
                }
            }
        }

        public void associateBankXml()
        {
            User currentUser = AppStore.currentUser;

            try
            {
                DataSet dataset = DataXml.readDataXml();

                DataRow userRow = null;
                foreach (DataRow row in dataset.Tables["Users"].Rows)
                {
                    if (row.ItemArray[0].Equals(currentUser.Dni))
                    {
                        userRow = row;
                        break;
                    }
                }

                if (userRow != null)
                {
                    userRow.BeginEdit();
                    userRow["bankId"] = currentUser.Bank.Id;
                    userRow.EndEdit();

                    DataXml.writeDataXml(dataset);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Choose Bank", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
