using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using LostAndFound.Models;

namespace LostAndFound.Services.Providers
{
    class DisposedItemProvider
    {
        private readonly string itemsQuery = "Select * from [Disposed$]";

        private DataTable itemsTable;
        private OleDbConnection itemsConnection;
        private OleDbDataAdapter itemsAdapert;

        public DisposedItemProvider(string file = @"C:\Users\Bryan\Documents\LostAndFound\LostAndFound\LostAndFound\Resources\Disposal.xlsx")
        {
            this.itemsTable = new DataTable();
            var filePath = file;

            var source = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties= 'Excel 12.0 Xml;HDR=Yes;'";

            this.itemsConnection = new OleDbConnection(source);
            this.itemsAdapert = new OleDbDataAdapter(this.itemsQuery, this.itemsConnection);
            this.itemsAdapert.Fill(this.itemsTable);
        }

        public List<DisposedItem> GetDisposedItems()
        {
            var items = new List<DisposedItem>();

            for (var i = 0; i < itemsTable.Rows.Count; i++)
            {
                var drow = itemsTable.Rows[i];
                var dataArray = drow.ItemArray;
               
                var disposalDate = Convert.ToDateTime(dataArray[0].ToString());
                var date = Convert.ToDateTime(dataArray[1].ToString());

                var descriptionTags = GenerateDescriptionTagsFromString(dataArray[2].ToString());
                var locationTags = GenerateLocationTagsFromString(dataArray[3].ToString());

                var claimedBy = dataArray[4].ToString();
                var phone = dataArray[5].ToString();
                var email = dataArray[6].ToString();
                var disposedBy = dataArray[7].ToString();
                var disposalMethod = dataArray[8].ToString();

                var item = new DisposedItem(date, disposalDate, descriptionTags, locationTags, claimedBy, phone, email, disposedBy, disposalMethod);

                items.Add(item);
            }

            return items;
        }


        public DisposedItem CreateDisposedItem(DateTime date, DateTime disposalDate, string description, string location, string claimedBy, string phone, string email, string disposedBy, string disposalMethod)
        {
            var insertCommandString = "INSERT INTO [Disposed$] (DisposalDate, OriginalDate, ItemDescription, FoundLocation, ClaimedBy, PhoneNumber, Email, DisposedBy, DisposalMethod) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";
            this.itemsAdapert.InsertCommand = new OleDbCommand(insertCommandString, itemsConnection);

            itemsAdapert.InsertCommand.Parameters.Add("@DisposalDate", OleDbType.VarChar, 255).SourceColumn = "DisposalDate";
            itemsAdapert.InsertCommand.Parameters.Add("@OriginalDate", OleDbType.VarChar, 255).SourceColumn = "OriginalDate";
            itemsAdapert.InsertCommand.Parameters.Add("@ItemDescription", OleDbType.VarChar, 255).SourceColumn = "ItemDescription";
            itemsAdapert.InsertCommand.Parameters.Add("@FoundLocation", OleDbType.VarChar, 255).SourceColumn = "FoundLocation";
            itemsAdapert.InsertCommand.Parameters.Add("@ClaimedBy", OleDbType.VarChar, 255).SourceColumn = "ClaimedBy";
            itemsAdapert.InsertCommand.Parameters.Add("@PhoneNumber", OleDbType.VarChar, 255).SourceColumn = "PhoneNumber";
            itemsAdapert.InsertCommand.Parameters.Add("@Email", OleDbType.VarChar, 255).SourceColumn = "Email";
            itemsAdapert.InsertCommand.Parameters.Add("@DisposedBy", OleDbType.VarChar, 255).SourceColumn = "DisposedBy";
            itemsAdapert.InsertCommand.Parameters.Add("@DisposalMethod", OleDbType.VarChar, 255).SourceColumn = "DisposalMethod";

            DataRow newItemRow = itemsTable.NewRow();
            newItemRow["DisposalDate"] = disposalDate.ToString();
            newItemRow["OriginalDate"] = date.ToString();
            newItemRow["ItemDescription"] = description.ToLower();
            newItemRow["FoundLocation"] = location.ToLower();
            newItemRow["ClaimedBy"] = claimedBy;
            newItemRow["PhoneNumber"] = phone;
            newItemRow["Email"] = email;
            newItemRow["DisposedBy"] = disposedBy;
            newItemRow["DisposalMethod"] = disposalMethod;

            itemsTable.Rows.Add(newItemRow);
            itemsAdapert.Update(itemsTable);

            var descriptionTags = GenerateDescriptionTagsFromString(description);
            var locationTags = GenerateLocationTagsFromString(location);

            var newItem = new DisposedItem(date, disposalDate, descriptionTags, locationTags, claimedBy, phone, email, disposedBy, disposalMethod);

            return newItem;
        }

        public void DeleteDisposedItem(DisposedItem oldDisposedItem)
        {

        }

        public void UpdateDisposedItem(DisposedItem oldDisposedItem, DisposedItem newDisposedItem)
        {
            //DisposalDate, OriginalDate, ItemDescription, FoundLocation, ClaimedBy, PhoneNumber, Email, DisposedBy, DisposalMethod
            var updateCommandString = "UPDATE [Disposed$] SET DisposalDate = '" + newDisposedItem.Date + "', OriginalDate = '" + newDisposedItem.DateReported + "', ItemDescription = '" + newDisposedItem.DescriptionTags +
                                      "', FoundLocation = '" + newDisposedItem.LocationTags + "', ClaimedBy = '" + newDisposedItem.claimedBy + "', PhoneNumber = '" + newDisposedItem.PhoneNumber +
                                       "', Email = '" + newDisposedItem.Email + "', DisposedBy = '" + newDisposedItem.disposedBy + "', DisposalMethod = '" + newDisposedItem.disposalMethod + "' " +
                                      "WHERE DisposalDate = '" + oldDisposedItem.Date + "', OriginalDate = '" + oldDisposedItem.DateReported + "', ItemDescription = '" + oldDisposedItem.DescriptionTags +
                                      "', FoundLocation = '" + oldDisposedItem.LocationTags + "', ClaimedBy = '" + oldDisposedItem.claimedBy + "', PhoneNumber = '" + oldDisposedItem.PhoneNumber +
                                       "', Email = '" + oldDisposedItem.Email + "', DisposedBy = '" + oldDisposedItem.disposedBy + "', DisposalMethod = '" + oldDisposedItem.disposalMethod + "'";
            OleDbCommand myCommand = new OleDbCommand();
            itemsConnection.Open();
            myCommand.Connection = itemsConnection;
            myCommand.CommandText = updateCommandString;
            myCommand.ExecuteNonQuery();
            itemsConnection.Close();
        }

        //These generate Tag methods could be moved to a factory or constructor or something
        //Todo: Figure out how to do this kind of Generic creating
        //private List<T> GenerateTagsFromString<T>(string tagString)
        //{
        //    var tagArray = tagString.Split(' ');

        //    var tags = new List<T>();

        //    for (var j = 0; j < tagArray.Length; j++)
        //    {
        //        var t = T(tagArray[j]); //<-- This is the broken part!!!
        //        tags.Add(t);
        //    }
        //}


        private List<DescriptionTag> GenerateDescriptionTagsFromString(string tagString)
        {
            var tagArray = tagString.Split(' ');

            var tags = new List<DescriptionTag>();

            for (var j = 0; j < tagArray.Length; j++)
            {
                var t = new DescriptionTag(tagArray[j]);
                tags.Add(t);
            }

            return tags;
        }

        private List<LocationTag> GenerateLocationTagsFromString(string tagString)
        {
            var tagArray = tagString.Split(' ');

            var tags = new List<LocationTag>();

            for (var j = 0; j < tagArray.Length; j++)
            {
                var t = new LocationTag(tagArray[j]);
                tags.Add(t);
            }

            return tags;
        }


    }
}
