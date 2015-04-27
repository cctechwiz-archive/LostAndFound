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

        public DisposedItemProvider(string file = @"..\..\Resources\Disposal.xlsx")
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

                if (date.Year != 1)
                {
                    items.Add(item);
                }
            }

            return items;
        }


        public DisposedItem CreateDisposedItem(DateTime date, DateTime disposalDate, string description, string location, string claimedBy, string phone, string email, string disposedBy, string disposalMethod)
        {
            try
            {
                var insertCommandString = "INSERT INTO [Disposed$] (DisposalDate, OriginalDate, ItemDescription, FoundLocation, ClaimedBy, PhoneNumber, Email, DisposedBy, DisposalMethod) VALUES " +
                "('" + disposalDate.ToString("MM/dd/yyyy") + "', '" + date.ToString("MM/dd/yyyy") + "', '" + description + "', '" + location + "', '" + claimedBy + "', '" + phone + "', '" + email + "', '" + disposedBy + "', '" + disposalMethod + "')";


                itemsConnection.Open();
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = itemsConnection;
                myCommand.CommandText = insertCommandString;
                myCommand.ExecuteNonQuery();
                itemsConnection.Close();

                var descriptionTags = GenerateDescriptionTagsFromString(description);
                var locationTags = GenerateLocationTagsFromString(location);
                var disposedItem = new DisposedItem(date, disposalDate ,descriptionTags, locationTags, claimedBy, phone, email, disposedBy, disposalMethod);
                return disposedItem;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return null;
        }

        public void UpdateDisposedItem(DisposedItem oldDisposedItem, DisposedItem newDisposedItem)
        {
            DescriptionTag[] oldDescs = oldDisposedItem.DescriptionTags.ToArray();
            string oldDesc = "";
            for (int i = 0; i < oldDescs.Length; i++)
            {
                oldDesc += oldDescs[i].Name;
                if (i < oldDescs.Length - 1) oldDesc += " ";
            }
            DescriptionTag[] newDescs = newDisposedItem.DescriptionTags.ToArray();
            string newDesc = "";
            for (int i = 0; i < newDescs.Length; i++)
            {
                newDesc += newDescs[i].Name;
                if (i < newDescs.Length - 1) newDesc += " ";
            }

            LocationTag[] oldLocs = oldDisposedItem.LocationTags.ToArray();
            string oldLoc = "";
            for (int i = 0; i < oldLocs.Length; i++)
            {
                oldLoc += oldLocs[i].Name;
                if (i < oldLocs.Length - 1) oldLoc += " ";
            }
            LocationTag[] newLocs = newDisposedItem.LocationTags.ToArray();
            string newLoc = "";
            for (int i = 0; i < newLocs.Length; i++)
            {
                newLoc += newLocs[i].Name;
                if (i < newLocs.Length - 1) newLoc += " ";
            }
            //DisposalDate, OriginalDate, ItemDescription, FoundLocation, ClaimedBy, PhoneNumber, Email, DisposedBy, DisposalMethod
            var updateCommandString = "UPDATE [Disposed$] SET DisposalDate = '" + newDisposedItem.Date.ToString("MM/dd/yyyy") + "', OriginalDate = '" + newDisposedItem.DateReported.ToString("MM/dd/yyyy") + "', ItemDescription = '" + newDesc +
                                      "', FoundLocation = '" + newLoc + "', ClaimedBy = '" + newDisposedItem.claimedBy + "', PhoneNumber = '" + newDisposedItem.PhoneNumber +
                                       "', Email = '" + newDisposedItem.Email + "', DisposedBy = '" + newDisposedItem.disposedBy + "', DisposalMethod = '" + newDisposedItem.disposalMethod + "' " +
                                      "WHERE DisposalDate = '" + oldDisposedItem.Date.ToString("MM/dd/yyyy") + "' AND OriginalDate = '" + oldDisposedItem.DateReported.ToString("MM/dd/yyyy") + "' AND ItemDescription = '" + oldDesc +
                                      "' AND FoundLocation = '" + oldLoc + "' AND ClaimedBy = '" + oldDisposedItem.claimedBy + "' AND PhoneNumber = '" + oldDisposedItem.PhoneNumber +
                                       "' AND Email = '" + oldDisposedItem.Email + "' AND DisposedBy = '" + oldDisposedItem.disposedBy + "' AND DisposalMethod = '" + oldDisposedItem.disposalMethod + "'";
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
