using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using LostAndFound.Models;

namespace LostAndFound.Services.Providers
{
    class FoundItemProvider
    {
        private readonly string itemsQuery = "Select * from [FoundItems$]";

        private DataTable itemsTable;
        private OleDbConnection itemsConnection;
        private OleDbDataAdapter itemsAdapert;

        public FoundItemProvider(string file = @"C:\Users\Bryan\Documents\LostAndFound\LostAndFound\LostAndFound\Resources\LostAndFoundDatabase.xlsx")
        {
            this.itemsTable = new DataTable();
            var filePath = file;

            var source = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties= 'Excel 12.0 Xml;HDR=Yes;'";

            this.itemsConnection = new OleDbConnection(source);
            this.itemsAdapert = new OleDbDataAdapter(this.itemsQuery, this.itemsConnection);
            this.itemsAdapert.Fill(this.itemsTable);
        }

        public List<FoundItem> GetFoundItems()
        {
            var items = new List<FoundItem>();

            for (var i = 0; i < itemsTable.Rows.Count; i++)
            {
                var drow = itemsTable.Rows[i];
                var dataArray = drow.ItemArray;

                var date = Convert.ToDateTime(dataArray[0].ToString());

                var descriptionTags = GenerateDescriptionTagsFromString(dataArray[1].ToString());
                var locationTags = GenerateLocationTagsFromString(dataArray[2].ToString());
                 
                var foundBy = dataArray[3].ToString();
                var recordedBy = dataArray[4].ToString();

                var item = new FoundItem(date, descriptionTags, locationTags, foundBy, recordedBy);

                items.Add(item);
            }

            return items;
        }


        public FoundItem CreateFoundItem(DateTime date, string description, string location, string foundBy, string recordedBy)
        {
            var insertCommandString = "INSERT INTO [FoundItems$] (Date, ItemDescription, LocationItemFound, FoundBy, RecordedBy) VALUES (?, ?, ?, ?, ?, ?)";
            this.itemsAdapert.InsertCommand = new OleDbCommand(insertCommandString, itemsConnection);

            itemsAdapert.InsertCommand.Parameters.Add("@Date", OleDbType.VarChar, 255).SourceColumn = "Date";
            itemsAdapert.InsertCommand.Parameters.Add("@ItemDescription", OleDbType.VarChar, 255).SourceColumn = "ItemDescription";
            itemsAdapert.InsertCommand.Parameters.Add("@LocationItemFound", OleDbType.VarChar, 255).SourceColumn = "LocationItemFound";
            itemsAdapert.InsertCommand.Parameters.Add("@FoundBy", OleDbType.VarChar, 255).SourceColumn = "FoundBy";
            itemsAdapert.InsertCommand.Parameters.Add("@RecordedBy", OleDbType.VarChar, 255).SourceColumn = "RecordedBy";

            DataRow newItemRow = itemsTable.NewRow();
            newItemRow["Date"] = date.ToString();
            newItemRow["ItemDescription"] = description.ToLower();
            newItemRow["LocationItemFound"] = location.ToLower();
            newItemRow["FoundBy"] = foundBy;
            newItemRow["RecordedBy"] = recordedBy;

            itemsTable.Rows.Add(newItemRow);
            itemsAdapert.Update(itemsTable);

            var descriptionTags = GenerateDescriptionTagsFromString(description);
            var locationTags = GenerateLocationTagsFromString(location);

            var newItem = new FoundItem(date, descriptionTags, locationTags, foundBy, recordedBy);

            return newItem;
        }

        public void UpdateLostItem(FoundItem oldFoundItem, FoundItem newFoundItem)
        {
            var updateCommandString = "UPDATE [LostReports$] SET Date = '" + newFoundItem.DateReported + "', ItemDescription = '" + newFoundItem.DescriptionTags + "', LocationLost = '" + newFoundItem.LocationTags +
                                      "', FoundBy = '" + newFoundItem.Reportee + "' " +  "', RecordedBy = '" + newFoundItem.Employee + "' " +
                                      "WHERE Date = '" + oldFoundItem.DateReported + "', ItemDescription = '" + oldFoundItem.DescriptionTags + "', LocationLost = '" + oldFoundItem.LocationTags +
                                      "', FoundBy = '" + oldFoundItem.Reportee + "' " + "', RecordedBy = '" + oldFoundItem.Employee + "'";
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
