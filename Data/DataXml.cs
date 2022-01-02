using System;
using System.Data;
using System.IO;

namespace BankTimeNET.Data
{
    public static class DataXml
    {
        public static String dataXmlFilename = @".\BankTimeNET.xml";
        public static String dataSetName = "BankTimeNET";

        public static DataSet readDataXml()
        {
            DataSet dataSet = new DataSet();

            if (File.Exists(dataXmlFilename))
            {
                dataSet.ReadXml(dataXmlFilename);
            } else
            {
                dataSet = new DataSet(dataSetName);
            }

            if (dataSet.Tables["Users"] == null)
            {
                DataTable usersTable = new DataTable();
                usersTable.TableName = "Users";

                usersTable.Columns.Add("dni");
                usersTable.Columns.Add("name");
                usersTable.Columns.Add("password");
                usersTable.Columns.Add("amount");
                usersTable.Columns.Add("active");
                usersTable.Columns.Add("bankId");

                dataSet.Tables.Add(usersTable);
            }

            if (dataSet.Tables["Banks"] == null)
            {
                DataTable banksTable = new DataTable();
                banksTable.TableName = "Banks";

                banksTable.Columns.Add("place");

                dataSet.Tables.Add(banksTable);
            }

            if (dataSet.Tables["Services"] == null)
            {
                DataTable servicesTable = new DataTable();
                servicesTable.TableName = "Services";

                servicesTable.Columns.Add("id");
                servicesTable.Columns.Add("date");
                servicesTable.Columns.Add("description");
                servicesTable.Columns.Add("requestTime");
                servicesTable.Columns.Add("doneTime");
                servicesTable.Columns.Add("state");
                servicesTable.Columns.Add("requestUserId");
                servicesTable.Columns.Add("doneUserId");
                servicesTable.Columns.Add("bankId");

                dataSet.Tables.Add(servicesTable);
            }

            return dataSet;
        }

        public static void writeDataXml(DataSet dataset)
        {
            dataset.WriteXml(dataXmlFilename);
        }
    }
}
