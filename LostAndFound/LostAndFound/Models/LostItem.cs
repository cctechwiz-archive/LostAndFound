using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    public class LostItem : Item
    {
        public LostItem(DateTime date, List<DescriptionTag> descTags, List<LocationTag> locTags, string name, string employee)
        {
            this.DateReported = date;
            this.DescriptionTags = descTags;
            this.LocationTags = locTags;
            this.Name = name;
            this.Employee = employee;
        }
    }
}
