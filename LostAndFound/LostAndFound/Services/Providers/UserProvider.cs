using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using LostAndFound.Models;

namespace LostAndFound.Services.Providers
{
    class UserProvider
    {
        private readonly string usersQuery = "Select * from [Users$]";
        
        private DataTable usersTable;
        private OleDbConnection usersConnection;
        private OleDbDataAdapter usersAdapter;

        public UserProvider(string file = @"C:\Users\Bryan\Documents\LostAndFound\LostAndFound\LostAndFound\Resources\LostAndFound.xlsx")
        {
            this.usersTable = new DataTable();
            var filePath = file;

            //To get this to work you need to download and install the following: http://www.microsoft.com/en-us/download/details.aspx?id=13255
            var SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties= 'Excel 12.0 Xml;HDR=Yes;'";

            this.usersConnection = new OleDbConnection(SourceConstr);

            this.usersAdapter = new OleDbDataAdapter(usersQuery, usersConnection);
            this.usersAdapter.Fill(usersTable);
        }

        public List<User> GetUsers()
        {
            var Users = new List<User>();

            for (int i = 0; i < usersTable.Rows.Count; i++)
            {
                DataRow drow = usersTable.Rows[i];
                var dataArray = drow.ItemArray;

                var user = new User(dataArray[0].ToString(), dataArray[3].ToString())
                {
                    LastName = dataArray[1].ToString(),
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

            DataRow newPersonRow = usersTable.NewRow();
            newPersonRow["FirstName"] = firstName;
            newPersonRow["LastName"] = lastName;
            newPersonRow["PhoneNumber"] = phone;

            usersTable.Rows.Add(newPersonRow);
            usersAdapter.Update(usersTable);

            User newUser = new User(firstName, phone)
            {
                LastName = lastName
            };

            return newUser;
        }

        public void UpdateUser(User oldUser, User newUser)
        {
            var updateCommandString = "UPDATE [Users$] SET FirstName = '" + newUser.FirstName + "', LastName = '" + newUser.LastName + "', PhoneNumber = '" + newUser.PhoneNumber + "' " +
                                      "WHERE FirstName = '" + oldUser.FirstName + "' AND LastName = '" + oldUser.LastName + "' AND PhoneNumber = '" + oldUser.PhoneNumber + "'";
            //usersAdapter.UpdateCommand = new OleDbCommand(updateCommandString, usersConnection);
            OleDbCommand myCommand = new OleDbCommand();
            //RANDAY - How do we know what row this User came from? Should we first run a query to find out the row number, orrr...? (Nora was here)
            usersConnection.Open();
            myCommand.Connection = usersConnection;
            myCommand.CommandText = updateCommandString;
            myCommand.ExecuteNonQuery();
            usersConnection.Close();
        }
    }
}
