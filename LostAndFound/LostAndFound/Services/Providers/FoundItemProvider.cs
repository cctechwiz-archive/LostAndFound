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

        public FoundItemProvider(string file = @"..\..\Resources\LostAndFoundDatabase.xlsx")
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

                var dateValue = dataArray[0].ToString();
                var date = Convert.ToDateTime(dataArray[0].ToString());

                var descriptionTags = GenerateDescriptionTagsFromString(dataArray[1].ToString());
                var locationTags = GenerateLocationTagsFromString(dataArray[2].ToString());
                 
                var foundBy = dataArray[3].ToString();
                var recordedBy = dataArray[4].ToString();

                var item = new FoundItem(date, descriptionTags, locationTags, foundBy, recordedBy);
                if (date.Year != 1)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public FoundItem CreateFoundItem(DateTime date, string description, string location, string foundBy, string recordedBy)
        {
            try
            {
               var insertCommandString = "INSERT INTO [FoundItems$] (DateFound, ItemDescription, LocationItemFound, FoundBy, RecordedBy) VALUES " +
               "('" + date.ToString("MM/dd/yyyy") + "', '" + description + "', '" + location + "', '" + foundBy + "', '" + recordedBy + "')";


                itemsConnection.Open();
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = itemsConnection;
                myCommand.CommandText = insertCommandString;
                myCommand.ExecuteNonQuery();
                itemsConnection.Close();


                var descriptionTags = GenerateDescriptionTagsFromString(description);
                var locationTags = GenerateLocationTagsFromString(location);
                var newItem = new FoundItem(date, descriptionTags, locationTags, foundBy, recordedBy);
                return newItem;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return null;
        }

        public void UpdateFoundItem(FoundItem oldFoundItem, FoundItem newFoundItem)
        {
            DescriptionTag[] oldDescs = oldFoundItem.DescriptionTags.ToArray();
            string oldDesc = "";
            for (int i = 0; i < oldDescs.Length; i++)
            {
                oldDesc += oldDescs[i].Name;
                if (i < oldDescs.Length - 1) oldDesc += " ";
            }
            DescriptionTag[] newDescs = newFoundItem.DescriptionTags.ToArray();
            string newDesc = "";
            for (int i = 0; i < newDescs.Length; i++)
            {
                newDesc += newDescs[i].Name;
                if (i < newDescs.Length - 1) newDesc += " ";
            }

            LocationTag[] oldLocs = oldFoundItem.LocationTags.ToArray();
            string oldLoc = "";
            for (int i = 0; i < oldLocs.Length; i++)
            {
                oldLoc += oldLocs[i].Name;
                if (i < oldLocs.Length - 1) oldLoc += " ";
            }
            LocationTag[] newLocs = newFoundItem.LocationTags.ToArray();
            string newLoc = "";
            for (int i = 0; i < newLocs.Length; i++)
            {
                newLoc += newLocs[i].Name;
                if (i < newLocs.Length - 1) newLoc += " ";
            }

            var updateCommandString = "UPDATE [FoundItems$] SET DateFound = '" + newFoundItem.DateReported.ToString("MM/dd/yyyy") + "', ItemDescription = '" + newDesc + "', LocationItemFound = '" + newLoc +
                "', FoundBy = '" + newFoundItem.Reportee + "', RecordedBy = '" + newFoundItem.Employee + "' " +
                "WHERE DateFound = '" + oldFoundItem.DateReported.ToString("MM/dd/yyyy") + "' AND ItemDescription = '" + oldDesc + "' AND LocationItemFound = '" + oldLoc +
                "'";
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
