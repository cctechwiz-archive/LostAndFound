using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using LostAndFound.Models;

namespace LostAndFound.Services.Providers
{
    class LostItemProvider
    {
        private readonly string itemsQuery = "Select * from [LostReports$]";

        private DataTable itemsTable;
        private OleDbConnection itemsConnection;
        private OleDbDataAdapter itemsAdapert;

        public LostItemProvider(string file = @"G:\GitHub\LostAndFound\LostAndFound\LostAndFound\Resources\LostAndFoundDatabase.xlsx")
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

                var name = dataArray[3].ToString();
                var employee = dataArray[6].ToString();

                var item = new LostItem(date, descriptionTags, locationTags, name, employee);

                if (date.Year != 1)
                {
                    items.Add(item);
                }
            }

            return items;
        }


        public LostItem CreateLostItem(DateTime date, string description, string location, string foundBy, string phonenumber, string email, string recordedBy)
        {
            try
            {
               var insertCommandString = "INSERT INTO [LostReports$] (DateLost, ItemDescription, LocationLost, LostBy, PhoneNumber, EmailAddress, RecordedBy) VALUES " +
               "('" + date.ToString("MM/dd/yyyy") + "', '" + description + "', '" + location + "', '" + foundBy + "', '" + phonenumber + "', '" + email + "', '" + recordedBy + "')";


                itemsConnection.Open();
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = itemsConnection;
                myCommand.CommandText = insertCommandString;
                myCommand.ExecuteNonQuery();
                itemsConnection.Close();

                var descriptionTags = GenerateDescriptionTagsFromString(description);
                var locationTags = GenerateLocationTagsFromString(location);
                var lostItem = new LostItem(date, descriptionTags, locationTags, foundBy, recordedBy);
                return lostItem;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return null;
        }

        public void UpdateLostItem(LostItem oldLostItem, LostItem newLostItem)
        {
            DescriptionTag[] oldDescs = oldLostItem.DescriptionTags.ToArray();
            string oldDesc = "";
            for (int i = 0; i < oldDescs.Length; i++)
            {
                oldDesc += oldDescs[i].Name;
                if (i < oldDescs.Length - 1) oldDesc += " ";
            }
            DescriptionTag[] newDescs = newLostItem.DescriptionTags.ToArray();
            string newDesc = "";
            for (int i = 0; i < newDescs.Length; i++)
            {
                newDesc += newDescs[i].Name;
                if (i < newDescs.Length - 1) newDesc += " ";
            }

            LocationTag[] oldLocs = oldLostItem.LocationTags.ToArray();
            string oldLoc = "";
            for (int i = 0; i < oldLocs.Length; i++)
            {
                oldLoc += oldLocs[i].Name;
                if (i < oldLocs.Length - 1) oldLoc += " ";
            }
            LocationTag[] newLocs = newLostItem.LocationTags.ToArray();
            string newLoc = "";
            for (int i = 0; i < newLocs.Length; i++)
            {
                newLoc += newLocs[i].Name;
                if (i < newLocs.Length - 1) newLoc += " ";
            }

            var updateCommandString = "UPDATE [LostReports$] SET DateLost = '" + newLostItem.DateReported.ToString("MM/dd/yyyy")
                + "', ItemDescription = '" + newDesc + "', LocationLost = '" + newLoc
                + "', LostBy = '" + newLostItem.Name + "', RecordedBy = '" + newLostItem.Employee + "' " +
                "WHERE DateLost = '" + oldLostItem.DateReported.ToString("MM/dd/yyyy")
                + "' AND ItemDescription = '" + oldDesc + "' AND LocationLost = '" + oldLoc
                + "' AND LostBy = '" + oldLostItem.Name + "' AND RecordedBy = '" + oldLostItem.Employee + "'";
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
