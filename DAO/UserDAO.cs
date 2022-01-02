using BankTimeNET.Data;
using BankTimeNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Windows;

namespace BankTimeNET.DAO
{
    public class UserDAO
    {
        public int newUser(User user)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    db.Users.Add(user);
                    int res = db.SaveChanges();

                    return res;
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: New User", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
        }

        public void addUserXml(User user)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                DataRow newUser = dataset.Tables["Users"].NewRow();
                newUser["dni"] = user.Dni;
                newUser["name"] = user.Name;
                newUser["password"] = user.Password;
                newUser["amount"] = user.Amount;
                newUser["active"] = user.Active;
                newUser["bankId"] = user.Bank != null ? user.Bank.Id : "";
                dataset.Tables["Users"].Rows.Add(newUser);

                DataXml.writeDataXml(dataset);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Add XML User", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int removeUser()
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
                            AppStore.currentUser = null;
                        }

                        return res;
                    }

                    return -1;
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: Remove User", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
        }

        public void removeUserXml()
        {
            User user = AppStore.currentUser;
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
    }
}
