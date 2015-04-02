using System;
using System.Collections.Generic;

namespace LostAndFound.Models
{
    public abstract class Item
    {
        private DateTime DateReported { get; set; }

        private List<Tag> Tags { get; set; }

        private User Reportee { get; set; }
    }
}
