using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using LostAndFound.Models;

namespace LostAndFound.Services
{
    class ItemProvider
    {
        private readonly string itemsQuery = "Select * from [LostReports$]";

        private DataTable itemsTable;
        private OleDbConnection itemsConnection;
        private OleDbDataAdapter itemsAdapert;

        public ItemProvider(string file = @"C:\Users\Bryan\Documents\LostAndFound\LostAndFound\LostAndFound\Resources\LostAndFound.xlsx")
        {
            this.itemsTable = new DataTable();
            var filePath = file;

            var source = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties= 'Excel 12.0 Xml;HDR=Yes;'";

            this.itemsConnection = new OleDbConnection(source);
            this.itemsAdapert = new OleDbDataAdapter(this.itemsQuery, this.itemsConnection);
            this.itemsAdapert.Fill(this.itemsTable);
        }

        public List<LostItem> GetLostItems()
        {
            var items = new List<LostItem>();

            for (var i = 0; i < itemsTable.Rows.Count; i++)
            {
                var drow = itemsTable.Rows[i];
                var dataArray = drow.ItemArray;

                var date = Convert.ToDateTime(dataArray[0].ToString());

                var descriptionTags = GenerateDescriptionTagsFromString(dataArray[1].ToString());
                var locationTags = GenerateLocationTagsFromString(dataArray[2].ToString());

                var name = dataArray[3].ToString().Split(' ');
                var user = new User(name[0], dataArray[4].ToString())
                {
                    Email = dataArray[5].ToString(),
                };
                if(null != name[1])
                {
                    user.LastName = name[1];
                }

                var item = new LostItem(date, descriptionTags, user)
                {
                    LocationTags = locationTags,
                    Recorder = dataArray[6].ToString()
                };

                items.Add(item);
            }

            return items;
        }


        public LostItem CreateLostItem(DateTime date, string description, string location, User owner, string recorder )
        {
            var insertCommandString = "INSERT INTO [LostReports$] (Date, ItemDescription, LocationLost, LostBy, PhoneNumber, EmailAddress, RecordedBy) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
            this.itemsAdapert.InsertCommand = new OleDbCommand(insertCommandString, itemsConnection);

            itemsAdapert.InsertCommand.Parameters.Add("@Date", OleDbType.VarChar, 255).SourceColumn = "Date";
            itemsAdapert.InsertCommand.Parameters.Add("@ItemDescription", OleDbType.VarChar, 255).SourceColumn = "ItemDescription";
            itemsAdapert.InsertCommand.Parameters.Add("@LocationLost", OleDbType.VarChar, 255).SourceColumn = "LocationLost";
            itemsAdapert.InsertCommand.Parameters.Add("@LostBy", OleDbType.VarChar, 255).SourceColumn = "LostBy";
            itemsAdapert.InsertCommand.Parameters.Add("@PhoneNumber", OleDbType.VarChar, 255).SourceColumn = "PhoneNumber";
            itemsAdapert.InsertCommand.Parameters.Add("@EmailAddress", OleDbType.VarChar, 255).SourceColumn = "EmailAddress";
            itemsAdapert.InsertCommand.Parameters.Add("@RecordedBy", OleDbType.VarChar, 255).SourceColumn = "RecordedBy";

            DataRow newItemRow = itemsTable.NewRow();
            newItemRow["Date"] = date.ToString();
            newItemRow["ItemDescription"] = description;
            newItemRow["LocationLost"] = location;
            newItemRow["LostBy"] = owner;
            newItemRow["PhoneNumber"] = owner.PhoneNumber;
            newItemRow["EmailAddress"] = owner.Email;
            newItemRow["RecordedBy"] = recorder;

            itemsTable.Rows.Add(newItemRow);
            itemsAdapert.Update(itemsTable);

            var descriptionTags = GenerateDescriptionTagsFromString(description);
            var locationTags = GenerateLocationTagsFromString(location);

            LostItem newItem = new LostItem();

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
