using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    public class LostItem : Item
    {
        public User Owner { get; set; }

        public LostItem(DateTime date, List<DescriptionTag> descTags, List<LocationTag> locTags, User owner, string recorder)
        {
            this.DateReported = date;
            this.DescriptionTags = descTags;
            this.LocationTags = locTags;
            this.Owner = owner;
            this.Recorder = recorder;
        }
    }
}
