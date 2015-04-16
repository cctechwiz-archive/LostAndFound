using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    public class FoundItem : Item
    {
        public string Reportee { get; set; }

        public FoundItem(DateTime date, List<DescriptionTag> descTags, List<LocationTag> locTags, string reportee, string employee)
        {
            this.DateReported = date;
            this.DescriptionTags = descTags;
            this.LocationTags = locTags;
            this.Reportee = reportee;
            this.Employee = employee;
        }

    }
}
