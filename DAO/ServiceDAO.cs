using BankTimeNET.Data;
using BankTimeNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Windows;

namespace BankTimeNET.DAO
{
    public class ServiceDAO
    {
        public int newService(Service service)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    db.Entry(service.RequestUser).State = EntityState.Unchanged;
                    db.Entry(service.Bank).State = EntityState.Unchanged;
                    db.Services.Add(service);
                    int res = db.SaveChanges();

                    return res;
                }
                catch (DbUpdateException sqlException)
                {
                    MessageBox.Show("ERROR: " + sqlException.InnerException, "ERROR: New Service", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
        }

        public void addServiceXml(Service service)
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

        public int confirmService(Service service, int timeSpent)
        {
            using (var db = new DatabaseContext())
            {
                Service? resService = db.Services.Where((Service service) => service.Id.Equals(service.Id)).FirstOrDefault();
                User? resRequestUser = db.Users.Where((User user) => user.Id.Equals(service.RequestUser.Id)).FirstOrDefault();
                User? resDoneUser = db.Users.Where((User user) => user.Id.Equals(service.DoneUser.Id)).FirstOrDefault();
                if (resService != null && resRequestUser != null && resDoneUser != null)
                {
                    resService.DoneTime = timeSpent;
                    resService.State = ServiceState.Done;
                    resRequestUser.Amount -= timeSpent;
                    resDoneUser.Amount += timeSpent;
                    int res = db.SaveChanges();

                    return res;
                }
                else
                {
                    return -1;
                }
            }
        }

        public void confirmServiceXml(Service service, int timeSpent)
        {
            User requestUser = service.RequestUser;
            User doneUser = service.DoneUser;

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

        public int acceptService(Service service)
        {
            using (var db = new DatabaseContext())
            {
                Service? resService = db.Services.Where((Service service) => service.Id.Equals(service.Id)).FirstOrDefault();
                if (resService != null)
                {
                    resService.DoneUser = AppStore.currentUser;
                    resService.State = ServiceState.Accepted;
                    int res = db.SaveChanges();

                    return res;
                }

                return -1;
            }
        }
        public void acceptServiceXml(Service service)
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

                if (serviceRow != null)
                {
                    serviceRow.BeginEdit();
                    serviceRow["doneUserId"] = AppStore.currentUser.Id;
                    serviceRow["state"] = ServiceState.Accepted;
                    serviceRow.EndEdit();

                    DataXml.writeDataXml(dataset);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Accept Service", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int removeService(Service service)
        {
            using (var db = new DatabaseContext())
            {
                db.Services.Remove(service);
                int res = db.SaveChanges();
                return res;
            }
        }

        public void removeServiceXml(Service service)
        {
            try
            {
                DataSet dataset = DataXml.readDataXml();

                for (int i = 0; i < dataset.Tables["Services"].Rows.Count; i++)
                {
                    if (dataset.Tables["Services"].Rows[i].ItemArray[0].Equals(service.Id.ToString()))
                    {
                        dataset.Tables["Services"].Rows[i].Delete();

                        DataXml.writeDataXml(dataset);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString(), "ERROR: Remove Service", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
