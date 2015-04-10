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

        public ExcelProvider(string file = @"C:\Users\Bryan\Documents\LostAndFound\LostAndFound\LostAndFound\Resources\LostAndFound.xlsx")
        {
            this.dtExcel = new DataTable();
            var filePath = file;

            //To get this to work you need to download and install the following: http://www.microsoft.com/en-us/download/details.aspx?id=13255
            var SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties= 'Excel 12.0 Xml;HDR=Yes;'";

            this.usersConnection = new OleDbConnection(SourceConstr);

            this.usersAdapter = new OleDbDataAdapter(usersQuery, usersConnection);
            this.usersAdapter.Fill(dtExcel);
        }

        public List<User> GetUsers()
        {
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
            var insertCommandString = "INSERT INTO [Users$] (FirstName, LastName, PhoneNumber) VALUES (?, ?, ?)";
            usersAdapter.InsertCommand = new OleDbCommand(insertCommandString, usersConnection);

            usersAdapter.InsertCommand.Parameters.Add("@FirstName", OleDbType.VarChar, 255).SourceColumn = "FirstName";
            usersAdapter.InsertCommand.Parameters.Add("@LastName", OleDbType.VarChar, 255).SourceColumn = "LastName";
            usersAdapter.InsertCommand.Parameters.Add("@PhoneNumber", OleDbType.VarChar, 20).SourceColumn = "PhoneNumber";

            DataRow newPersonRow = dtExcel.NewRow();
            newPersonRow["FirstName"] = firstName;
            newPersonRow["LastName"] = lastName;
            newPersonRow["PhoneNumber"] = phone;

            dtExcel.Rows.Add(newPersonRow);
            usersAdapter.Update(dtExcel);

            User newUser = new User(firstName, lastName, phone);

            return newUser;
        }

        public void UpdateUser(User oldUser, User newUser)
        {
            var updateCommandString = "UPDATE [Users$] SET FirstName = '" + newUser.FirstName + "', LastName = '" + newUser.LastName + "', PhoneNumber = '" + newUser.PhoneNumber + "' " +
                                      "WHERE FirstName = '" + oldUser.FirstName + "' AND LastName = '" + oldUser.LastName + "' AND PhoneNumber = '" + oldUser.PhoneNumber + "'";
            //usersAdapter.UpdateCommand = new OleDbCommand(updateCommandString, usersConnection);
            System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
            //RANDAY - How do we know what row this User came from? Should we first run a query to find out the row number, orrr...? (Nora was here)
            usersConnection.Open();
            myCommand.Connection = usersConnection;
            myCommand.CommandText = updateCommandString;
            myCommand.ExecuteNonQuery();
            usersConnection.Close();
        }
    }
}
