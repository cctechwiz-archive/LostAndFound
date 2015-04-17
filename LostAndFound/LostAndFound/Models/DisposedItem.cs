using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    public class DisposedItem : Item
    {
        public DateTime Date { get; set; }

        //private List<DisposedTag> Tags { get; set; }

        public string claimedBy;

        public string disposedBy;

        public string disposalMethod;

        public DisposedItem(DateTime date, DateTime disposalDate, List<DescriptionTag> descTags, List<LocationTag> locTags, string claimedBy, string phone, string email, string disposedBy, string disposalMethod)
        {
            this.DateReported = date;
            this.Date = disposalDate;
            this.DescriptionTags = descTags;
            this.LocationTags = locTags;
            this.claimedBy = claimedBy;
            this.PhoneNumber = phone;
            this.Email = email;
            this.disposedBy = disposedBy;
            this.disposalMethod = disposalMethod;
        }
    }
}
