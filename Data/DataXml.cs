using System;
using System.Data;
using System.IO;

namespace BankTimeNET.Data
{
    public static class DataXml
    {
        public static String dataXmlFilename = @"d:\BankTimeNET.xml";
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

            return dataSet;
        }

        public static void writeDataXml(DataSet dataset)
        {
            dataset.WriteXml(dataXmlFilename);
        }
    }
}
