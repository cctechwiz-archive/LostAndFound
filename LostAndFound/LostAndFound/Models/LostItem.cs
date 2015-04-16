using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    public class LostItem : Item
    {
        public LostItem(DateTime date, List<DescriptionTag> descTags, User owner)
        {
            this.DateReported = date;
            this.DescriptionTags = descTags;
            this.Owner = owner;
        }
    }
}
