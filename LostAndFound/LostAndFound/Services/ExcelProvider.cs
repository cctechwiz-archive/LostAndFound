using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Dynamic;
using System.Windows.Documents;
using LostAndFound.Models;

namespace LostAndFound.Services
{
    class ExcelProvider
    {
        private readonly string usersQuery = "Select * from [Users$]";
        
        private DataTable dtExcel;
        private OleDbConnection usersConnection;
        private OleDbDataAdapter usersAdapter;

        public ExcelProvider(string file = @"C:\git\LostAndFound\LostAndFound\LostAndFound\Resources\LostAndFound.xlsx")
        {
            this.dtExcel = new DataTable();
            var filePath = file;

            //To get this to work you need to download and install the following: http://www.microsoft.com/en-us/download/details.aspx?id=13255
            var SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties= 'Excel 12.0 Xml;HDR=Yes;'";

            this.usersConnection = new OleDbConnection(SourceConstr);

            this.usersAdapter = new OleDbDataAdapter(usersQuery, usersConnection);
        }

        public List<User> GetUsers()
        {
            OleDbDataAdapter data = new OleDbDataAdapter(usersQuery, usersConnection);

            data.Fill(dtExcel);

            var Users = new List<User>();

            for (int i = 0; i < dtExcel.Rows.Count; i++)
            {
                DataRow drow = dtExcel.Rows[i];
                var dataArray = drow.ItemArray;

                var user = new User(dataArray[0].ToString(), dataArray[1].ToString(), dataArray[3].ToString())
                {
                    Email = dataArray[2].ToString(),
                    TNumber = dataArray[4].ToString()
                };
                Users.Add(user);
            }

            return Users;
        }

        public User CreateNewUser(string firstName, string lastName, string phone)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter(usersQuery, usersConnection);

            adapter.Fill(dtExcel);

            var insertCommandString = "INSERT INTO [Users$] (FirstName, LastName, PhoneNumber) VALUES (?, ?, ?)";
            adapter.InsertCommand = new OleDbCommand(insertCommandString, usersConnection);

            adapter.InsertCommand.Parameters.Add("@FirstName", OleDbType.VarChar, 255).SourceColumn = "FirstName";
            adapter.InsertCommand.Parameters.Add("@LastName", OleDbType.VarChar, 255).SourceColumn = "LastName";
            adapter.InsertCommand.Parameters.Add("@PhoneNumber", OleDbType.VarChar, 20).SourceColumn = "PhoneNumber";

            DataRow newPersonRow = dtExcel.NewRow();
            newPersonRow["FirstName"] = firstName;
            newPersonRow["LastName"] = lastName;
            newPersonRow["PhoneNumber"] = phone;

            dtExcel.Rows.Add(newPersonRow);
            adapter.Update(dtExcel);

            User newUser = new User(firstName, lastName, phone);

            return newUser;
        }
    }
}
